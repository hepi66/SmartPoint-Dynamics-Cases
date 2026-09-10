# Account Numbering Console Application

Status: COMPLETED and functionally VERIFIED against Dataverse. [PROJECT_PLAN.md](../../PROJECT_PLAN.md) controls project status.

This small .NET 10 console application uses `Microsoft.PowerPlatform.Dataverse.Client` 1.2.27 for SDK queries and updates. MSAL, supplied transitively by that package, obtains application tokens using the explicitly configured tenant. `System.Security.Cryptography.Xml` 10.0.10 overrides the SDK's vulnerable transitive 8.0.2 dependency. There are no frameworks, configuration files, or persistent token caches.

## Configuration

Set these process environment variables in the PowerShell session used to run the application:

| Variable | Value |
| --- | --- |
| `SMARTPOINT_DATAVERSE_URL` | `https://oebb-stoerungsmanagement.crm3.dynamics.com` |
| `SMARTPOINT_TENANT_ID` | Directory (tenant) ID for the working Entra registration. |
| `SMARTPOINT_CLIENT_ID` | Application (client) ID of `SmartPoint Dataverse Integration 2`. |
| `SMARTPOINT_CLIENT_SECRET` | Supply locally through a masked prompt; never store in a file or command history. |

The working Entra application is `SmartPoint Dataverse Integration 2`, registered as a Dataverse application user in environment `CRM816895`. Authentication uses client credentials. Its assigned role is System Administrator for this practical case. Use the replacement registration's client ID, not the original registration's ID.

```powershell
$env:SMARTPOINT_DATAVERSE_URL = 'https://oebb-stoerungsmanagement.crm3.dynamics.com'
$env:SMARTPOINT_TENANT_ID = Read-Host 'Directory (tenant) ID'
$env:SMARTPOINT_CLIENT_ID = Read-Host 'Application (client) ID of SmartPoint Dataverse Integration 2'
# PowerShell 7: entered text is masked and is not part of command history.
$env:SMARTPOINT_CLIENT_SECRET = Read-Host 'Client secret' -MaskInput
```

Do not print the secret or commit credentials. The application reads environment variables only; it does not load `.env` files. After running, remove the session secret with `Remove-Item Env:SMARTPOINT_CLIENT_SECRET`.

## Build and Run

From `D:\Projekte\SmartPoint-Dynamics-Cases` in PowerShell:

```powershell
& 'C:\Program Files\dotnet\dotnet.exe' restore .\src\AccountNumbering\AccountNumbering.csproj
& 'C:\Program Files\dotnet\dotnet.exe' build .\src\AccountNumbering\AccountNumbering.csproj --configuration Release --no-restore
& 'C:\Program Files\dotnet\dotnet.exe' run --project .\src\AccountNumbering\AccountNumbering.csproj --configuration Release --no-build
```

The absolute executable path works even when `dotnet` is not on PATH. Enter a signed 32-bit integer when prompted. Running with valid credentials updates Accounts immediately; existing company numbers are overwritten.

## Numbering and Error Behavior

1. Validate the start number and all four environment variables before connecting.
2. Retrieve all accessible `account` records (including inactive Accounts), requesting only `name` and the record ID. Use 5,000-record pages, a paging cookie, and ID ordering while retrieving.
3. Sort the full result by Account Name using invariant-culture, case-insensitive comparison. Missing names sort as empty strings; Account ID breaks name ties deterministically.
4. Check the complete sequence fits within the integer range before any writes. Assign `start + index`, convert using invariant culture, and update only the Single Line of Text column `cr0c9_firmennummer` (Firmennummer).
5. Print each confirmed assignment and the final updated count. No Accounts is a successful run with zero updates.

Updates are individual SDK operations, not a transaction. Failure stops the run and reports the stage and confirmed update count; prior writes remain, and a request interrupted in transit may have reached Dataverse. Avoid concurrent Account changes during the demo. Exit code is 0 on success and 1 on input, configuration, connection, or runtime failure. Connection diagnostics include redacted LastError and exception types/messages, including inner exceptions; raw exception objects are not printed.

## Validation

Validated locally with installed .NET SDK 10.0.401: dependency restore and Release build passed with zero warnings and errors after the XML dependency override.

The user-provided successful live test used starting company number `1000`. Execution connected to Dataverse, retrieved 1 Account, assigned `SmartPoint Test Account` the number `1000`, and reported `Successfully completed. Updated 1 Accounts.` The update persisted in the text column `cr0c9_firmennummer` as a string. This verifies the complete required workflow from input through persistence; the one-Account dataset does not independently demonstrate multi-Account ordering or paging.

## Troubleshooting

### Authentication failure: AADSTS700016

1. **Initial symptom:** The console application failed while connecting to Dataverse with `DataverseConnectionException`. The relevant inner Microsoft identity error was `AADSTS700016`: application ID `cfb288b5-955e-46fb-831f-fb79ebc9fa8e` could not be found in tenant `9573d76d-ef51-494d-8586-a29c47e01e1e`.
2. **Diagnosis:** The tenant ID and configured client ID were checked explicitly. A direct OAuth token request reproduced the same `AADSTS700016` error, isolating the failure to Entra ID authentication before Dataverse Account access or update logic was reached. The Dataverse `Firmennummer` field type was not the cause of this connection failure.
3. **Resolution:** A new Entra application registration, `SmartPoint Dataverse Integration 2`, was created in the correct tenant, with a new client secret. The application was added to Dataverse environment `CRM816895` as an application user and assigned the System Administrator security role for this practical case. Local environment variables were updated to use the new client ID and client secret. No secret value is stored in this repository.
4. **Verification:** Authentication succeeded with the replacement registration. With starting company number `1000` supplied, the console application connected to Dataverse, retrieved the Account record, and assigned and persisted company number `"1000"` for `SmartPoint Test Account`. Execution reported `Successfully completed. Updated 1 Accounts.` The `cr0c9_firmennummer` column remains a text field, so storing the company number as a string is intentional.

**Lesson learned:** The Entra application registration defines the application identity; client credentials authenticate that identity; the Dataverse application user maps it into the target environment; and its Dataverse security role grants data-operation permissions. All four pieces must refer to the intended identity/environment for service-to-service authentication to work.
