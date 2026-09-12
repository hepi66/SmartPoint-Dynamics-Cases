# SmartPoint Dynamics Case — Project Plan

This file is the authoritative source for the project roadmap, progress, completed work, and next steps. Update it as work is performed; record completed work only when supported by actual results.

## Project Context

- Project: SmartPoint Dynamics Case
- Power Platform environment: `CRM816895`
- Target unmanaged solution: `Achim Beispiel`
- Local project root: `D:\Projekte\SmartPoint-Dynamics-Cases`
- This repository is completely separate from `Dynamics365-Learning-Lab`.
- Repository content, documentation, file names, and technical terminology are in English. Required platform names and the exact requested German notification text are preserved.

## Current Focus / Next Step

Case Consolidation and Demonstration Readiness while awaiting external clarification for Copilot Studio runtime capacity. If capacity is resolved, completing Copilot Studio runtime verification has immediate priority.

## Roadmap and Implementation Status

| Priority | Implementation item | Status |
| --- | --- | --- |
| 1 | Room-planning Dataverse model and model-driven app | COMPLETED to the extent required for the SmartPoint case |
| 2 | Booking confirmation Power Automate flow | COMPLETED and VERIFIED |
| 3 | JavaScript Account form notification | COMPLETED and VERIFIED |
| 4 | C# Dataverse console application | COMPLETED and functionally VERIFIED |
| 5 | Timer-triggered Azure Function | COMPLETED and functionally VERIFIED |
| 6 | Copilot Studio Account Creator | CONFIGURED, but NOT RUNTIME VERIFIED — missing credits; external clarification pending |
| 7 | Optional Opportunity notification flow | NOT STARTED |

## Requirements

### 1. Room-planning Dataverse Model and Model-driven App

Status: COMPLETED to the extent required for the SmartPoint case. The Dataverse foundation, Room Planning model-driven app, representative test data, Today's Bookings view, app navigation, and basic booking creation workflow are verified. Planning Capacity is informational only, as documented in the deliberate scope decision below.

- Support multiple locations, each containing rooms.
- Rooms have different maximum capacities.
- Employees create daily room bookings; bookings cover whole days only.
- Provide a daily booking overview so employees can see the current occupancy situation for the day.
- Offices are conceptually occupied only up to 50%.
- No custom overbooking validation or custom programming is required.
- Design the data model, tables, fields, forms, and views appropriately with a minimal implementation.

### 2. Booking Confirmation Power Automate Flow

Status: COMPLETED and VERIFIED. The saved flow successfully processed a new Booking and delivered the confirmation email to the user's Microsoft 365 mailbox. Implementation, verification, and troubleshooting are recorded under Completed Work.

- Trigger when a booking is created.
- Send the employee a confirmation email for the booked day.

### 3. JavaScript Account Form Notification

Status: COMPLETED and VERIFIED based on the user-provided successful Account main form runtime test in the published Account Management app.

- Add a form notification to the Account form on load.
- Display the required text, substituting the Account name and company number for the placeholders:

  "Die aktuelle Firmennummer der Firma **Firmenname** lautet **Firmennummer**."

- Source artifact: [src/javascript/AccountFormNotification.js](src/javascript/AccountFormNotification.js).
- Handler: `showCompanyNumberNotification(executionContext)`, using `executionContext.getFormContext()`.
- Field logical names: `name` (Account Name) and `cr0c9_firmennummer` (Firmennummer).
- Uses `formContext.ui.setFormNotification` with level `INFO` and ID `companyNumberNotification`. Missing attributes, null values, and empty or whitespace-only values suppress the message and clear any previous notification with that ID. Numeric zero is treated as a supplied value.
- The `Account Form Notification` JavaScript web resource was created in the `Achim Beispiel` unmanaged solution and registered on the Account main form OnLoad event. The script reads Account Name and Firmennummer and displays the required notification.
- The dedicated `Account Management` model-driven app was created and published to provide a clean runtime test of the Account form.
- Functional verification succeeded using Account Name `SmartPoint Test Account` and Firmennummer `4711`. The displayed notification was: "Die aktuelle Firmennummer der Firma SmartPoint Test Account lautet 4711."
- Current Case 03 runtime verification: `SmartPoint Test Account` shows Firmennummer `1` and the matching notification "Die aktuelle Firmennummer der Firma SmartPoint Test Account lautet 1." Later account-numbering work changed the earlier `4711` value to `1`. Runtime and OnLoad configuration screenshots are linked in [Case 03](docs/case-guide/03-javascript-account-notification.md).
- Missing-value handling was checked locally with mocked inputs; no additional Power Apps runtime test of missing values is claimed.

### 4. C# Dataverse Console Application

