# SmartPoint Case Guide

This is the central navigation and explanation guide for the SmartPoint Dynamics 365 practical case. Use it to map each original requirement to its implementation, locate the result, review recorded verification and prepare a verbal explanation. [PROJECT_PLAN.md](../../PROJECT_PLAN.md) is authoritative for roadmap and status; detailed component READMEs remain the deeper technical references.

## Case Overview

The case combines Dataverse data modeling, model-driven apps, event-driven email automation, client-side JavaScript, SDK-based C# integration, scheduled execution and a conversational Dataverse agent. The seventh component was evaluated and intentionally not implemented because the standard Opportunity table is unavailable in the provided environment.

| Case | Current implementation status |
| --- | --- |
| [1. Room Planning](01-room-planning.md) | COMPLETED to case scope |
| [2. Booking Confirmation](02-booking-confirmation.md) | COMPLETED and VERIFIED |
| [3. JavaScript Account Form Notification](03-javascript-account-notification.md) | COMPLETED and VERIFIED |
| [4. C# Account Numbering Console](04-account-numbering-console.md) | COMPLETED and VERIFIED |
| [5. Timer-triggered Account Numbering Function](05-account-numbering-function.md) | COMPLETED and VERIFIED |
| [6. Copilot Studio Account Creator](06-account-creator.md) | COMPLETED and end-to-end VERIFIED |
| [7. Optional Opportunity Notification](07-opportunity-notification.md) | OPTIONAL — EVALUATED; INTENTIONALLY NOT IMPLEMENTED |

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

Cases 01–06 use a common nine-section structure and exactly three explicitly illustrative business examples. Optional Case 07 uses five concise sections covering its objective, intended design, environment verification, decision and status. The new “Case in One Sentence” section is a learning/navigation aid that connects each requirement with its technology and outcome, not a new requirement. Existing repository evidence has been summarized; missing app URLs, detailed form/view configuration and the verified Azurite launch command remain explicit gaps. Case 06 now includes successful-test and persisted-result screenshots. See [evidence workflow](evidence/README.md).

## Final Restart and Demo Sequence — PENDING

The final repeatable sequence will be captured and checked after restart. It is not yet verified. It must cover locating each cloud component, re-establishing shell environment variables without chat history, starting Azurite and the Functions host, preparing current-day booking data, and stopping local processes after the demo.

The Account notification, console and timer use the same company-number field. Their historical test values 4711, 1000 and 1 belong to different runs. Stop the timer before showing another numbering result and define the intended values and order in the final sequence. Do not treat the historical booking date as today's date.

Collect and link one primary screenshot per case by default. Use dedicated demonstration data and avoid unnecessary personal information. Complete a restart rehearsal before marking a case Demo Ready. Present the successful Case 06 test and persisted Account; do not imply that the agent was published. Present the optional flow as evaluated and intentionally not implemented due to the verified environment limitation.

## Combined PDF from VS Code

The seven individual case Markdown files remain authoritative. Run the small merge script before each export; never edit or maintain the generated `00-complete-case-guide.md` manually. It contains only Cases 01–07 in order. Linked repository references are not imported. Personal preparation files are excluded from the merge and the formal PDF.

### Setup (once)

Install **Markdown PDF** by **yzane** (extension ID `yzane.markdown-pdf`) in VS Code Extensions. No include extension, Python, Node package installation or documentation framework is needed. The script runs with Windows PowerShell or PowerShell in the integrated terminal.

The extension uses Chromium for export; complete the first export before relying on an offline presentation workflow. Its current release can use an installed Chrome/Edge browser or download Chromium. See the [extension documentation](https://github.com/yzane/vscode-markdown-pdf) for setup and troubleshooting.

### Generate, preview and export

1. Open this repository folder in VS Code and save any edits to the individual cases.
2. In the integrated PowerShell terminal at the repository root, run:

   ```powershell
   powershell -NoProfile -ExecutionPolicy Bypass -File .\docs\case-guide\Build-CompleteCaseGuide.ps1
   ```

   The execution-policy option applies only to this process. The script reads exactly the seven named sources, checks their evidence paths and writes the disposable combined Markdown beside them. It does not copy images or embed Base64 data.

3. Open `docs/case-guide/00-complete-case-guide.md`. Press **Ctrl+Shift+V** for VS Code's built-in Markdown Preview. Check Cases 01–07 and all 13 evidence images. Their existing `evidence/...` paths and approximately 80% widths are preserved.
4. In VS Code Settings, filter by `@ext:yzane.markdown-pdf`. Keep **Include Default Styles** enabled, set **Format** to `A4`, disable **Display Header Footer**, and leave **Output Directory** empty so the PDF is written beside the Markdown. These are local settings; do not commit them.
5. Return to the combined Markdown editor. Press **Ctrl+Shift+P** and select **Markdown PDF: Export (pdf)** (shown as `markdown-pdf: Export (pdf)` in some versions).
6. Open `docs/case-guide/00-complete-case-guide.pdf` and check every screenshot, code block and page break before sharing. After source edits, repeat the merge and export; exporting an old combined file does not refresh it automatically.

Both generated files are ignored by Git. Keep the PDF at that path as a local output, not repository source material. Do not export the entire `docs/` directory or include the preparation documents. The script fails if an expected evidence image is missing or excluded preparation content is detected.

### Rendering and limitations

This workflow deliberately produces ordinary Markdown for built-in Preview and export. It does not use `${include ...}$` or depend on an extension-specific include renderer. Markdown PDF documents its own include syntax, but that is unnecessary here and does not provide built-in VS Code Preview expansion.

The PDF uses the extension's default Markdown styles rather than capturing the current Preview. Fonts, highlighting, theme colors, wrapping and pagination can differ. Existing relative repository links remain useful in the local Markdown but are not portable attachments in a shared PDF. Images are included in the PDF, while their original PNG files remain the only image sources in the repository.

Merge and path checks can run without the extension. Successful PDF rendering must still be checked in VS Code; these instructions do not claim that an export has already been visually verified.
