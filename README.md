# SmartPoint Dynamics Case

This repository supports the SmartPoint Dynamics 365 practical case. It contains the project plan and will hold implementation notes, evidence, and source code as work progresses.

- Power Platform environment: `CRM816895`
- Target unmanaged solution: `Achim Beispiel`

The scope covers a room-planning Dataverse model and model-driven app, booking confirmation emails, an Account form notification, a C# Account-numbering console application, a timer-triggered Azure Function, and the Copilot Studio `Account Creator` agent. An Opportunity notification flow is optional.

[PROJECT_PLAN.md](PROJECT_PLAN.md) is the authoritative source for roadmap, progress, completed work, and next steps. The Dataverse room-planning foundation (Location, Room, Booking, and their relationships) is completed. Room planning remains IN PROGRESS, with forms, booking/day views, the model-driven app, test data, and validation next. All later roadmap items remain NOT STARTED.

Implementation notes belong in [docs/](docs/README.md), and future source code belongs in [src/](src/README.md). This repository is completely separate from `Dynamics365-Learning-Lab`.
