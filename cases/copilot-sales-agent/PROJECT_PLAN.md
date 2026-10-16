# Copilot Studio Sales Agent Project Plan

## Project Context

- **Project:** Copilot Studio Sales Agent
- **Environment:** `CRM738378`
- **Agent:** `SmartPoint Sales Assistant`
- **Requirements source:** [Hepberger Copilot Studio Example - SmartPoint.pdf](Hepberger_Copilot_Studio_Beispiel-smartpoint.pdf)
- **Case directory:** `cases/copilot-sales-agent/`
- **Progress log:** [PROJECT_PROGRESS.md](PROJECT_PROGRESS.md)
- **Authoritative case status:** this case-local `PROJECT_PLAN.md`

The separate email context document contains access information. Do not commit it or copy sensitive information from it into repository documents.

## Repository Boundary and Case Authority

All Copilot Studio Sales Agent planning, progress, handoff, flow notes, technical documentation, and case evidence belong under `cases/copilot-sales-agent/`. This case-local `PROJECT_PLAN.md` is authoritative for environment `CRM738378`; the repository-root `PROJECT_PLAN.md` documents the separate original SmartPoint practical case in `CRM816895` and is not a source of status or continuation instructions for this case.

## Status Terms

- **VERIFIED:** observed in the target environment.
- **PARTIALLY VERIFIED:** some behavior works, but full acceptance remains open.
- **INCOMPLETE:** configuration or implementation exists, but required parts are missing.
- **BLOCKED:** a reproduced issue prevents the current implementation path; this does not establish that alternatives are blocked.
- **UNVERIFIED / PENDING:** there is not enough runtime evidence.
- A confirmation message alone does not prove a Dataverse write occurred.

## MVP Goal and Acceptance Criteria

The MVP is a working Copilot Studio Sales Agent in `CRM738378`, usable from Microsoft Teams, that:

1. Reads customer and opportunity data from Dataverse.
2. Answers a product question using SharePoint knowledge and provides a source link.
3. Creates an Opportunity through a controlled flow after explicit confirmation.
4. Returns a usable link to the created record.
5. Can retrieve the newly created Opportunity.

**Architecture First:** establish the minimum working end-to-end architecture, then implement the remaining case requirements, then improve robustness, security, and answer quality. This order does not change or expand the agreed MVP scope.

## Approved Story Workflow and Current Status (updated 2026-10-16)

Work on exactly one user story at a time: **Plan -> Implement -> Test -> Fix -> Accept -> Presentation -> Next story.** After each accepted story, prepare a presentation covering requirements, implementation, architecture, and test evidence before implementing or testing the next story. Do not repeat full-project audits unless requirements or architecture materially change.

| Story | Status | Verified work | Open acceptance / limits | Tests |
| --- | --- | --- | --- | --- |
| US-01 / S01 — Customer Overview | FUNCTIONALLY COMPLETE / ACCEPTED | T1 returned four open opportunities for Huber Logistik, with sales stage, estimated revenue, expected close date, an open quote, and recent customer activity. T2 asked the user to distinguish `Huber Logistik` from `Huber Maschinenbau GmbH`. | The stage is returned as a numeric Choice value. Currency presentation is not reliably verified and must not be assumed to be EUR. End-to-end response time below 10 seconds is unverified. These are documented non-blocking limitations. | T1 PASS; T2 PASS |
| US-02 / S02 — Product Knowledge | FUNCTIONALLY COMPLETE / ACCEPTED (2026-10-16), based on Copilot Studio test evidence | T3 compared AX-200 and AX-300 with SharePoint citations. T4's corrected answer stated that Italian VAT information was unavailable in the company knowledge base. | T4 initially used general model knowledge despite web search being disabled. After correction, the agent stated the information gap. No permanent fix or broader behavior is claimed. | T3 PASS; T4 PASS after correction |
| US-03 / S03 — Create Opportunity | FUNCTIONALLY COMPLETE for the tested Copilot Studio interaction; report available | On 2026-10-16 the four-input flow path clarified a missing close date, showed a complete summary, waited for explicit confirmation, created the row, and returned a clickable link. The complete-input test link opened the row in `SmartPoint Sales Demo`. | The recorded examples used AX-300 / `35000`, not the exact original T5 example of three AX-200 / approximately EUR 450,000. Currency persistence/display and a separate negative test of flow-side missing-input validation remain unverified. | T5 behavior PASS for the tested variants; exact original sample values not replayed |
| US-04 — Discount Approval (bonus) | PREPARED / BONUS; scheduled after mandatory US-05 / T7; workflow not implemented | `Discount Request` model, relationship, form, view, app navigation, and a Pending test record are verified. | Approvals/Teams availability, licensing, permissions, approver identity, Opportunity-level outcome persistence, requester notification, and T6 are unverified. | T6 BONUS / PENDING |
| US-05 — Regional Permissions | NEXT MANDATORY STORY / UNVERIFIED | No regional access enforcement is claimed. | User-context access and refusal for a restricted Region North request remain to be tested. | T7 PENDING |

