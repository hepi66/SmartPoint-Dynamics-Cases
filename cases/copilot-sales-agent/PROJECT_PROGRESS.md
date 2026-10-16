# Copilot Studio Sales Agent Project Progress

## Current Status (2026-10-16)

**Environment:** `CRM738378`
**Agent:** `SmartPoint Sales Assistant`

S01 / US-01 is functionally complete and accepted after T1 and T2 passed. Known non-blocking limitations remain: sales stage is returned as a numeric Choice value, currency is not reliably verified and must not be assumed to be EUR, and end-to-end response time below 10 seconds is unverified. S02 / US-02 is functionally accepted after T3 and T4 passed in Copilot Studio test chat. T4 initially used general model knowledge despite web search being disabled; after correction, the agent stated that Italian VAT information was unavailable in the company knowledge base. S03 / US-03 is next for preparation; implementation and T5 testing have not started. The repository-root [PROJECT_PLAN.md](../../PROJECT_PLAN.md) is the authoritative roadmap and status source.

## Approved Story Workflow

Work on one story at a time: **Plan -> Implement -> Test -> Fix -> Accept -> Presentation -> Next story.** Prepare a presentation covering requirements, implementation, architecture, and test evidence after each accepted story and before implementation/testing of the next story. The approved S01 and S02 German v3 reports were not found locally; do not substitute or regenerate them. Do not repeat full-project audits unless requirements or architecture materially change. Partial evidence does not equal acceptance.

## Verified Work

- The `SmartPoint Sales Assistant`, Dataverse Add Row/List Rows tools, and Dataverse connection in `CRM738378` are documented as verified foundations.
- `Accounts List Rows` retrieved `Huber Logistik`. Read-only tool `Get Sales Opportunities` retrieved `Logistics Fleet Modernization` from `Sales Opportunities`. The Opportunity's Account ID is `177a9152-afc7-f111-a05a-000d3a0a2928`; Opportunity ID is `5aa34222-b0c7-f111-a05a-000d3a0a2928`.
- A business-oriented combined query returned correct customer and Opportunity information and confirmed their Dataverse relationship. The final execution trace did not separately establish that both tools were automatically selected.
- `SmartPoint Copilot Sales Case` and `SmartPoint Sales Demo` are separate solutions in the same environment. The published `SmartPoint Sales Demo` app displays the custom Sales Opportunity and Sales Quote tables. The separate solution's Discount Request table is also available in that app.
- The root plan records the MVP Dataverse foundation, Account and Opportunity records, saved Quotes, and the relational `Discount Request` custom table (`alp_rabattanfrage`) with its form, view, and verified test record.
- M-02, at least one related custom Dataverse table, is verified through `Discount Request` and its N:1 relationship to Sales Opportunity. This does not verify an agent query or the US-04 approval workflow.
- T1 passed: the agent returned four open Huber Logistik Opportunities with stage, estimated revenue, expected close date, an open Sales Quote, and recent customer activity. T2 passed: ambiguous `Huber` prompted clarification between `Huber Logistik` and `Huber Maschinenbau GmbH`. Non-blocking T1 limits are the numeric Choice stage, unverified currency, and unverified under-10-second response time.
- Agent-driven Account creation was verified for its specific path. It is not the central mandatory story.
- The `Create Customer` topic asks for `customerName`; its confirmation message works. The explicit Add Row action remains blocked by `Input binding 'item.name' was not found.` This does not prove record creation.
- Four Alpentec DOCX documents were uploaded to `SmartPoint Sales Knowledge` on 2026-10-14. Two SharePoint knowledge sources were configured and marked Ready; library search visibility was enabled.
- On 2026-10-15, the final `Alpentec Sales Knowledge` source retrieved all four documents and returned source references. After the temporary `Alpentec Sales Documents Test` source was removed, a repeated test again retrieved all four documents.
- On 2026-10-15, the `Create Sales Opportunity` flow succeeded manually at 07:35 and from the agent at 07:43. One confirmation preceded creation of `AX-300 CNC Machine Final Test` for `Huber Logistik`; the returned ID was `c2ade295-5ec8-f111-a05a-002248d597bf`. The record was independently found and opened in `SmartPoint Sales Demo`. See [SALES_OPPORTUNITY_FLOW.md](SALES_OPPORTUNITY_FLOW.md).
- On 2026-10-16, T3 returned an AX-200/AX-300 comparison with SharePoint citations. For T4, the original Italian VAT question first received general model knowledge despite web search being disabled; after correction, the agent stated that the information was unavailable in the company knowledge base. No persistent fix beyond the observed response is claimed.

