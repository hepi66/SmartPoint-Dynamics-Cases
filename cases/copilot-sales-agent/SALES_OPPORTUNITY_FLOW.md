# SmartPoint Sales Assistant: Opportunity Creation Flow

## Status and components

The Copilot Studio → Power Automate → Dataverse path was functionally verified in environment `CRM738378` on 2026-10-15 and 2026-10-16. The 2026-10-16 Copilot Studio test verified a missing-close-date clarification, a complete pre-write summary, one explicit confirmation, flow creation, and a clickable record link. In the complete-input test, the link opened the created record in the `SmartPoint Sales Demo` model-driven app. The exact original T5 sample data was not used; see the acceptance boundary below.

| Component | Configuration |
| --- | --- |
| Copilot Studio agent | `SmartPoint Sales Assistant` |
| Agent tool | `Create Sales Opportunity via Flow` |
| Cloud flow | `Create Sales Opportunity` |
| Dataverse table | Custom `Sales Opportunity` (`cr233_salesopportunity`) |
| Environment | `CRM738378` |
| Model-driven app | `SmartPoint Sales Demo` |

## Architecture and flow contract

1. The agent uses the existing Dataverse Account lookup to resolve the customer.
2. The agent collects the required Opportunity values. If the expected close date is missing, it asks for it before presenting the complete proposed values.
3. The agent waits for explicit user confirmation before calling the flow.
4. The existing tool passes four inputs: `opportunityName`, `accountId`, `estimatedRevenue`, and `estimatedCloseDate`.
5. The flow creates a row in the custom Sales Opportunity table and responds to the agent with the created-record result. A clickable record link was observed in the successful 2026-10-16 test and opened the row in the complete-input test.

The report confirms the four input names and successful result, but does not specify the current response property's exact name or URL expression. Do not assume that the current output is named `opportunityUrl` without inspecting the live flow. In an earlier test version, the `opportunityId` response contained an OData record URL, from which the agent displayed the GUID. The 2026-10-16 technical report is the evidence for the later clickable-link behavior.

## Dataverse Account lookup binding

The visible tool input is `accountId` and carries the Account GUID. Dataverse's create-row Account lookup must receive an OData entity reference, not a bare GUID. In the earlier verified flow trigger, the actual internal property for the visible `accountId` input was `text_1`; this expression built the reference:

```text
concat('accounts(', triggerBody()?['text_1'], ')')
```

This documents the observed flow version and troubleshooting resolution. The exact current trigger property should not be guessed if the flow is later changed; the historical error was diagnosed from run-history trigger outputs and Dataverse action inputs.

## Confirmation and response behavior

The agent instructions require explicit confirmation before the write. The demo disables the tool-level **Ask end user before running** option to avoid a duplicate confirmation; the agent-level confirmation remains. The 2026-10-16 missing-date test showed the agent asking for the date, then summarizing all four values and waiting for an explicit confirmation before invoking the flow. The agent returned one success message with a clickable link.

This is the tested proof-of-concept configuration. For production write operations, enforce one clear approval step at the tool boundary and avoid asking the user twice. No dedicated Copilot Studio Topic was introduced; the existing instruction-based orchestration remains the selected PoC design. The verified pre-write summary is sufficient; no additional post-creation summary is needed.

## Verification

### Earlier test — 2026-10-15

- Manual Power Automate run completed successfully at 07:35; agent-triggered run completed at 07:43.
- One confirmation preceded creation of `AX-300 CNC Machine Final Test` for `Huber Logistik`.
- Account ID: `177a9152-afc7-f111-a05a-000d3a0a2928`.
- Created Opportunity ID: `c2ade295-5ec8-f111-a05a-002248d597bf`.
- The record was independently found and opened in `SmartPoint Sales Demo`; its ID matched the agent-returned ID.

### Updated tests — 2026-10-16

