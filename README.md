# SmartPoint Dynamics Case

This repository supports the SmartPoint Dynamics 365 practical case. It contains the project plan and will hold implementation notes, evidence, and source code as work progresses.

- Power Platform environment: `CRM816895`
- Target unmanaged solution: `Achim Beispiel`

The scope covers a room-planning Dataverse model and model-driven app, booking confirmation emails, an Account form notification, a C# Account-numbering console application, a timer-triggered Azure Function, and the Copilot Studio `Account Creator` agent. An Opportunity notification flow is optional.

[PROJECT_PLAN.md](PROJECT_PLAN.md) is the authoritative source for roadmap, progress, completed work, and next steps. The Dataverse and `Room Planning` model-driven app foundation is COMPLETED to the extent required for the SmartPoint case, including representative test data and functional verification of Today's Bookings, navigation, and basic booking creation. Planning Capacity is informational only; leaving it unenforced is an explicitly tested, deliberate scope decision. The `Booking Confirmation` Power Automate flow is COMPLETED and VERIFIED, including actual receipt of the confirmation email sent through Office 365 Outlook. JavaScript Account Form Notification is COMPLETED and VERIFIED. The [C# console application](src/AccountNumbering/README.md) is COMPLETED and functionally VERIFIED against Dataverse with starting number 1000. The [timer function](src/AccountNumberingFunction/README.md) is COMPLETED and functionally VERIFIED through two local Dataverse timer invocations. The [Copilot Studio Account Creator](docs/AccountCreator.md) is CONFIGURED, but NOT RUNTIME VERIFIED: missing Copilot Studio credits blocked preview before tool execution. External clarification on capacity or acceptance of the configured state is pending. Priority 7 remains NOT STARTED.

The [Account notification source](src/javascript/AccountFormNotification.js) is deployed as the `Account Form Notification` web resource and registered on the Account main form OnLoad event. `showCompanyNumberNotification` reads `name` and `cr0c9_firmennummer` through the execution context. The published `Account Management` app was used for successful functional verification with `SmartPoint Test Account` and company number `4711`.

Implementation notes belong in [docs/](docs/README.md), and source code belongs in [src/](src/README.md). This repository is completely separate from `Dynamics365-Learning-Lab`.

Start with the [central SmartPoint Case Guide](docs/case-guide/README.md) for case navigation, recorded results, interview notes and demonstration-readiness tracking.
