# SmartPoint Copilot Sales Agent — Chat Handoff

**Updated:** 2026-10-16
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

T1 and T2 passed in Copilot Studio. T1 returned four open Huber Logistik opportunities, sales stage, estimated revenue, expected close date, an open quote, and recent customer activity. T2 clarified the ambiguous name `Huber` between `Huber Logistik` and `Huber Maschinenbau GmbH`.

Known limits: stage is a numeric Choice rather than a readable label; currency display is not reliably verified and must not be called EUR; end-to-end response time under 10 seconds is unverified. Activity schema and field mappings are not documented. These limits are recorded, not silently fixed.

### US-02 / S02 — Product Knowledge

T3 and T4 passed in Copilot Studio test chat on 2026-10-16. T3 compared AX-200 with AX-300 using SharePoint citations. T4 asked about Italian VAT: the first answer used general model knowledge despite web search being disabled; after the agent instructions were corrected, the agent said the information was unavailable in the company knowledge base. This verifies those test responses, not a universal grounding guarantee.

Four Alpentec documents were uploaded to the SmartPoint Sales Knowledge site. The final `Alpentec Sales Knowledge` source retrieved all four and returned source references, including in a repeat test after removing a temporary source. Manual library reindexing preceded successful retrieval, but its causal role is not proven. Optional follow-ups include automatic indexing validation and consistent German SharePoint UI language.

### US-03 / S03 — Create Opportunity

The existing tool and flow now use four inputs: `opportunityName`, `accountId`, `estimatedRevenue`, and `estimatedCloseDate`. On 2026-10-16 in Copilot Studio test chat:

- A complete-input test created `AX-300 CNC Machine Demo` for `Huber Logistik`, revenue `35000`, close date `2026-12-15`. One confirmation preceded the write; the returned clickable link opened the row in `SmartPoint Sales Demo`.
- A missing-date test for `AX-300 CNC Machine Demo Missing Date` asked for the close date. After it was supplied, the agent showed all four values, waited for explicit confirmation, created the Opportunity, and returned a clickable link.
- The report does not give the second record's ID or an independent read-back for that specific run.
- A test request said EUR, while a prior Power Apps view displayed `$35,000.00`; stored/displayed currency was not verified as EUR.
- The exact original T5 input — Huber Logistik wants three AX-200 for approximately EUR 450,000 — was not used in the recorded tests. The generic clarification/confirmation/write/link behavior is verified with the recorded AX-300 samples; do not claim the literal T5 business-data variant or EUR currency was verified.
- A separate negative test of the flow-side required-input condition was not documented. The defensive condition exists, but its behavior under an incomplete direct flow call is unverified.

Architecture decisions: keep the existing agent instructions, tool, and flow; do not add a dedicated Copilot Studio Topic for this PoC. The pre-write summary and single agent confirmation are sufficient. Tool-level confirmation is disabled in the demo to avoid a duplicate prompt; production writes should enforce one clear approval at the tool boundary. Do not add a redundant post-creation summary.

The verified 2026-10-15 record is `AX-300 CNC Machine Final Test`, Account `Huber Logistik`, record ID `c2ade295-5ec8-f111-a05a-002248d597bf`, independently opened in `SmartPoint Sales Demo`. The documented app URL uses app ID `37590ede-88c7-f111-a05a-000d3a0a2928` and table `cr233_salesopportunity`. The 2026-10-16 technical report describes the later clickable-link test but does not record its response-property name or URL expression.

## Important failures, resolutions, and decisions

- Initial T5-style missing-date dialogue offered creation without the required date. Agent instructions were tightened; the retest asked for the date before summary and confirmation.
- Initial Dataverse lookup failures included `ODataUnrecognizedPathException` from an invalid lookup input and an empty `accounts()` reference caused by reading `triggerBody()?['accountId']` instead of the then-observed internal property `text_1`. Run history exposed the actual trigger output. The historically verified expression used an OData reference: `concat('accounts(', triggerBody()?['text_1'], ')')`. Do not guess current internal property names if the flow changes.
- A flow validation check incorrectly applied `empty()` to a numeric value and failed at runtime; validation was corrected. A direct negative test of the corrected server-side condition remains unverified.
- Conflicting flow response-action issues were resolved; the final report confirms the agent receives a clickable record link. Exact current response schema is not stated, so do not invent a property name such as `opportunityUrl`.
- The unrelated `Create Customer` topic still has the error `Input binding 'item.name' was not found.` Do not retry it as part of Opportunity creation.
- A formula-column approach for the CRM738378 Dataverse model was rejected by the editor and discarded without a schema change.
- The Discount Request model was verified independently; this does not mean its Copilot integration or approval workflow exists.

