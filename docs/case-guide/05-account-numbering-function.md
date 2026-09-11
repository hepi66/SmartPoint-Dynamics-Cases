# 5. Timer-triggered Account Numbering Function

## 1. Original Requirement

Use a timer-triggered Azure Function with application authentication to retrieve and alphabetically sort Accounts, assign numbers starting at 1, and overwrite Firmennummer on every run. Local execution is sufficient.

## 2. Case in One Sentence

A locally run timer-triggered Azure Function overwrites Account company numbers in alphabetical order, restarting at 1 on every invocation.

## 3. What We Built

An Azure Functions v4 .NET 10 isolated worker project, adapting the console logic without a shared library or interactive input.

## 4. Where to Find It

- **Project:** Repository → `src/AccountNumberingFunction/` → [AccountNumberingFunction.csproj](../../src/AccountNumberingFunction/AccountNumberingFunction.csproj).
- **Timer logic:** same directory → [AccountNumberingTimer.cs](../../src/AccountNumberingFunction/AccountNumberingTimer.cs), function `NumberDataverseAccounts`.
- **Startup:** same directory → [Program.cs](../../src/AccountNumberingFunction/Program.cs).
- **Local execution:** [Function README](../../src/AccountNumberingFunction/README.md) contains exact commands and the Core Tools executable path; verified tools are Core Tools 4.13.0 and Azurite 3.37.0. Exact installed Azurite launch command: PENDING verification.
- **Dataverse target:** `CRM816895` → table `account` → text field `cr0c9_firmennummer`; this is an object reference, not a verified UI click path.

## 5. How It Works

A configured timer calls the function with RunOnStartup=false. Each invocation validates environment variables, authenticates, retrieves all accessible pages, sorts by name then ID, resets its counter to 1 and writes strings using individual SDK updates. Functions logging records start, connection, count, assignments and redacted failures. The schedule comes from `SMARTPOINT_ACCOUNT_NUMBERING_SCHEDULE`.

## 6. Result and Verification

COMPLETED and functionally VERIFIED. With local test schedule `*/30 * * * * *`, two timer invocations each retrieved one Account, persisted SmartPoint Test Account -> string "1", and reported `Successfully completed. Updated 1 Accounts.` This demonstrates restarting at 1 every invocation; multi-Account order/paging was not separately demonstrated.

## 7. Offline Evidence

Evidence status: PENDING

Recommended screenshot:
The local Functions log showing two distinct successful scheduled invocations, each assigning SmartPoint Test Account -> 1 and reporting one update. Exclude sensitive diagnostics.

Use dedicated demonstration data and avoid unnecessary personal information.

Intended storage: `docs/case-guide/evidence/`. No image exists or is linked yet. Follow the [evidence instructions](evidence/README.md). Repository text and source code can be shown offline now; they do not replace captured runtime evidence.

## 8. Three Real-World Use Cases

Explanatory examples only; these are not additional implemented SmartPoint functionality.

1. Run a scheduled customer-data maintenance job.
2. Refresh derived account classifications overnight.
3. Reconcile reference values periodically with an external system.

## 9. What I Should Be Able to Explain

- [ ] Explain isolated worker startup versus the timer invocation.
- [ ] Explain schedule configuration and RunOnStartup=false.
- [ ] Explain why every invocation resets its counter.
- [ ] Explain local storage, logging and partial-update behavior.

## 10. Troubleshooting and Important Notes

Missing AzureWebJobsStorage required running Azurite with `UseDevelopmentStorage=true`. “Worker runtime cannot be None” was resolved with `FUNCTIONS_WORKER_RUNTIME=dotnet-isolated` and `func start --dotnet-isolated`. Azurite artifacts are ignored. Exact installed Azurite launch command remains to be captured for the final restart guide. The 30-second schedule is local test configuration only. Stop the host after demonstration so it does not overwrite other test results.

[Back to Case Guide](README.md)
