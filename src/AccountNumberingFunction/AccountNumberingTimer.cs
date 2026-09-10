using System.Globalization;
using System.Text.RegularExpressions;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Microsoft.Identity.Client;
using Microsoft.PowerPlatform.Dataverse.Client;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;

public sealed class AccountNumberingTimer(ILogger<AccountNumberingTimer> logger)
{
    [Function("NumberDataverseAccounts")]
    public void Run([TimerTrigger("%SMARTPOINT_ACCOUNT_NUMBERING_SCHEDULE%", RunOnStartup = false)] TimerInfo timer)
    {
        logger.LogInformation("Account numbering invocation started. Past due: {IsPastDue}", timer.IsPastDue);
        int updated = 0;
        string stage = "checking configuration";
        var diagnosticSecrets = new List<string>();

        string Redact(string message)
        {
            foreach (string value in diagnosticSecrets)
            {
                message = message.Replace(value, "[REDACTED]", StringComparison.Ordinal)
                    .Replace(Uri.EscapeDataString(value), "[REDACTED]", StringComparison.Ordinal);
            }
            // Omit entire lines containing credential fields or connection strings.
            message = Regex.Replace(message,
                @"^.*(?:client[_\s]*secret|password|pwd\s*=|authorization|bearer\s|access[_\s]*token|refresh[_\s]*token|id[_\s]*token|client[_\s]*assertion|connection\s*string|AuthType\s*=|ClientId\s*=|AccountKey\s*=).*$",
                "[Credential-bearing diagnostic line omitted]", RegexOptions.IgnoreCase | RegexOptions.Multiline);
            return Regex.Replace(message, @"\beyJ[A-Za-z0-9_-]+\.[A-Za-z0-9_-]+(?:\.[A-Za-z0-9_-]+)?", "[REDACTED TOKEN]");
        }

        void ReportException(Exception error, string label = "Exception")
        {
            logger.LogError("{Diagnostic}", $"{label}: {error.GetType().FullName}: {Redact(error.Message)}");
            if (error is AggregateException aggregate)
            {
                foreach (var inner in aggregate.InnerExceptions)
                    ReportException(inner, "Inner exception");
            }
            else if (error.InnerException is { } inner)
                ReportException(inner, "Inner exception");
        }

        try
        {
            const int start = 1;
            stage = "checking configuration";
            string Required(string key) => Environment.GetEnvironmentVariable(key) is { } value &&
                !string.IsNullOrWhiteSpace(value) ? value : throw new ArgumentException($"Set {key} before running.");

            string url = Required("SMARTPOINT_DATAVERSE_URL");
            string tenant = Required("SMARTPOINT_TENANT_ID");
            string clientId = Required("SMARTPOINT_CLIENT_ID");
            string secret = Required("SMARTPOINT_CLIENT_SECRET");
            diagnosticSecrets.Add(secret);
            if (!Uri.TryCreate(url, UriKind.Absolute, out var uri) || uri.Scheme != "https" ||
                uri.AbsolutePath != "/" || uri.Query.Length != 0 || uri.Fragment.Length != 0 || uri.UserInfo.Length != 0)
                throw new ArgumentException("SMARTPOINT_DATAVERSE_URL must be an HTTPS organization root URL.");
            if (!Guid.TryParse(tenant, out _) || !Guid.TryParse(clientId, out _))
                throw new ArgumentException("SMARTPOINT_TENANT_ID and SMARTPOINT_CLIENT_ID must be GUIDs.");

            stage = "connecting to Dataverse";
            logger.LogInformation("{Progress}", "Connecting to Dataverse...");
            // MSAL is supplied by the Dataverse client package. Tokens stay in its in-memory cache.
            var identity = ConfidentialClientApplicationBuilder.Create(clientId)
                .WithAuthority($"https://login.microsoftonline.com/{tenant}")
                .WithClientSecret(secret).Build();
            string[] scopes = [$"{uri.GetLeftPart(UriPartial.Authority)}/.default"];
            using var client = new ServiceClient(uri, async _ =>
            {
                string token = (await identity.AcquireTokenForClient(scopes).ExecuteAsync()).AccessToken;
                diagnosticSecrets.Add(token);
                return token;
            }, true);
            if (!client.IsReady)
            {
                logger.LogError("{Diagnostic}", "Connection failed. Check configuration, application-user access, and connectivity.");
                logger.LogError("{Diagnostic}", $"ServiceClient.LastError: {Redact(client.LastError ?? "(not supplied)")}");
                if (client.LastException is { } lastException)
                    ReportException(lastException, "ServiceClient.LastException");
                throw new InvalidOperationException("Account numbering failed; see redacted diagnostics.");
            }

            logger.LogInformation("Connected to Dataverse.");
            stage = "retrieving Accounts";
            var query = new QueryExpression("account")
            {
                ColumnSet = new ColumnSet("name"),
                PageInfo = new PagingInfo { Count = 5000, PageNumber = 1 }
            };
            query.AddOrder("accountid", OrderType.Ascending);
            var accounts = new List<Entity>();
            while (true)
            {
                var page = client.RetrieveMultiple(query);
                accounts.AddRange(page.Entities);
                if (!page.MoreRecords) break;
                query.PageInfo.PageNumber++;
                query.PageInfo.PagingCookie = page.PagingCookie;
            }

            var ordered = accounts.OrderBy(a => a.GetAttributeValue<string>("name") ?? "",
                StringComparer.InvariantCultureIgnoreCase).ThenBy(a => a.Id).ToList();
            if (ordered.Count > 0 && (long)start + ordered.Count - 1 > int.MaxValue)
            {
                logger.LogError("{Diagnostic}", "The sequence would exceed the integer range. No Accounts were updated.");
                throw new InvalidOperationException("Account numbering failed; see redacted diagnostics.");
            }

            stage = "updating Accounts";
            logger.LogInformation("{Progress}", $"Retrieved {ordered.Count} Accounts. Assigning company numbers...");
            foreach (var account in ordered)
            {
                string number = ((long)start + updated).ToString(CultureInfo.InvariantCulture);
                var update = new Entity("account", account.Id) { ["cr0c9_firmennummer"] = number };
                client.Update(update);
                updated++;
                logger.LogInformation("{Progress}", $"{account.GetAttributeValue<string>("name") ?? "(unnamed)"} [{account.Id}] -> {number}");
            }
            logger.LogInformation("{Progress}", $"Successfully completed. Updated {updated} Accounts.");
            return;
        }
        catch (ArgumentException error) when (stage == "checking configuration")
        {
            logger.LogError("{Diagnostic}", error.Message);
            throw new InvalidOperationException("Account numbering failed; see redacted diagnostics.");
        }
        catch (Exception error)
        {
            if (stage == "connecting to Dataverse")
                ReportException(error);
            // Keep non-connection failures generic; never print raw SDK exception objects.
            logger.LogError("{Diagnostic}", $"Failed while {stage}. Confirmed updates: {updated}. Check connectivity, credentials, and Account read/write permissions. Earlier updates are not rolled back; an interrupted request may also have reached the server.");
            throw new InvalidOperationException("Account numbering failed; see redacted diagnostics.");
        }

    }
}