## US-04 / T6 — Bonus story to prepare

Original user story: a sales manager wants to receive and approve discount requests through chat. For discounts above 10%, Power Automate Approvals must route a real approval to sales management; the outcome must be recorded against the Opportunity; and the requester must receive a Microsoft Teams notification. T6 is `Ich brauche 15 % Rabatt für die Opportunity von eben.` Expected: create the request, send approval, record the outcome, and notify the requester.

Existing verified foundation: `Discount Request` has `Name`, `Discount Percentage`, `Reason`, `Approval Status` (Pending/Approved/Rejected), and an N:1 relationship to Sales Opportunity; its form, `Active Discount Requests` view, app navigation, and one Pending test record were verified. No agent-driven request, approval, flow, or Teams notification is implemented.

Open design/dependency questions: the current documented table does not include an Approver field or approval timestamps. The original data-model proposal includes an approver, and the assignment says the result is documented against the Opportunity; current evidence only confirms Approval Status on the related Discount Request. Confirm whether an Opportunity field exists for the outcome or whether a minimal schema change is required. Confirm whether Reason is required, how the “Opportunity of earlier” resolves to the correct row, the designated sales-management approver, requester identity, Power Automate Approvals availability, Teams message action availability, licenses, environment permissions, DLP, and connections. Teams agent usage-limit failure does not prove that the Power Automate Teams connector is unavailable. Do not simulate approval or notification; do not enable paid billing without explicit approval.

**First action when US-04 is scheduled:** Perform a read-only feasibility and field-mapping check in CRM738378 for the Approvals action, Teams notification action, licensing, permissions, approver/requester identities, and Opportunity outcome target. Only after this passes should implementation begin. The minimum demo milestone is one correct 15% request for the intended Opportunity, a real approval decision, persisted outcome, and one Teams notification. See the case plan for sequence, acceptance, and estimates.

## Other open work and blockers

- Teams agent channel: `SmartPoint Sales Assistant` is published and installed, and Copilot Studio test chat works. Teams requests fail with a usage-limit error. The environment shows 1,000 allocated legacy sessions but no Copilot Credits or billing plan. Licensing/capacity is suspected, not confirmed. Dominic Bley was contacted on 2026-10-15; his automatic reply said he would be available again from 2026-10-18. Evaluate any guidance and retest in Teams; do not enable paid billing without explicit approval.
- US-05 / T7 regional permissions and user-context security remain unverified.
- The active next project action is mandatory US-05 / T7 verification of user-context access and refusal for a restricted Region North request. Pursue the prepared US-04 bonus only after T7.
- Entra/Dataverse effective permissions, the full Teams end-to-end MVP, broader governance/DLP/ALM/monitoring, and product readiness remain open per the project plan.
- The expected solution explanation/demo deliverable still needs to cover the architecture, Dataverse model, orchestration decision, security, and repeatable evidence-based live scenarios.
- Bonus Sales Hub integration and extended ALM remain unimplemented.

## Source and report paths

- Original assignment: [Hepberger Copilot Studio Example - SmartPoint.pdf](Hepberger_Copilot_Studio_Beispiel-smartpoint.pdf).
- Approved US-01 report: [SmartPoint_US01_Customer_Overview_Technical_Report_DE_v3.pdf](SmartPoint_US01_Customer_Overview_Technical_Report_DE_v3.pdf).
- Approved US-02 report: [SmartPoint_US02_Product_Knowledge_Technical_Report_DE_v3.pdf](SmartPoint_US02_Product_Knowledge_Technical_Report_DE_v3.pdf).
- US-03 completion report: [SmartPoint_US03_Sales_Opportunity_Technical_Report_DE_v2.pdf](SmartPoint_US03_Sales_Opportunity_Technical_Report_DE_v2.pdf). This remains a local unstaged report artifact; do not edit its content.
- Flow design and verification: [SALES_OPPORTUNITY_FLOW.md](SALES_OPPORTUNITY_FLOW.md).
- Four SharePoint knowledge documents are under `sharepoint-knowledge/`.
- An email context PDF is also present in the case directory and contains access information. Do not commit it or copy its sensitive contents.

## Git handoff

The repository is on branch `main`, tracking `origin/main`. Keep local assignment, report, SharePoint knowledge, and email-context inputs unstaged unless a task explicitly authorizes handling them. The email-context PDF contains access information and must not be committed or quoted. Never delete or overwrite untracked inputs during a documentation repair.