### Teams Channel Blocker

`SmartPoint Sales Assistant` is published and installed in Microsoft Teams. Copilot Studio test chat works, but Teams requests fail with a usage-limit error. `CRM738378` has 1,000 allocated legacy sessions; no Copilot Credits or billing plan were found. Licensing/capacity is a suspected, not confirmed, cause. An inquiry was emailed to Dominic Bley on 2026-10-15; his automatic reply says he will be available again from 2026-10-18. Evaluate the guidance and retest in Teams when a response is available. Do not enable paid billing without explicit approval. This channel blocker is distinct from the working Copilot Studio test-chat results.

The existing agent and verified Dataverse connection are completed foundations, not new implementation tasks. Agent-driven Account creation is a limited verified path; it does not establish the separate confirmation-controlled `Create Customer` topic or US-03.

## Mandatory Requirements and Status

| ID | Requirement | Status | Evidence / acceptance still needed |
| --- | --- | --- | --- |
| M-01 | Dataverse customer, Opportunity, Quote, and Activity context | S01 / T1 functionally verified | Four open Huber Logistik opportunities, quote, and recent activity appeared in the answer. Activity schema/mappings are not documented. Stage is numeric; currency and sub-10-second latency are unverified. |
| M-02 | At least one custom Dataverse table with relationships | MVP foundation VERIFIED | `Discount Request` (`alp_rabattanfrage`) has fields `Name`, `Discount Percentage`, `Reason`, and `Approval Status` (`Pending`, `Approved`, `Rejected`), an N:1 relationship to Sales Opportunity, a main form, an `Active Discount Requests` view, and a verified app record. |
| M-03 | Customer lookup, including ambiguous names | T2 VERIFIED for ambiguous name | `Huber` prompted clarification between `Huber Logistik` and `Huber Maschinenbau GmbH`. Lookup by customer number was not separately verified. |
| M-04 | SharePoint knowledge from 3–5 documents with citations | COMPLETED / VERIFIED (2026-10-15; T3 tested 2026-10-16) | The final source `Alpentec Sales Knowledge` retrieves all four Alpentec DOCX documents and returns source references. T3 comparison citations were observed. |
| M-05 | Confirmed Opportunity creation and record link | FUNCTIONALLY VERIFIED in Copilot Studio test chat (2026-10-16) | Four required inputs, missing-date clarification, complete pre-write summary, explicit confirmation, flow write, and clickable link are documented; the complete-input test link opened the new row. Exact original T5 sample values (three AX-200 / approximately EUR 450,000), EUR persistence/display, and the separate server-side missing-input negative test were not verified. See [Opportunity flow notes](SALES_OPPORTUNITY_FLOW.md). |
| M-06 | Entra ID authentication and effective Dataverse permissions | UNVERIFIED | Verify allowed and denied operations under intended identities/roles; tool availability alone does not prove enforcement. |
| M-07 | User-context access and regional restriction | UNVERIFIED | Verify access in the signed-in user context and denial for the restricted Region North customer. |
| M-08 | Microsoft Teams deployment and use | BLOCKED — published/installed, usage-limit error | Test chat works; Teams request fails. Suspected licensing/capacity cause is not confirmed. Await SmartPoint guidance; do not enable paid billing without approval. |
| M-09 | Demo scenarios T1–T7, mandatory first | T1–T4 PASS; T5 behavior functionally verified with alternate sample data; mandatory T7 PENDING before bonus T6 | Verify US-05/T7 before pursuing bonus US-04/T6. Preserve the original T5 sample for presentation rehearsal and disclose that currency is unverified. |
| M-10 | Architecture, security, and demo documentation | PENDING | Bring data model, orchestration choice, identity/permissions, known limits, and repeatable demo path into a cohesive solution explanation. Do not claim unverified behavior. |

