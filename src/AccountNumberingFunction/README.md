# Account Numbering Timer Function

Status: COMPLETED and functionally VERIFIED through two successful local timer invocations against Dataverse. No Azure deployment is required for the SmartPoint case.

## Behavior and Structure

`NumberDataverseAccounts` is an Azure Functions v4 .NET 10 isolated timer function. It adapts the verified console application's environment validation, MSAL application authentication, ServiceClient, and paged SDK retrieval. All accessible Accounts, including inactive Accounts, are sorted by name with invariant-culture case-insensitive comparison and Account ID as a stable tie-breaker. Missing names sort as empty strings.

Each invocation restarts numbering at **1** and overwrites `cr0c9_firmennummer` using invariant-culture strings because the column is text. Updates are individual SDK operations; a failed invocation may leave partial updates. Logs show invocation start, connection status, retrieved count, each assignment, final count, and redacted failures. Raw SDK exceptions are not passed to the host. There is no interactive input, HTTP trigger, or shared library.

- `AccountNumberingFunction.csproj`: `Azure.Functions.Sdk` 1.0.0, Worker 2.52.0, Timer extension 4.3.1, Dataverse client 1.2.27, and XML security dependency override 10.0.10.
- `Program.cs`: isolated worker startup.
- `AccountNumberingTimer.cs`: timer and Account numbering logic.
- `host.json`: minimal Functions v2 host configuration schema for runtime v4.
- `README.md`: local setup and verification.

## Local Configuration

Supply these environment variables in the shell that launches Core Tools:

| Variable | Purpose |
| --- | --- |
| `SMARTPOINT_DATAVERSE_URL` | HTTPS Dataverse organization root URL. |
| `SMARTPOINT_TENANT_ID` | Tenant GUID for the working Entra registration. |
| `SMARTPOINT_CLIENT_ID` | Client GUID for the working application, SmartPoint Dataverse Integration 2. |
| `SMARTPOINT_CLIENT_SECRET` | Client secret supplied locally through a masked prompt. |
| `SMARTPOINT_ACCOUNT_NUMBERING_SCHEDULE` | Six-field NCRONTAB timer schedule. |
| `FUNCTIONS_WORKER_RUNTIME` | `dotnet-isolated`. |
| `AzureWebJobsStorage` | For local Azurite: `UseDevelopmentStorage=true`, with the emulator running. |

Use the same verified authentication values as the console application. Never commit secrets or print them. In PowerShell 7, a masked prompt avoids putting the secret in command history:

```powershell
$env:SMARTPOINT_CLIENT_SECRET = Read-Host 'Client secret' -MaskInput
```

No `local.settings.json` is needed when configuration comes from the launching shell. If one is later required, the existing root `.gitignore` excludes it; do not put `SMARTPOINT_CLIENT_SECRET` in it. Build output is also ignored.

## Build and Run Locally

From the repository root:

```powershell
& 'C:\Program Files\dotnet\dotnet.exe' restore .\src\AccountNumberingFunction\AccountNumberingFunction.csproj
& 'C:\Program Files\dotnet\dotnet.exe' build .\src\AccountNumberingFunction\AccountNumberingFunction.csproj --configuration Release

# Set the four authentication variables in this shell before starting.
# Start a locally available Azurite emulator in a separate terminal first.
$env:PATH = 'C:\Program Files\dotnet;' + $env:PATH
$env:FUNCTIONS_WORKER_RUNTIME = 'dotnet-isolated'
$env:AzureWebJobsStorage = 'UseDevelopmentStorage=true'
# Temporary local verification schedule only: every 30 seconds.
$env:SMARTPOINT_ACCOUNT_NUMBERING_SCHEDULE = '*/30 * * * * *'
& 'C:\Program Files\Microsoft\Azure Functions Core Tools\func.exe' start --dotnet-isolated --script-root .\src\AccountNumberingFunction\bin\Release\net10.0 --no-build
```

`RunOnStartup` is false. The schedule is configuration, not hardcoded behavior. Wait for a scheduled invocation; starting the host does not itself verify a timer run. Stop the host with Ctrl+C after the test and remove the temporary schedule and secret from the shell:

```powershell
Remove-Item Env:SMARTPOINT_ACCOUNT_NUMBERING_SCHEDULE
Remove-Item Env:SMARTPOINT_CLIENT_SECRET
```

## Functional Verification

The user-provided live test used Core Tools 4.13.0 and Azurite 3.37.0. Function `NumberDataverseAccounts` ran through `timerTrigger` with the temporary local schedule `*/30 * * * * *` and `RunOnStartup=false`. Host storage used `AzureWebJobsStorage=UseDevelopmentStorage=true`.

Authentication with the existing Entra application registration and Dataverse application user succeeded. The function connected to Dataverse, retrieved 1 Account, assigned `SmartPoint Test Account -> 1`, persisted `cr0c9_firmennummer` as string "1", and reported `Successfully completed. Updated 1 Accounts.` A second timer invocation completed successfully and again assigned 1, verifying that numbering restarts on every invocation. The single-Account dataset does not independently demonstrate multi-Account ordering or paging.

## Troubleshooting

- **Host storage:** Earlier startup failed its storage check because `AzureWebJobsStorage` was missing. Running local Azurite and setting `AzureWebJobsStorage=UseDevelopmentStorage=true` supplied the required local storage. Azurite runtime artifacts are excluded by the repository's .gitignore.
- **Worker runtime:** Startup reported `Worker runtime cannot be 'None'.` Set `FUNCTIONS_WORKER_RUNTIME=dotnet-isolated` in the launching shell and start with `func start --dotnet-isolated` (the full-path command above also includes the compiled output location). The host then started successfully.
