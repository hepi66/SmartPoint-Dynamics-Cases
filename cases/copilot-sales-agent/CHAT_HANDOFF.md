# SmartPoint Copilot Sales Agent — Chat Handoff

**Updated:** 2026-10-19
**Repository:** `D:\Projekte\SmartPoint-Dynamics-Cases`
**Authoritative case status and roadmap:** [PROJECT_PLAN.md](PROJECT_PLAN.md)
**Progress log:** [PROJECT_PROGRESS.md](PROJECT_PROGRESS.md)
**Technical flow notes:** [SALES_OPPORTUNITY_FLOW.md](SALES_OPPORTUNITY_FLOW.md)

## Project and working rules

This handoff covers the separate Copilot Studio Sales Agent case in environment `CRM738378`. It is distinct from the repository's original room-planning case in `CRM816895`; do not mix their environments, solutions, or status. Keep all planning, progress, handoff, flow notes, technical documentation, and case evidence in `cases/copilot-sales-agent/`. The case-local `PROJECT_PLAN.md` is authoritative for this case; the repository-root plan belongs to the original project. Repository content and filenames are in English. Work one story at a time: **Plan -> Implement -> Test -> Fix -> Accept -> Presentation -> Next story.** Do not repeat a full-project audit unless requirements or architecture change. Never claim unverified behavior. Do not store credentials, secrets, or access information from the email context PDF. Do not modify cloud resources or application code unless the next task authorizes it. Never enable paid billing without explicit approval.

## Verified environment and components

- Environment: `CRM738378`.
- Agent: `SmartPoint Sales Assistant`.
- Separate solutions in this environment: `SmartPoint Copilot Sales Case` and `SmartPoint Sales Demo`.
- Published model-driven app: `SmartPoint Sales Demo`.
- `Sales Opportunity` and `Sales Quote` are custom Dataverse tables, not standard Dynamics 365 Sales entities.
- SharePoint knowledge source: `Alpentec Sales Knowledge`, with four Alpentec DOCX documents and source citations.
- Opportunity write path: Copilot Studio tool `Create Sales Opportunity via Flow` -> cloud flow `Create Sales Opportunity` -> custom `Sales Opportunity` table (`cr233_salesopportunity`).
- Existing Discount Request table: `Discount Request` (`alp_rabattanfrage`) in `SmartPoint Copilot Sales Case`, related N:1 to Sales Opportunity.

## Story status and verified tests

### US-01 / S01 — Customer Overview

**Status: FUNCTIONAL ACCEPTANCE VERIFIED; end-to-end performance OPEN.** T1 returned four open Huber Logistik opportunities, sales stage, estimated revenue, expected close date, an open quote, and recent customer activity. T2 clarified the ambiguous name `Huber` between `Huber Logistik` and `Huber Maschinenbau GmbH`.

Follow-up verification on 2026-10-18 returned all 13 existing Sales Opportunities with readable status `Active` (`Aktiv`) and stage `New` (`Neu`); no unresolved numeric values appeared. Other status and stage values are not present in the existing records and remain untested. End-to-end response time under 10 seconds remains unverified. Activity retrieval is verified for the recorded result; its schema and field mappings are not documented.

**Package A — Automatic Account Number: COMPLETED / VERIFIED for demonstrated scope.** In `CRM738378`, standard Dataverse `Account.accountnumber` uses native Autonumber: prefix `CUST`, minimum six digits, start value `1`. Creating `SmartPoint Autonumber Test` through `SmartPoint Sales Demo` generated `CUST-000001` after save. `Huber Logistik` retained `HL-1001`, and Copilot resolved `CUST-000001` to `SmartPoint Autonumber Test`. Lookup by name/account number and ambiguous `Huber` clarification are verified. Explicit number display across all intended forms/views is not separately recorded. Production readiness still needs a customer-approved format/uniqueness policy, duplicate and unintended-manual-change controls, backfill for Accounts without numbers, and safe handling of ambiguous/missing matches.

**Agent scope:** The `Create Customer` topic is disabled, and the saved agent instructions explicitly prohibit Account creation through Copilot. Sales Opportunities may be created only for existing, unambiguously identified Dataverse Accounts. A combined request to create a customer and then an Opportunity created neither record; that sequence is now explicitly out of scope, and Copilot Account creation is not verified. The Huber Logistik regression reached the complete confirmation step for `SmartPoint Regression Test` (revenue `10,000 EUR`, expected close date `2026-11-30`). The Opportunity was not created; do not report it as created.