## Dataverse Foundation and Verified Data

The two solutions `SmartPoint Copilot Sales Case` and `SmartPoint Sales Demo` are separate solutions in the same environment. Sales Opportunity and Sales Quote are custom Dataverse tables, not standard Dynamics 365 Sales entities. The published `SmartPoint Sales Demo` app displays Account, Sales Opportunity, and Sales Quote records. The `Discount Request` table belongs to the separate `SmartPoint Copilot Sales Case` solution and was added to the demo app; the solution boundary remains unchanged.

Verified Account and Opportunity read tools include `Accounts List Rows` and the read-only `Get Sales Opportunities`. They retrieved `Huber Logistik` and linked Opportunity `Logistics Fleet Modernization` (Account ID `177a9152-afc7-f111-a05a-000d3a0a2928`; Opportunity ID `5aa34222-b0c7-f111-a05a-000d3a0a2928`). A business-oriented combined request returned correct customer and Opportunity data and their relationship. The final execution trace was not separately checked to prove that the agent automatically selected both tools.

Other documented demo data includes `Logistics Fleet Modernization` (Estimated Revenue `85000 USD`, Expected Close Date `2026-12-31`, Sales Stage `New`, Probability `40`), quotes `QUO-2026-001` and `QUO-2026-002`, and the verified `Discount Request` record `Alpentec CNC – 10% Discount Request`. These app/data checks do not by themselves prove agent retrieval.

The agent tools `Dataverse Add Row` and `Dataverse List Rows` are available, and the Dataverse connection is verified. The `Create Customer` topic asks for `customerName` and displays a working confirmation message, but its explicit Add Row action fails with `Input binding 'item.name' was not found.` Do not retry this path. Neither the confirmation nor the separate agent-driven Account creation test proves that this topic writes a record.

## MVP Implementation Sequence

Effort values are planning estimates; completed foundation work is not to be repeated.

| Step / status | Objective and deliverable | Acceptance | Learning value / effort |
| --- | --- | --- | --- |
| 1 — COMPLETED / Dataverse foundation verified | Reuse accessible Account data and configured custom Sales Opportunity, Sales Quote, and Discount Request tables, relationships, forms, and test data. | Published app displayed the tested records; Discount Request N:1 model and record were verified. Full schema metadata and wider requirements remain open. | Dataverse modeling and relationships; original estimate 45–75 min. |
| 2 — COMPLETED / Dataverse reads verified | Read Account and linked Opportunity via agent tools. | Both documented records were retrieved and combined correctly; automatic tool selection was not separately verified. | Queries and agent context; original estimate 30–45 min. |
| 3 — COMPLETED / SharePoint retrieval verified (2026-10-15) | Ground product answers in the final SharePoint source. | All four documents and their source references were retrieved, including a repeat test after the temporary source was removed. Manual reindexing preceded success, but causality is not proven. | Grounding and citations; original estimate 30–60 min. |
| 4 — FUNCTIONALLY VERIFIED (2026-10-16) | Create an Opportunity through the existing controlled flow after agent confirmation. | Existing tool now passes `opportunityName`, `accountId`, `estimatedRevenue`, and `estimatedCloseDate`; test chat clarified a missing date, summarized all fields, waited for explicit confirmation, created the row, and returned a clickable link. The complete-input test link opened the row. Exact original T5 sample values and EUR currency are not verified. | Controlled writes, validation, and record links; no new flow or Topic is required for the verified PoC. |
| 5 — BLOCKED / published and installed; awaiting SmartPoint response | Verify use of the published agent in Teams after evaluating licensing/capacity guidance. | Evaluate response and retest a representative Teams interaction. No paid billing without explicit approval. | Channel and capacity diagnosis; 30–45 min after blocker addressed. |
| 6 — PENDING | Demonstrate and verify all five MVP acceptance points end to end in Teams. | Reproducible Dataverse read, cited SharePoint response, confirmed Opportunity creation, returned link, and read-back. | End-to-end demo; 30–45 min. |

