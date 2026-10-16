# Copilot Studio Sales Agent Project Plan

## Project Context

- **Project:** Copilot Studio Sales Agent
- **Environment:** `CRM738378`
- **Agent:** `SmartPoint Sales Assistant`
- **Requirements source:** [Hepberger Copilot Studio Example - SmartPoint.pdf](Hepberger_Copilot_Studio_Beispiel-smartpoint.pdf)
- **Case directory:** `cases/copilot-sales-agent/`
- **Progress log:** [PROJECT_PROGRESS.md](PROJECT_PROGRESS.md)
- **Authoritative project status:** the repository-root [PROJECT_PLAN.md](../../PROJECT_PLAN.md)

The separate email context document contains access information. Do not commit it or copy sensitive information from it into repository documents.

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
| US-03 / S03 — Create Opportunity | PREPARATION / NEXT; core create path verified, full acceptance open | The controlled flow, one confirmation, Opportunity creation, returned ID, and independent read-back in `SmartPoint Sales Demo` were verified on 2026-10-15. | Exact T5, required field collection, missing close-date clarification, summary/confirmation for that scenario, and a dynamically returned clickable record link remain open. | T5 partial evidence; acceptance open |
| US-04 — Discount Approval (bonus) | Dataverse table and relationship verified; workflow open | Custom `Discount Request` data model, relationship, form, view, and an app record are verified. | Approval workflow, Opportunity outcome logging, and Teams notification are not verified. | T6 BONUS / PENDING |
| US-05 — Regional Permissions | UNVERIFIED | No regional access enforcement is claimed. | User-context access and refusal for a restricted Region North request remain to be tested. | T7 PENDING |

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
| M-05 | Confirmed Opportunity creation and record link | Core create / confirmation / read-back VERIFIED; full T5 and link OPEN | Flow creation, returned ID, and independent app read-back are verified. The flow does not yet return a dynamic clickable `opportunityUrl`; exact T5 is not verified. See [Opportunity flow notes](SALES_OPPORTUNITY_FLOW.md). |
| M-06 | Entra ID authentication and effective Dataverse permissions | UNVERIFIED | Verify allowed and denied operations under intended identities/roles; tool availability alone does not prove enforcement. |
| M-07 | User-context access and regional restriction | UNVERIFIED | Verify access in the signed-in user context and denial for the restricted Region North customer. |
| M-08 | Microsoft Teams deployment and use | BLOCKED — published/installed, usage-limit error | Test chat works; Teams request fails. Suspected licensing/capacity cause is not confirmed. Await SmartPoint guidance; do not enable paid billing without approval. |
| M-09 | Demo scenarios T1–T7, mandatory first | IN PROGRESS — T1–T4 PASS; T5–T7 PENDING | Complete T1–T5 and T7 before bonus T6; capture input, observed result, and evidence. |
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
| 4 — Core create/confirmation/read-back verified; full T5 and record link open | Create an Opportunity through the controlled flow after confirmation. | Manual and agent runs, one confirmation, returned ID, and independent app read-back passed on 2026-10-15. Exact T5 and a dynamic clickable link remain open. | Controlled writes and flow integration; complete core path, remaining work follows US-03. |
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

The user approved these German v3 presentation artifacts: `SmartPoint_US01_Customer_Overview_Technical_Report_DE_v3.pdf` and `SmartPoint_US02_Product_Knowledge_Technical_Report_DE_v3.pdf`. Neither exact file was found locally in the repository, so neither was included or substituted. Do not regenerate, translate, or edit them; add the approved originals only if they become locally accessible.

## US-03 / S03 Preparation — Next Story

**Original user story:** “As an inside sales employee, I want to create an Opportunity by chat so that sales opportunities are entered into the system immediately.”

**Exact mandatory T5 scenario:** “Create an Opportunity: Huber Logistik wants three AX-200, approximately EUR 450,000.” The agent must ask for the missing expected close date, show a summary, obtain confirmation, create the record, and return a usable link to the Opportunity. This is the original acceptance scope; do not substitute the already verified AX-300 scenario.

**Existing foundation and resources:** `SmartPoint Sales Assistant` in `CRM738378`; existing Account lookup and Dataverse read tools; SharePoint source `Alpentec Sales Knowledge` with four Alpentec documents; agent tool `Create Sales Opportunity via Flow`; cloud flow `Create Sales Opportunity`; custom Dataverse table `Sales Opportunity`; and published `SmartPoint Sales Demo` app with a verified record URL pattern. The flow currently accepts only `opportunityName` and `accountId`. Its observed `opportunityId` output was an OData record URL, from which the agent displayed the GUID. It does not yet return a dynamically constructed clickable `opportunityUrl`.

**Required behavior and acceptance:** Collect customer, topic/product, estimated revenue, and expected close date; ask for missing information; show the complete proposed Opportunity summary; require explicit confirmation before writing; create the Opportunity through the controlled flow; return a clickable link that opens the newly created row. The exact T5 scenario, missing-date question, and link return have not been verified. Keep the separate `Create Customer` error `Input binding 'item.name' was not found.` documented and do not retry that path.

**Practical sequence:** (1) Inspect the existing flow trigger, create-row mappings, agent tool inputs, and response outputs against T5. (2) Plan the smallest additions needed for product/topic, revenue, close date, and dynamic URL using existing table/app metadata; do not assume a URL expression before confirming its inputs. (3) Configure the agent to ask for the missing close date and present all required values in one summary. (4) Retain one explicit confirmation before the write and avoid duplicate prompts. (5) Return the created row's usable app link to the agent after success. (6) Only after implementation, run the exact T5 prompt in Copilot Studio test chat: verify the close-date clarification, summary, one confirmation, successful create, returned link opening the created record, and independent read-back. Capture the conversation, flow run, and opened record. None of these S03 steps are being implemented or tested in this update.

**Prerequisites and dependencies:** The working Copilot Studio test chat and current Dataverse/SharePoint foundations are available. Teams remains blocked by a usage-limit error; licensing/capacity is suspected, not confirmed, and paid billing must not be enabled without explicit approval. This blocks full MVP acceptance in Teams but not S03 preparation or test-chat development. Complete the S02 presentation gate before S03 implementation/testing. No additional table or Sales package is currently identified as necessary. **Planning estimate for remaining S03 implementation and T5 verification: approximately 45–75 minutes**, assuming the existing flow and metadata remain available; this is an estimate, not a case requirement.

**Exactly one next project action:** Begin the S03 / US-03 Plan stage by inspecting the existing `Create Sales Opportunity` flow and mapping its inputs, Dataverse fields, and response against the original T5 acceptance criteria. Do not implement or test S03 in this preparation update. S01 limitations remain non-blocking and unchanged; S02 T3/T4 are PASS; Teams remains BLOCKED pending SmartPoint guidance.