A combined Copilot query retrieved related Dataverse Sales Opportunities, customer Activities, and SharePoint knowledge from three documents with source citations and appropriate demo-content disclaimers. Execution trace durations were Accounts 0.88 s; Opportunities with Labels 0.44 s; Quotes 0.35 s; Customer Activities 0.42 s; SharePoint knowledge search 7.91 s; Opportunities 0.68 s. These individual actions do not establish end-to-end user-perceived response time. The under-10-second criterion remains OPEN and must be measured in Teams, the intended end-user channel; Teams testing is BLOCKED by the existing usage-limit/access-channel issue. Do not optimize prematurely. SharePoint search is only a potential optimization candidate if future Teams measurements justify it.

2026-10-18 Copilot Studio tests of `Get Sales Quotes with Status` verified both existing quotes. `QUO-2026-001` returned `Draft`, `Alpentec CNC Machine Project`, and `150,000.00 USD`; `QUO-2026-002` returned `Sent`, `Logistics Fleet Modernization`, and `85,000.00 USD`. The agent correctly returned amount, currency, status, and related Opportunity for both. A direct query, “Welche Angebote gibt es zur Verkaufschance Logistics Fleet Modernization?”, returned exactly `QUO-2026-002` and excluded `QUO-2026-001`; direct Opportunity-focused lookup is VERIFIED / COMPLETED for this tested Opportunity. The Opportunity has Estimated Revenue `85,000 EUR`; its related quote has TotalAmount `85,000 USD`, on separate records with separate currencies. On the published Sales Opportunity form, `Estimated Revenue (Base)` was removed; live verification showed Estimated Revenue `€350,000` and Currency `Euro`. The underlying Dataverse base-currency column remains unchanged.

### US-02 / S02 — Product Knowledge

T3 and T4 passed in Copilot Studio test chat on 2026-10-16. T3 compared AX-200 with AX-300 using SharePoint citations. T4 asked about Italian VAT: the first answer used general model knowledge despite web search being disabled; after the agent instructions were corrected, the agent said the information was unavailable in the company knowledge base. This verifies those test responses, not a universal grounding guarantee.

Four Alpentec documents were uploaded to the SmartPoint Sales Knowledge site. The final `Alpentec Sales Knowledge` source retrieved all four and returned source references, including in a repeat test after removing a temporary source. Manual library reindexing preceded successful retrieval, but its causal role is not proven. Optional follow-ups include automatic indexing validation and consistent German SharePoint UI language.

### US-03 / S03 — Create Opportunity

The existing tool and flow now use four inputs: `opportunityName`, `accountId`, `estimatedRevenue`, and `estimatedCloseDate`. On 2026-10-16 in Copilot Studio test chat:

- A complete-input test created `AX-300 CNC Machine Demo` for `Huber Logistik`, revenue `35000`, close date `2026-12-15`. One confirmation preceded the write; the returned clickable link opened the row in `SmartPoint Sales Demo`.
- A missing-date test for `AX-300 CNC Machine Demo Missing Date` asked for the close date. After it was supplied, the agent showed all four values, waited for explicit confirmation, created the Opportunity, and returned a clickable link.
- The report does not give the second record's ID or an independent read-back for that specific run.
- A test request said EUR, while a prior Power Apps view displayed `$35,000.00`; that 2026-10-16 create test did not verify stored currency. The separate exact T5 test on 2026-10-18 subsequently verified EUR persistence for the required scenario.
- The exact original T5 input had not been used in the 2026-10-16 tests; it was tested and passed on 2026-10-18 as documented below.
- A separate negative test of the flow-side required-input condition was not documented. The defensive condition exists, but its behavior under an incomplete direct flow call is unverified.

**Exact T5 scenario — VERIFIED / COMPLETED (2026-10-18):** The agent requested the missing close date for the three AX-200 / approximately EUR 450,000 request, summarized it, waited for confirmation, created the Opportunity, and returned a working Dataverse link. Independent read-back verified `Drei AX-200 Maschinen`, Account `Huber Logistik`, Estimated Revenue `450000 EUR`, Currency `Euro`, Expected Close Date `2026-12-15`, Sales Stage `New`, record ID `3eb38884-d3ca-f111-a05a-000d3a0a2928`. The separate direct flow-side missing-input negative test remains an additional unverified robustness check.