## SharePoint Knowledge: Evidence and Follow-ups

**Environment:** `CRM738378`; **agent:** `SmartPoint Sales Assistant`; **final knowledge source:** `Alpentec Sales Knowledge`.

- Site: https://crm738378.sharepoint.com/sites/SmartPointSalesKnowledge
- Document library: https://crm738378.sharepoint.com/sites/SmartPointSalesKnowledge/Shared%20Documents/. The site URL came from the actual SharePoint site and the library URL from the browser address of the existing library. Document links were checked through agent source references; these tenant-specific URLs were not supplied by Microsoft documentation.
- The agent retrieves all four uploaded Alpentec DOCX documents and returns sources. A repeated test after removing `Alpentec Sales Documents Test` retrieved all four again.
- Search visibility and automatic index management were enabled during initial troubleshooting. The library was submitted for reindexing through **Library Settings → More Library Settings → Advanced Settings → Reindex Document Library**, reason `Library content missing from search results`. Successful retrieval was seen afterwards, but manual reindexing is a suspected rather than proven cause.
- Optional, non-blocking validation: upload a fifth test document and see whether it becomes searchable without a manual reindex.
- Presentation readiness: SharePoint currently displays a mixture of German and Dutch despite German Microsoft 365 user settings. Investigate site language, regional settings, and multilingual configuration before a customer-facing presentation. No language settings were changed in this documentation task.

## S02 — Product Knowledge: T3/T4 Results

Both tests passed in Copilot Studio test chat on 2026-10-16. T3 returned an AX-200/AX-300 comparison with SharePoint citations. For T4, the original Italian VAT question first received a response using general model knowledge even though web search was disabled. After correction, the agent stated that Italian VAT information was unavailable in the company knowledge base. Do not claim an unverified permanent fix.

The German v3 reports [US-01](SmartPoint_US01_Customer_Overview_Technical_Report_DE_v3.pdf) and [US-02](SmartPoint_US02_Product_Knowledge_Technical_Report_DE_v3.pdf), and the German US-03 report [US-03](SmartPoint_US03_Sales_Opportunity_Technical_Report_DE_v2.pdf), are stored in this case directory. Their contents are unchanged. The US-03 v2 report remains an unstaged local report artifact and documents the 2026-10-16 tests.

## US-03 / S03 — Verified Implementation and Test Results

**Original user story:** “As an inside sales employee, I want to create an Opportunity by chat so that sales opportunities are entered into the system immediately.”

**Exact mandatory T5 scenario:** “Create an Opportunity: Huber Logistik wants three AX-200, approximately EUR 450,000.” The agent must ask for the missing expected close date, show a summary, obtain confirmation, create the record, and return a usable link to the Opportunity. This is the original acceptance scope; do not substitute the already verified AX-300 scenario.

**Existing foundation and resources:** `SmartPoint Sales Assistant` in `CRM738378`; existing Account lookup and Dataverse read tools; SharePoint source `Alpentec Sales Knowledge` with four Alpentec documents; agent tool `Create Sales Opportunity via Flow`; cloud flow `Create Sales Opportunity`; custom Dataverse table `Sales Opportunity`; and published `SmartPoint Sales Demo` app. The current tool accepts `opportunityName`, `accountId`, `estimatedRevenue`, and `estimatedCloseDate`. Earlier runs exposed an OData URL through `opportunityId`; the 2026-10-16 technical report now verifies a clickable record link returned to the agent and opened by the complete-input test. The exact current response property name and URL expression are not recorded in the report and should not be guessed.