## Incomplete and Blocked

- Teams: the agent is published and installed, and test chat works, but Teams requests fail with a usage-limit error. `CRM738378` has 1,000 allocated legacy sessions, but no Copilot Credits or billing plan were found. Licensing/capacity is suspected, not confirmed. SmartPoint guidance is pending. Do not enable paid billing without explicit approval.
- The full end-to-end Teams MVP test remains open. Entra/Dataverse permissions, user-context/regional restrictions, and T7 also remain open.
- The current flow has not met full T5: the exact requested scenario, expected close-date clarification, complete pre-write summary, and dynamic clickable `opportunityUrl` remain open. The separate `item.name` binding issue must not be retried.
- The underlying Activity schema and field mappings remain undocumented, although T1 returned recent activity.
- Manual SharePoint library reindexing preceded successful retrieval, but causality is not established. A fifth-document automatic-indexing check is optional and non-blocking. The SharePoint UI has mixed German and Dutch; language settings have not been changed.
- US-04 approval workflow, Sales Hub, additional clarification skills, and extended ALM are bonus work and remain unimplemented/unverified.

## Exactly One Next Project Action

Begin S03 / US-03 planning by inspecting the existing `Create Sales Opportunity` flow and mapping its inputs, Dataverse fields, and outputs against original T5. Do not implement or test S03 yet. S02 presentation materials are a workflow prerequisite before S03 implementation/testing; the approved German v3 reports were not locally available and were not substituted. See [PROJECT_PLAN.md](PROJECT_PLAN.md) for the exact T5 requirements and preparation sequence.

## Work History

| Date | Verified progress |
| --- | --- |
| 2026-10-13 | Case documents were initialized. Environment and source materials were not yet inspected; no implementation was claimed. |
| 2026-10-13 | Original requirements and project context were reviewed. US-01–US-05, T1–T7, tenant-provisioning uncertainty, the permitted custom-table approach, and the `item.name` blocker were recorded. Sensitive access information was not copied into repository text. |
| 2026-10-14 | Architecture First roadmap and six MVP implementation steps were documented. Existing agent, Dataverse connection, and verified foundations were recorded as completed prerequisites. |
| 2026-10-14 | The CRM738378 Dataverse foundation was recorded; Activity/full-case requirements remained open. Account and linked Opportunity read access was verified, including the limitation that automatic selection of both tools was not independently traced. |
| 2026-10-14 | Four Alpentec DOCX files were uploaded and two SharePoint knowledge sources marked Ready. Search returned no documents at that time; a library reindex was requested and the outcome was pending. |
| 2026-10-15 | All four documents and their source references were retrieved from the final SharePoint source. A repeat test after removing the temporary source succeeded. Manual reindexing preceded success, but causality was not proven. Optional automatic-indexing validation and mixed SharePoint UI language were recorded as follow-ups. |
| 2026-10-15 | Manual and agent-triggered Opportunity flow runs succeeded. The created record was independently opened in the model-driven app and its ID matched. Teams was published/installed but the usage-limit error and full Teams test remained unresolved. |
| 2026-10-15 | Audit baseline established with explicit US-01–US-05 and T1–T7 states, the one-story workflow, and the deferred dynamic link. S01 was then accepted after T1/T2; its display and timing limitations were retained. |
| 2026-10-16 | S02 was accepted after T3/T4 passed in Copilot Studio test chat. T4's initial general-knowledge response and corrected knowledge-gap response were both recorded without claiming a persistent fix. The approved German v3 reports were not available locally. S03 preparation was set as the next action; no S03 implementation or test was performed. |