Architecture decisions: keep the existing agent instructions, tool, and flow; do not add a dedicated Copilot Studio Topic for this PoC. The pre-write summary and single agent confirmation are sufficient. Tool-level confirmation is disabled in the demo to avoid a duplicate prompt; production writes should enforce one clear approval at the tool boundary. Do not add a redundant post-creation summary.

The verified 2026-10-15 record is `AX-300 CNC Machine Final Test`, Account `Huber Logistik`, record ID `c2ade295-5ec8-f111-a05a-002248d597bf`, independently opened in `SmartPoint Sales Demo`. The documented app URL uses app ID `37590ede-88c7-f111-a05a-000d3a0a2928` and table `cr233_salesopportunity`. The 2026-10-16 technical report describes the later clickable-link test but does not record its response-property name or URL expression.

## Important failures, resolutions, and decisions

- Initial T5-style missing-date dialogue offered creation without the required date. Agent instructions were tightened; the retest asked for the date before summary and confirmation.
- Initial Dataverse lookup failures included `ODataUnrecognizedPathException` from an invalid lookup input and an empty `accounts()` reference caused by reading `triggerBody()?['accountId']` instead of the then-observed internal property `text_1`. Run history exposed the actual trigger output. The historically verified expression used an OData reference: `concat('accounts(', triggerBody()?['text_1'], ')')`. Do not guess current internal property names if the flow changes.
- A flow validation check incorrectly applied `empty()` to a numeric value and failed at runtime; validation was corrected. A direct negative test of the corrected server-side condition remains unverified.
- Conflicting flow response-action issues were resolved; the final report confirms the agent receives a clickable record link. Exact current response schema is not stated, so do not invent a property name such as `opportunityUrl`.
- The unrelated `Create Customer` topic still has the error `Input binding 'item.name' was not found.` Do not retry it as part of Opportunity creation.
- The initial quote lookup attempt `_spdemo_salesopportunity_value` failed with a Dataverse property-not-found error. The verified Sales Quote -> Sales Opportunity lookup logical name is `spdemo_cr233_salesopportunity`; using that relationship resolved the quote lookup and added `OpportunityName` to the quote output.
- A formula-column approach for the CRM738378 Dataverse model was rejected by the editor and discarded without a schema change.
- The Discount Request model was verified independently; this does not mean its Copilot integration or approval workflow exists.

## US-04 / T6 — Package B, Second Priority (Bonus)

Original user story: a sales manager wants to receive and approve discount requests through chat. For discounts above 10%, Power Automate Approvals must route a real approval to sales management; the outcome must be recorded against the Opportunity; and the requester must receive a Microsoft Teams notification. T6 is `Ich brauche 15 % Rabatt für die Opportunity von eben.` Expected: create the request, send approval, record the outcome, and notify the requester.

**Stage 1 — COMPLETED for verified scope (CRM738378):** Existing `Discount Request` (`alp_rabattanfrage`) has an N:1 relationship to Sales Opportunity. Its main form and `Active Discount Requests` view work in `SmartPoint Sales Demo`. The existing `Alpentec CNC – 10% Discount Request` record was verified with related Opportunity `Alpentec CNC Machine Project`, Discount Percentage `10.00`, Reason “Customer requested a 10% discount to support the purchase decision for the CNC machine project.”, and Approval Status `Pending`. Required form fields are Sales Opportunity, Discount Percentage, Reason, and Approval Status. Agreed minimal design: demo approver is the CRM738378 environment administrator; persist outcome in `Discount Request.Approval Status`; no additional fields or duplicate tables.

**Stage 2 — COMPLETED / VERIFIED for tested approval/rejection outcomes (2026-10-19):** In `CRM738378`, the agent created a 12% Discount Request that was approved and a 15% request that was rejected. Both decisions persisted in Dataverse. This verifies the tested outcomes, but the `>10%` threshold logic itself is not independently verified. Connector identity, licensing, permissions/DLP, and broader flow feasibility have not been fully audited.