**Verified behavior (2026-10-16, Copilot Studio test chat):** A complete-input test created `AX-300 CNC Machine Demo` for `Huber Logistik` with estimated revenue `35000` and expected close date `2026-12-15`; the returned link opened the row in `SmartPoint Sales Demo`. A separate missing-date test used `AX-300 CNC Machine Demo Missing Date`, also for `Huber Logistik` with `35000`: the agent asked for the date, then displayed all four fields and waited for an explicit confirmation. After confirmation, the flow completed and the agent returned one clickable success link. The report does not record the second row's ID or an independent read-back of that specific row. The user entered EUR in the test request, but an earlier app view displayed `$35,000.00`; the stored/displayed currency is not verified as EUR.

**T5 acceptance boundary:** The original required demo request is “Create an Opportunity: Huber Logistik wants three AX-200, approximately EUR 450,000,” with expected close date initially omitted. The tests verify the missing-field/confirmation/create/link behavior with different AX-300 / `35000` sample data. Do not claim that the exact three-machine/approximately-EUR-450,000 scenario or its currency handling was tested. The flow-side missing-input condition exists as a defensive layer, but a direct negative test invoking the flow with incomplete inputs was not separately verified. The current PoC is instruction-driven; a dedicated Topic is not required or planned now. See the [unchanged German technical report](SmartPoint_US03_Sales_Opportunity_Technical_Report_DE_v2.pdf) and [flow notes](SALES_OPPORTUNITY_FLOW.md).

**Resolved implementation issues and lessons:** The initial missing-date test incorrectly allowed the agent to ask for confirmation without the required date. The instructions were corrected, and the repeated dialogue asked for the date before summarizing and confirming. Flow troubleshooting also resolved competing response-action issues, an `empty()` check applied to a numeric input, and an invalid Account lookup input. The valid Account GUID is supplied as the flow input; the Dataverse lookup must receive the proper entity reference. Earlier run-history diagnosis found that the visible `accountId` input used internal trigger property `text_1`; the historically verified expression was `concat('accounts(', triggerBody()?['text_1'], ')')`. No newer expression or current response property name should be inferred. The current server-side condition remains a defense-in-depth check. The separate `Create Customer` topic error `Input binding 'item.name' was not found.` is unrelated; do not retry it.

**Architecture decision:** Keep the current agent instructions, tool, and flow. Do not add a dedicated Topic or a second post-creation summary. The tested complete pre-creation summary and single explicit confirmation are sufficient for this PoC. The demo disables the duplicate tool-level confirmation while retaining agent-level confirmation; production write operations should enforce one clear approval step at the tool boundary.

## US-04 — Discount Approval (Bonus) Preparation

**Original user story:** “As a sales manager, I want to receive and approve discount requests through chat so that customer quotations can be processed faster.” The original assignment marks discount approval as a bonus capability.

**Exact requirement and T6 scenario:** A discount request above 10% starts a Power Automate Approvals request for sales management. The approval outcome is documented against the Opportunity, and the requester receives a Microsoft Teams notification. T6 is: “Ich brauche 15 % Rabatt für die Opportunity von eben.” Expected behavior: create the Discount Request, send approval to sales management, document the outcome, and notify the requester in Teams.

**Verified foundation:** The existing `Discount Request` table (`alp_rabattanfrage`) belongs to solution `SmartPoint Copilot Sales Case`, which remains separate from `SmartPoint Sales Demo`; both are in `CRM738378`. The table has `Name`, `Discount Percentage`, `Reason`, and `Approval Status` (`Pending`, `Approved`, `Rejected`), an N:1 relationship to the custom `Sales Opportunity` table, a configured main form and `Active Discount Requests` view, and is available in the published `SmartPoint Sales Demo` app. The `Alpentec CNC – 10% Discount Request` test row was created and read with related Opportunity `Alpentec CNC Machine Project`, 10%, and Pending status. This verifies the model/app only; no Copilot Studio integration, approval, workflow, or Teams notification is implemented.