**Complete-input test:** `AX-300 CNC Machine Demo` for `Huber Logistik`, estimated revenue `35000`, expected close date `2026-12-15`. The agent requested confirmation, the flow created the row, and the returned link opened it in `SmartPoint Sales Demo`. The report does not record this test's row ID. The user request specified EUR, but an earlier Power Apps record view displayed `$35,000.00`; the stored/displayed currency was not independently verified as EUR.

**Missing-date test:** The request named `AX-300 CNC Machine Demo Missing Date` for `Huber Logistik` with estimated revenue `35000` and omitted the close date. The agent explicitly asked for the missing date; after the user supplied `2026-12-15`, the agent presented all four values, waited for explicit confirmation, and then the flow completed. The agent returned one clickable success link. The report does not record this run's row ID or an independent read-back of this specific row.

### T5 acceptance boundary

The original assignment's exact scenario is: “Create an Opportunity: Huber Logistik wants three AX-200, approximately EUR 450,000.” The tests above used AX-300 and `35000`, not three AX-200 and approximately EUR 450,000. Therefore, the required-field clarification, confirmation, create, and link behavior is verified with the recorded variants, but the exact original T5 business-data scenario and EUR handling are not. A separate negative test that directly invokes the flow with missing required inputs was also not documented; the flow-side condition remains a defensive layer rather than a separately verified test.

### Independently verified record URL from 2026-10-15

- App ID: `37590ede-88c7-f111-a05a-000d3a0a2928`
- Table logical name: `cr233_salesopportunity`
- Record ID: `c2ade295-5ec8-f111-a05a-002248d597bf`
- Verified URL: [Open the previously verified Opportunity](https://org52f65de8.crm3.dynamics.com/main.aspx?appid=37590ede-88c7-f111-a05a-000d3a0a2928&pagetype=entityrecord&etn=cr233_salesopportunity&id=c2ade295-5ec8-f111-a05a-002248d597bf)

The URL structure and its `appid`, `pagetype`, `etn`, and `id` parameters were verified for that record. It does not reveal the exact expression or response-property name used by the later flow link.

## Troubleshooting and lessons learned

- **Missing close date:** The initial negative test showed “planned close: not specified” and still offered to create the record. That contradicted the requirement. Agent instructions were tightened so all four values must be present before confirmation; the 2026-10-16 retest asked for the date first. This demonstrates the tested conversation, not every possible missing-field combination.
- **Lookup reference:** An initial `ODataUnrecognizedPathException` followed from passing a raw GUID directly into the Dataverse lookup. A later empty `accounts()` reference came from reading `triggerBody()?['accountId']` when the observed internal key was `text_1`. Run history revealed the actual trigger payload. The corrected mapping uses the GUID input to build the required OData account reference.
- **Flow validation:** A numeric input caused a runtime error when checked with `empty()`. The validation was corrected. The flow-side condition remains defensive; a direct test of the flow with missing inputs was not recorded.
- **Flow response actions:** Conflicting/multiple response-action issues and response outputs were corrected. No exact current output schema or expression is documented beyond the observed returned clickable link.
- **Separate issue:** The unrelated `Create Customer` topic error `Input binding 'item.name' was not found.` is not part of this Opportunity flow and must not be retried as an Opportunity fix.

## Current demo guidance and remaining project work

Use Copilot Studio test chat for the verified flow demo. The agent is published and installed in Teams, but Teams requests fail with a usage-limit error; licensing/capacity is suspected, not confirmed. `CRM738378` has 1,000 allocated legacy sessions, but no Copilot Credits or billing plan were found. SmartPoint guidance is pending. Do not enable paid billing without explicit approval.

US-03's current agent instructions, flow, and tool are the verified PoC architecture; do not add a dedicated Topic or change the working link without a requirement. The exact original T5 sample, currency persistence, and the separate flow-side negative test remain limitations. The next mandatory project action is US-05 / T7 regional-permission verification. The US-04 / T6 bonus plan is scheduled after T7; when it is pursued, first verify Approvals and Teams action availability, licenses, permissions, approver/requester identities, and the required Opportunity outcome-persistence target. See the [authoritative case plan](PROJECT_PLAN.md) and [chat handoff](../../docs/CHAT_HANDOFF.md).
