# SmartPoint Case Guide

This is the central navigation and explanation guide for the SmartPoint Dynamics 365 practical case. Use it to map each original requirement to its implementation, locate the result, review recorded verification and prepare a verbal explanation. [PROJECT_PLAN.md](../../PROJECT_PLAN.md) is authoritative for roadmap and status; detailed component READMEs remain the deeper technical references.

## Case Overview

The case combines Dataverse data modeling, model-driven apps, event-driven email automation, client-side JavaScript, SDK-based C# integration, scheduled execution and a conversational Dataverse agent. The seventh component is optional and unimplemented.

| Case | Current implementation status |
| --- | --- |
| [1. Room Planning](01-room-planning.md) | COMPLETED to case scope |
| [2. Booking Confirmation](02-booking-confirmation.md) | COMPLETED and VERIFIED |
| [3. JavaScript Account Form Notification](03-javascript-account-notification.md) | COMPLETED and VERIFIED |
| [4. C# Account Numbering Console](04-account-numbering-console.md) | COMPLETED and VERIFIED |
| [5. Timer-triggered Account Numbering Function](05-account-numbering-function.md) | COMPLETED and VERIFIED |
| [6. Copilot Studio Account Creator](06-account-creator.md) | COMPLETED and end-to-end VERIFIED |
| [7. Optional Opportunity Notification](07-opportunity-notification.md) | OPTIONAL — NOT IMPLEMENTED |

## Environment and Tooling

- Power Platform environment: CRM816895; target unmanaged solution: Achim Beispiel.
- Microsoft Dataverse and Power Apps: Room Planning and Account Management model-driven apps.
- Power Automate: Booking Confirmation. Copilot Studio: Account Creator Standard Test, model shown during the successful test as GPT-5.5 Chat.
- C# projects: .NET 10; verified SDK 10.0.401; Dataverse client 1.2.27; XML dependency override 10.0.10.
- Azure Functions v4 isolated worker: Core Tools 4.13.0 and Azurite 3.37.0 used in verification.
- Git/GitHub repository: SmartPoint-Dynamics-Cases; local root: D:\Projekte\SmartPoint-Dynamics-Cases. Keep separate from Dynamics365-Learning-Lab.
- Exact console and Function commands are linked from cases 4 and 5. No secrets or sensitive authentication values belong in this guide.

## Demonstration Readiness

All seven guide pages are CREATED. Evidence status is tracked per case in PROJECT_PLAN.md; Case 06 now has two final screenshots; case 7 evidence is NOT AVAILABLE because nothing is implemented. Demo readiness for cases 1–5 is PENDING, case 6 is PENDING, and case 7 is NOT AVAILABLE. Documentation creation is not a new functional verification.

Case pages use a common ten-section structure and exactly three explicitly illustrative business examples. The new “Case in One Sentence” section is a learning/navigation aid that connects each requirement with its technology and outcome, not a new requirement. Existing repository evidence has been summarized; missing app URLs, detailed form/view configuration and the verified Azurite launch command remain explicit gaps. Case 06 now includes successful-test and persisted-result screenshots. See [evidence workflow](evidence/README.md).

## Final Restart and Demo Sequence — PENDING

The final repeatable sequence will be captured and checked after restart. It is not yet verified. It must cover locating each cloud component, re-establishing shell environment variables without chat history, starting Azurite and the Functions host, preparing current-day booking data, and stopping local processes after the demo.

The Account notification, console and timer use the same company-number field. Their historical test values 4711, 1000 and 1 belong to different runs. Stop the timer before showing another numbering result and define the intended values and order in the final sequence. Do not treat the historical booking date as today's date.

Collect and link one primary screenshot per case by default. Use dedicated demonstration data and avoid unnecessary personal information. Complete a restart rehearsal before marking a case Demo Ready. Present the successful Case 06 test and persisted Account; do not imply that the agent was published. The optional flow remains planned and must not be presented as built.