**Model gaps to resolve before building:** The existing table has no documented `Approver` field or approval timestamps. The original case data-model proposal includes an approver, but the configured table field list does not. The assignment says the result is recorded against the Opportunity; current evidence only verifies `Approval Status` on the related Discount Request and does not establish an Opportunity-level status field or mapping. Confirm whether the existing Opportunity table has a suitable field; if not, plan the smallest approved schema addition or a clearly related Discount Request record without misrepresenting it as direct Opportunity-field persistence. Do not change the model in this preparation step. Also confirm whether `Reason` is required, how the “Opportunity of earlier” is resolved to a specific row, and which sales-management identity should receive approval.

**Implementation sequence (only after the feasibility gate):**

| Step | Objective and deliverable | Acceptance criteria | Estimate |
| --- | --- | --- | --- |
| 1. Feasibility and field mapping | Confirm Approvals and Teams connector availability, licensing, permissions, authenticated connections, approver identity, requester identity, and the target field for recording the outcome. Map existing Discount Request fields and Opportunity relationship; do not duplicate the verified table. | The required approval and notification actions can run in `CRM738378` with approved identities, and the decision can be persisted against the Opportunity as required. No paid billing is enabled without explicit approval. | 15–30 min for review; external licensing/permission resolution excluded |
| 2. Controlled request creation | Configure the agent to identify the Opportunity from the current conversation or ask for clarification, collect the discount percentage and any required reason, and create a related Discount Request through an existing or dedicated controlled flow. | The 15% T6 request creates one related row with the requested percentage and Pending status; no guessed Opportunity or invented reason is written. | 30–45 min |
| 3. Approval and persistence | Have Power Automate start a real Approvals action for the confirmed sales-management approver and write the result to the agreed Opportunity-related target and request status. | The actual approval outcome is recorded and the Discount Request reaches the corresponding status; no simulated approval is used. | 30–60 min |
| 4. Requester notification and T6 test | Send the requester a Teams notification after the real outcome; run the full chat scenario using the opportunity created in the preceding story. | T6 shows one correct related request, real approval routing and result persistence, and a notification to the correct requester. Capture chat, flow run, approval result, related rows, and Teams notification. | 30–45 min |

These are planning estimates, not assignment requirements; implementation is estimated at approximately 105–180 minutes after the feasibility gate, excluding external licensing/permission delays. The smallest useful demonstrable milestone is one real 15% request for the correctly identified Opportunity, one actual approval decision, persisted status/outcome, and one Teams notification. Do not begin until the connector/licensing/permission gate is clear. Do not silently replace Approval or Teams actions with a simulation.

**Known dependencies and risks:** The Teams agent channel currently fails with a usage-limit error; its licensing/capacity cause is suspected, not confirmed. This does not prove or disprove that the Power Automate Teams connector can send a message. Test the exact Approvals and Teams actions under the intended connections before relying on them. Confirm license entitlements, environment policies/DLP, connector permissions, approver availability, user identity available for requester notification, and whether a flow can update the Opportunity. Do not enable paid billing without explicit approval. The existing `Discount Request` fields may not satisfy direct Opportunity outcome logging without additional schema configuration. S01–S03 are foundations, but the full Teams MVP remains blocked by the separate channel issue; US-04 is bonus and must not reorder mandatory MVP/security work.

**Recommended first action when US-04 is scheduled:** In CRM738378, perform a read-only feasibility check in Power Automate for the Approvals action and the Teams notification action using the intended connections; verify licensing, environment permissions, the sales-management approver, the requester identity, and the Opportunity outcome field. Record the result before creating any flow or changing any Dataverse schema.

**Exactly one next project action:** Complete mandatory US-05 / T7 verification of user-context access and refusal for a restricted Region North request. US-04 / T6 remains a prepared bonus to pursue only after T7 and its feasibility gate. Do not create a flow, topic, table, or field as part of this status update.
