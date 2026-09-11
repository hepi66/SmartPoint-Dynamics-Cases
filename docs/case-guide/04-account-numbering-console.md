# 4. C# Account Numbering Console

## 1. Original Requirement

Ask for a starting number, retrieve Accounts with the .NET SDK, sort alphabetically, and assign sequential company numbers to Firmennummer.

## 2. Case in One Sentence

A C# Dataverse SDK console application numbers Accounts alphabetically from a user-supplied starting number and persists each number as text.

## 3. What We Built

A small .NET 10 console application using Dataverse ServiceClient, MSAL application authentication, paging, deterministic sorting and individual updates.

## 4. Where to Find It

- **Project:** Repository → `src/AccountNumbering/` → [AccountNumbering.csproj](../../src/AccountNumbering/AccountNumbering.csproj).
- **Logic:** same directory → [Program.cs](../../src/AccountNumbering/Program.cs).
- **Configure, build and run:** [component README](../../src/AccountNumbering/README.md), including the verified .NET SDK 10.0.401 setup.
- **Dataverse result:** environment `CRM816895`, app `Account Management`, table `account`, fields `name` and `cr0c9_firmennummer`; exact record-navigation click path: PENDING verification.
- **Identity:** `SmartPoint Dataverse Integration 2`, mapped to a Dataverse application user; exact administrative navigation path: PENDING verification.

## 5. How It Works

Validate a signed integer and the four SMARTPOINT authentication environment variables. Authenticate to the configured tenant with client credentials. Retrieve accessible Accounts in 5,000-record pages using paging cookies. Sort names with invariant-culture case-insensitive comparison, then Account ID. Check integer range and write start + index as an invariant-culture string to the text column `cr0c9_firmennummer`. Print each assignment and the final count.

## 6. Result and Verification

COMPLETED and functionally VERIFIED. Starting at 1000, the live run retrieved one Account and persisted string "1000" for SmartPoint Test Account, reporting `Successfully completed. Updated 1 Accounts.` Restore and Release build passed with no warnings/errors. One Account does not independently demonstrate multi-Account ordering or paging.

## 7. Offline Evidence

Evidence status: PENDING

Recommended screenshot:
The successful console run showing starting number 1000, the SmartPoint Test Account assignment and final updated count. Exclude secret prompts, environment dumps and sensitive diagnostics.

Use dedicated demonstration data and avoid unnecessary personal information.

Intended storage: `docs/case-guide/evidence/`. No image exists or is linked yet. Follow the [evidence instructions](evidence/README.md). Repository text and source code can be shown offline now; they do not replace captured runtime evidence.

## 8. Three Real-World Use Cases

Explanatory examples only; these are not additional implemented SmartPoint functionality.

1. Assign legacy customer references during a migration.
2. Perform a controlled one-off master-data correction.
3. Populate identifiers before an integration handover.

## 9. What I Should Be Able to Explain

- [ ] Explain the registration, credentials, application user and security role.
- [ ] Explain paging and stable name sorting.
- [ ] Explain why a number is written as a string.
- [ ] Explain overwrite behavior and partial completion after a failure.

## 10. Troubleshooting and Important Notes

AADSTS700016 was reproduced with a direct token request: the original registration was not found in the tenant, before Account access. A replacement registration/application user resolved authentication; the text field was not the cause. The detailed README preserves this history. Shell configuration must be supplied again after restart; obtain the working identity values locally and never store secrets in the guide. Stop the timer before demonstrating another numbering result.

[Back to Case Guide](README.md)