**Stage 3 — COMPLETED / VERIFIED for tested scope (2026-10-19):** The agent created Discount Requests after explicit user confirmation. The 12% request was approved and the 15% request rejected; both outcomes persisted in Dataverse. New agent flow `Get Discount Request Approval Status` retrieves a Discount Request by record ID. For record `64c07bb4-9dcb-f111-a05a-7ced8d05c265`, the flow returned `Rejected`; the agent invoked it and reported “abgelehnt (Rejected)”. The `>10%` threshold itself remains unverified.

Package B follows completed Package A and had an estimated total effort of approximately 2.5–4.5 hours. Stages 1–3 are completed / verified for their documented tested scopes. Dominic confirmed Teams integration is not required for this practical case, so Stage 4 is SKIPPED / NOT REQUIRED. The `>10%` threshold remains OPEN / NOT VERIFIED; no production-readiness claim is made. Package B practical implementation and S04 / T6 technical reporting are complete for demonstrated scope. The next priority is demo preparation and interview Q&A. Detailed status is in [PROJECT_PLAN.md](PROJECT_PLAN.md). Do not recreate the verified table.

**Priority rule:** Package B practical implementation and S04 / T6 technical documentation are complete for demonstrated scope. The next priority is demo preparation and interview Q&A. US-05 / T7 remains BLOCKED / NOT VERIFIED; do not schedule further implementation automatically, and do not treat model-driven app RBAC as a substitute for agent-level verification.

## Other open work and blockers

- Teams/end-user agent test: currently unavailable due to access/channel/licensing limitations; the cause is unconfirmed and external SmartPoint clarification remains outstanding. The environment has 1,000 allocated legacy sessions, but no Copilot Credits or billing plan were found. T7 / US-05 is BLOCKED / NOT VERIFIED. Do not run further licensing or channel experiments while awaiting clarification; do not enable paid billing without explicit approval. Separately verified model-driven app RBAC results do not prove agent-specific authorization.
- Practical implementation and technical reporting are finished for the demonstrated scope. The next priority is demo preparation and interview Q&A. Package B Stage 4 is SKIPPED / NOT REQUIRED per Dominic's clarification. The `>10%` threshold is OPEN / NOT VERIFIED. T7 is BLOCKED / NOT VERIFIED pending the existing access/channel limitation and external clarification; the app RBAC result does not prove agent authorization. Do not schedule further implementation automatically or claim production readiness. M-10 documentation remains deferred unless specifically resumed as part of preparation. Do not infer end-to-end latency from action timings or enable paid billing without approval.
- Entra/Dataverse effective permissions, the full Teams end-to-end MVP, broader governance/DLP/ALM/monitoring, and product readiness remain open per the project plan.
- The expected solution explanation/demo deliverable still needs to cover the architecture, Dataverse model, orchestration decision, security, and repeatable evidence-based live scenarios.
- Bonus Sales Hub integration and extended ALM remain unimplemented.

## Source and report paths

- Original assignment: [Hepberger Copilot Studio Example - SmartPoint.pdf](Hepberger_Copilot_Studio_Beispiel-smartpoint.pdf).
- Approved US-01 report: [SmartPoint_US01_Customer_Overview_Technical_Report_DE_v3.pdf](SmartPoint_US01_Customer_Overview_Technical_Report_DE_v3.pdf).
- Approved US-02 report: [SmartPoint_US02_Product_Knowledge_Technical_Report_DE_v3.pdf](SmartPoint_US02_Product_Knowledge_Technical_Report_DE_v3.pdf).
- US-03 completion report: [SmartPoint_US03_Sales_Opportunity_Technical_Report_DE_v2.pdf](SmartPoint_US03_Sales_Opportunity_Technical_Report_DE_v2.pdf).
- US-04 completion report: [SmartPoint_US04_Discount_Approval_Technical_Report_DE.pdf](SmartPoint_US04_Discount_Approval_Technical_Report_DE.pdf).
- Flow design and verification: [SALES_OPPORTUNITY_FLOW.md](SALES_OPPORTUNITY_FLOW.md).
- Four SharePoint knowledge documents are under `sharepoint-knowledge/`.
- An email context PDF is also present in the case directory and contains access information. Do not commit it or copy its sensitive contents.

## Git handoff

The repository is on branch `main`, tracking `origin/main`. Keep local assignment, report, SharePoint knowledge, and email-context inputs unstaged unless a task explicitly authorizes handling them. The email-context PDF contains access information and must not be committed or quoted. Never delete or overwrite untracked inputs during a documentation repair.