Status: COMPLETED and functionally VERIFIED based on successful live Dataverse execution, most recently the user-provided final three-Account verification with starting company number 3000.

- Ask the user for a starting number.
- Read all Accounts from Dataverse using the .NET SDK.
- Sort Accounts alphabetically.
- Assign sequential company numbers beginning with the supplied number.
- Store the value in the Dataverse field `Firmennummer`.

- Source and run instructions: [src/AccountNumbering/](src/AccountNumbering/README.md). Uses the official Dataverse client 1.2.27, paged Account retrieval, deterministic alphabetical sorting, and sequential string values in `cr0c9_firmennummer` (Single Line of Text).
- Configuration uses `SMARTPOINT_DATAVERSE_URL`, `SMARTPOINT_TENANT_ID`, `SMARTPOINT_CLIENT_ID`, and `SMARTPOINT_CLIENT_SECRET` environment variables. No secret is stored in the repository.
- Authentication uses Entra client credentials with `SmartPoint Dataverse Integration 2`, registered as a Dataverse application user in `CRM816895` with the System Administrator role for this practical case.
- Live verification: starting number `1000`; connection succeeded; 1 accessible Account was retrieved; `SmartPoint Test Account` received `1000`; execution reported `Successfully completed. Updated 1 Accounts.` The value persisted in `cr0c9_firmennummer` as a string because the column is text.
- Final Case 04 verification: the user supplied only starting number `3000`; three Accounts were processed alphabetically and persisted as `Alpha Test Account` = `3000`, `SmartPoint Test Account` = `3001`, `Zebra Test Account` = `3002`. The console reported `Successfully completed. Updated 3 Accounts.` Account Management displayed the same mapping. Two final screenshots are linked in [Case 04](docs/case-guide/04-account-numbering-console.md); documentation is reviewed and approved by the user; restart/live demo rehearsal remains PENDING.
- This verifies the prompt-to-persistence workflow, multi-Account alphabetical processing and sequential numbering. Paging is implemented but was not exercised across multiple pages by the three-record test.
- The earlier authentication failure was caused by the original Entra application registration not being resolvable by the tenant. A new registration and corresponding Dataverse application user resolved it. No secret values are documented.
- The initial `AADSTS700016` failure, root-cause isolation, and successful replacement registration are documented in [Account Numbering troubleshooting](src/AccountNumbering/README.md#authentication-failure-aadsts700016).

### 5. Timer-triggered Azure Function

Status: COMPLETED and functionally VERIFIED based on the user-provided successful two-invocation local Dataverse test.

- Use a timer trigger. Implementation: [AccountNumberingFunction](src/AccountNumberingFunction/README.md), function `NumberDataverseAccounts`, configurable `SMARTPOINT_ACCOUNT_NUMBERING_SCHEDULE`, with `RunOnStartup=false`. Verified with Core Tools 4.13.0, Azurite 3.37.0, and timerTrigger using the temporary local test schedule `*/30 * * * * *`. Authentication with the existing Entra registration and Dataverse application user succeeded. Both timer invocations retrieved 1 Account, assigned SmartPoint Test Account -> 1, persisted `cr0c9_firmennummer` as string `"1"`, and reported `Successfully completed. Updated 1 Accounts.` This verifies numbering restarts at 1 each invocation.
- Local startup troubleshooting: missing AzureWebJobsStorage required running Azurite with `UseDevelopmentStorage=true`. The error `Worker runtime cannot be 'None'.` was resolved by setting `FUNCTIONS_WORKER_RUNTIME=dotnet-isolated` and using `func start --dotnet-isolated`. Azurite artifacts are ignored by Git.
- Read all Accounts from Dataverse and sort them alphabetically.
- Assign sequential company numbers beginning with 1.
- Store the values in `Firmennummer` and overwrite existing numbers on every run.
- Local execution is sufficient.
- Use OAuth/application-user authentication.

### 6. Copilot Studio Account Creator

Status: CONFIGURED, but NOT RUNTIME VERIFIED. Implementation/configuration is complete; runtime verification is blocked by external licensing/capacity, not a demonstrated implementation failure.

- Agent name: `Account Creator`.
- Allow users to create a new Account in Dataverse.
- Configure the required Dataverse tools.
- Enable Web Search where useful for enrichment.

- Configured in `CRM816895` with model `GPT-5 Chat`, Web Search enabled, and the attached `Create Dataverse Account` tool using User authentication, fixed environment/table inputs, and AI-filled Account Name. Other fields are not artificially populated.
- Preview request `Create an account called SmartPoint Copilot Test Account.` stopped before tool execution with `EnforcementUsageCredits`. No successful Dataverse Account creation is claimed.
- Administrative checks found 0 current Copilot Credits, no environment capacity allocation, and no pay-as-you-go configuration. Trial activation was deliberately not completed; no billing or organization data was changed. The environment owner/contact has been asked about capacity or acceptance of the configured state; clarification is pending.
- Configuration, instructions, test evidence, and blocker details: [Account Creator](docs/AccountCreator.md).

### 7. Optional Opportunity Notification Flow

Status: NOT STARTED

- Trigger when a new Opportunity is created.
- Send a simple email to the record owner.
- Include the Opportunity title and date.

## Working Principles

- Prioritize demonstrable working results.
- Keep implementation minimal and explainable.
- Avoid unnecessary setup work.
- Keep this repository completely separate from `Dynamics365-Learning-Lab`.
- Do not invent completed work.
- Maintain roadmap, progress, completed work, and next steps in this file.

## Completed Work

- Created the initial repository documentation and minimal `.gitignore` baseline.
- Dataverse room-planning foundation: COMPLETED manually in environment `CRM816895`, unmanaged solution `Achim Beispiel`, based on the user-provided verified implementation state.
- Room Planning Dataverse/model-driven app foundation: COMPLETED to the extent required for the SmartPoint case, based on the user-provided functional verification recorded below.
- Booking Confirmation Power Automate flow: COMPLETED and VERIFIED based on the user-provided end-to-end test and actual email receipt recorded below. Priority 6 is CONFIGURED, but NOT RUNTIME VERIFIED; priority 7 remains NOT STARTED.
- Copilot Studio Account Creator configuration is complete, including the attached Dataverse tool. Runtime verification remains blocked by missing credits; external clarification is pending.
- Timer-triggered Azure Function: COMPLETED and functionally VERIFIED; two local timer invocations persisted company number `"1"` for SmartPoint Test Account, as recorded in work package 5.
- C# Account Numbering console application: COMPLETED and functionally VERIFIED; the live run persisted company number `1000` for `SmartPoint Test Account`, as recorded in work package 4.
- JavaScript Account Form Notification: COMPLETED and VERIFIED in the published `Account Management` test app, as recorded in work package 3 above.

### Completed Dataverse Foundation

| Table | Primary column | Implemented fields and purpose |
| --- | --- | --- |
| Location | Location Name | Represents an office location containing bookable rooms. |
| Room | Room Name | Location: required lookup to Location. Maximum Capacity: required whole number, minimum 1. Planning Capacity: required whole number, minimum 1. |
| Booking | Booking Name | Booking Date: required Date Only field. Room: required lookup to Room. Employee: required lookup to the existing Dataverse User (`systemuser`) table. |

- Implemented relationships: Location 1:N Room, Room 1:N Booking, and User 1:N Booking.
- Planning Capacity represents the intended 50% occupancy planning limit; it is informational and is not enforced by Dataverse.
- User (`systemuser`) is an existing managed Dataverse system table, not a table created by this project. It appears in the solution because the Employee lookup introduces that dependency.
- No custom overbooking validation has been implemented; it is not required for the case.

### Verified Room Planning App and Test Data

- The model-driven app is named `Room Planning` and contains Locations, Rooms, and Bookings.
- Location test data exists for `Graz`.
- Room test data exists for `Graz Meeting Room 01`, with Maximum Capacity = 10 and Planning Capacity = 5.
- Booking records contain Booking Name, Booking Date, Room, and Employee. Employee uses a Dataverse User lookup.
- The `Today's Bookings` view exists and was functionally verified: a booking for the current day is shown, while a booking for the following day is excluded.
- The model-driven app navigation and basic booking creation workflow were functionally tested.

### Known Limitation / Deliberate Scope Decision

Planning Capacity is currently informational only. Dataverse does not prevent bookings beyond this capacity. This behavior was explicitly tested by successfully creating a sixth same-day booking for a room with Planning Capacity = 5. Custom overbooking validation is intentionally not implemented because it is not required by the SmartPoint case scope.

This is a conscious scope decision, not an unnoticed defect. Completion of the Room Planning foundation does not imply enforcement of the planning limit.

### Verified Booking Confirmation Flow

- Flow name: `Booking Confirmation`; environment: `CRM816895`.
- Trigger: Microsoft Dataverse — `When a row is added, modified or deleted`, configured to react to creation of a Booking record.
- The Booking contains Employee and Room lookups. A Dataverse `Get a row by ID` action resolves the Employee/User record; the Employee's Primary Email supplies the recipient.
- A second Dataverse `Get a row by ID` action resolves the Room record; Room Name supplies the room text. Booking Date comes from the Booking record.
- Email action: Office 365 Outlook — `Send an email (V2)`. Subject: `Room booking confirmation`. The body contains confirmation text, Booking Date, and Room Name.
- The flow was successfully saved and tested end-to-end using a new Booking named `Flow Test`, dated `2026-09-09`, for room `Graz Meeting Room 01` and employee `Achim Hepberger`.
- All four flow stages completed successfully, and the confirmation email was actually received in the user's Microsoft 365 mailbox with the correct booking date and room. This constitutes functional verification of the SmartPoint booking-confirmation requirement.

### Booking Confirmation Troubleshooting History

1. During initial setup, the Dataverse trigger could not load Change Type values. The designer reported `Unexpected error occurred when calling the XRM api` and `The remote name could not be resolved: 'org96048834.crm3.dynamics.com'`.
2. Recreating the flow through another Power Apps/Flows entry point did not resolve this issue. Multiple newly created Dataverse connections showed `Connected`, so it was not simply an unauthenticated Dataverse connection.
3. After the environment/connectivity/DNS-related XRM API issue was resolved, Dataverse metadata and lookup operations worked normally and the flow could be configured. No more specific root cause or resolution mechanism was verified.
4. A separate issue occurred during the first execution: the initially selected generic Mail action, `Send an email notification (V3)`, failed with Microsoft's message that the Mail connector is currently restricted for new tenants and suggested alternatives such as Office 365 Outlook.
5. The Mail action was replaced with Office 365 Outlook — `Send an email (V2)`, and the existing dynamic values were restored: Employee Primary Email, Booking Date, and Room Name.
6. After saving the updated flow, a new Booking triggered it successfully; all four stages completed and the received email contained the correct booking date and room. Office 365 Outlook is the final working mail connector.

## Case Consolidation and Demonstration Readiness

Status: IN PROGRESS. The [central Case Guide](docs/case-guide/README.md) and all seven case pages are CREATED. Existing implementation statuses and verified history remain unchanged; guide creation is not runtime verification.

This phase consolidates the case into one navigable guide, maps original requirements to implementations, documents where to find them, prepares offline evidence and a safe repeatable demo sequence, supplies concise interview notes and three illustrative real-world use cases per component, and checks that the completed work can be found and demonstrated after restart without chat history.

| Case | Implementation | Case Guide | Offline Evidence | Demo Ready |
| --- | --- | --- | --- | --- |
| 1. Room Planning | COMPLETED to case scope | CREATED, REVIEWED and APPROVED by the user | AVAILABLE — two runtime screenshots | PENDING |
| 2. Booking Confirmation | COMPLETED and VERIFIED | CREATED, REVIEWED and APPROVED by the user | AVAILABLE — three screenshots | PENDING |
| 3. JavaScript Account Notification | COMPLETED and VERIFIED | CREATED, REVIEWED and APPROVED by the user | AVAILABLE — two screenshots | PENDING |
| 4. C# Account Numbering | COMPLETED and functionally VERIFIED | CREATED, REVIEWED and APPROVED by the user | AVAILABLE — two screenshots | PENDING |
| 5. Azure Function Account Numbering | COMPLETED and functionally VERIFIED | CREATED | PENDING | PENDING |
| 6. Copilot Studio Account Creator | CONFIGURED, but NOT RUNTIME VERIFIED | CREATED | PENDING | BLOCKED / PENDING EXTERNAL CLARIFICATION |
| 7. Opportunity Notification | OPTIONAL — NOT IMPLEMENTED | CREATED | NOT AVAILABLE | NOT AVAILABLE |

Two supplied Room Planning runtime screenshots are linked in [Case 01](docs/case-guide/01-room-planning.md), showing the room capacities and Today's Bookings. Case 01 demo readiness remains pending; screenshots do not establish a restart rehearsal. Case 7's page reserves a future evidence slot but has no available implementation result. The final restart/demo sequence is a clearly marked placeholder, not a completed rehearsal.

Demo Ready can become complete only after the implementation status is understood, navigation/restart instructions are sufficient, primary offline evidence exists, and the intended demonstration path is known. Confirm these through a restart rehearsal rather than inferring readiness from documentation.

Preserve detailed component documentation as the technical reference. Capture missing verified navigation details and the actual Azurite startup command, collect sanitized screenshots, and account for the console, timer and notification sharing the same company-number field. Copilot runtime verification has immediate priority if external capacity is resolved. The optional Opportunity flow remains planned and is not implemented in this phase.

## Next Steps

1. Consolidate missing navigation/restart details and collect one sanitized primary screenshot per implemented/configured case. Link real evidence in the Case Guide and update this phase's table conservatively.
2. Prepare and rehearse the final restart/demo sequence without relying on chat history; preserve pending readiness until its criteria are met.
3. Await external clarification for Copilot Studio capacity. If resolved, prioritize its runtime test immediately and verify the created Account before recording success.
4. Optional Opportunity notification flow remains planned and NOT STARTED; no implementation is included in this phase.

Update this plan as implementation and verification actually occur, following the priority order above.
