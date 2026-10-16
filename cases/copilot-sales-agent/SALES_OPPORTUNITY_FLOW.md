# SmartPoint Sales Assistant: Opportunity Creation Flow

## Status and components

The Copilot Studio → Power Automate → Microsoft Dataverse creation path was successfully exercised on 2026-10-15 in environment `CRM738378`. The created Opportunity was subsequently found and opened in the `SmartPoint Sales Demo` model-driven app. The agent-returned ID matches the independently opened record. The flow does not yet return a dynamically generated `opportunityUrl` for a clickable link in the agent response. This remains an open US-03 / M-05 acceptance requirement for T5, not an optional enhancement.

| Component | Configuration |
| --- | --- |
| Copilot Studio agent | `SmartPoint Sales Assistant` |
| Agent tool | `Create Sales Opportunity via Flow` |
| Cloud flow | `Create Sales Opportunity` |
| Dataverse table | `Sales Opportunity` (custom table) |
| Environment | `CRM738378` |

## Architecture and workflow

1. The agent uses the existing Account lookup capability to identify the requested customer.
2. It presents the proposed Opportunity and Account and obtains explicit user confirmation before writing.
3. On confirmation, the agent calls `Create Sales Opportunity via Flow`, passing `opportunityName` and `accountId` to `Create Sales Opportunity`.
4. The flow creates a row in the custom Dataverse `Sales Opportunity` table and returns `opportunityId` through **Respond to the agent**.
5. The flow output observed during testing was an OData record URL in `opportunityId`. The agent extracted and displayed the GUID. The separately verified model-driven-app record URL is documented below.

## Dataverse lookup binding

The flow's visible input is `accountId`, but its internal trigger property is `text_1`. Use the internal property name when building the Account lookup reference:

```text
concat('accounts(', triggerBody()?['text_1'], ')')
```

The Dataverse Account lookup expects an OData entity reference, such as `accounts(<Account GUID>)`, rather than a bare GUID. The Opportunity Name column uses the dynamic `opportunityName` input. The flow returns `opportunityId` using **Respond to the agent**.

## Troubleshooting notes

- **`ODataUnrecognizedPathException`:** The first attempt supplied the raw Account GUID to the lookup. Dataverse requires an OData reference using the `accounts(<GUID>)` path.
- **`accounts()` / empty reference:** A later expression read `triggerBody()?['accountId']`, so the resulting path was `accounts()` with no Account ID. That key was not present in the trigger body; the actual internal property was `text_1`.
- **Diagnosis:** Power Automate run history was used to inspect trigger outputs and the inputs passed to the Dataverse create-row action. Compare the displayed input name with the actual trigger property instead of assuming they match.
- **Lesson:** Power Automate display names and internal trigger property names can differ. Build expressions against the actual trigger output shape.

## Confirmation and response behavior

The agent instructions require explicit confirmation before creating an Opportunity. During the demo configuration, tool-level **Ask end user before running** was disabled because it added a second confirmation prompt; agent-level confirmation remained enabled. This is a demo choice, not the production security recommendation. For production write operations, prefer enforced tool-level confirmation with one clear approval step and no redundant conversational confirmation.

The tool completion behavior was changed from **Do not respond** to **Send specific response**, using an English success message. Tool outputs remain available to the agent, which can use the returned `opportunityId`.

## Verification on 2026-10-15

- Manual Power Automate run: successful at **07:35**.
- Agent-triggered flow run: successful at **07:43**.
- Final Copilot Studio test created the requested Opportunity through the tool and returned a success response:
  - Opportunity: `AX-300 CNC Machine Final Test`
  - Account: `Huber Logistik`
  - Account ID: `177a9152-afc7-f111-a05a-000d3a0a2928`
  - Created record ID: `c2ade295-5ec8-f111-a05a-002248d597bf`
- The final test showed one confirmation, successful tool completion, and a user-visible success message.
- The record was independently found and opened through the `SmartPoint Sales Demo` model-driven app. Its ID matches the agent's returned GUID.

### Independent record read-back and verified URL

- Model-driven app: `SmartPoint Sales Demo`
- App ID (`appid`): `37590ede-88c7-f111-a05a-000d3a0a2928`
- Table logical name (`etn`): `cr233_salesopportunity`
- Record ID (`id`): `c2ade295-5ec8-f111-a05a-002248d597bf`
- Record URL: [Open the verified Opportunity](https://org52f65de8.crm3.dynamics.com/main.aspx?appid=37590ede-88c7-f111-a05a-000d3a0a2928&pagetype=entityrecord&etn=cr233_salesopportunity&id=c2ade295-5ec8-f111-a05a-002248d597bf)

URL parameters:

- `appid` selects the `SmartPoint Sales Demo` model-driven app.
- `pagetype=entityrecord` opens a Dataverse row form.
- `etn` identifies the table by logical name, `cr233_salesopportunity`.
- `id` identifies the Opportunity row; it matches the GUID returned by the agent.

This verifies that the record exists and can be opened in the app. It does not mean the flow currently returns this app-specific URL to the agent.

## 3–5 minute demonstration outline (Copilot Studio test chat)

The agent is published and installed in Teams, but Teams requests currently fail with a usage-limit error. Use the working Copilot Studio test chat for this flow demonstration until the Teams retest succeeds; do not present Teams runtime as verified.

1. **0:00–0:45 — Set the context:** Show `SmartPoint Sales Assistant` and explain that the agent reads the existing Account data before a write.
2. **0:45–1:30 — Identify and confirm:** Use `Huber Logistik` as the customer, review the proposed Opportunity name and account, and show the single explicit confirmation. A suggested demo prompt is “Create the opportunity AX-300 CNC Machine Final Test for Huber Logistik”; this is presentation wording, not a quoted test transcript.
3. **1:30–2:30 — Show the integration:** Point out the `Create Sales Opportunity via Flow` tool and the `Create Sales Opportunity` flow. Explain the `opportunityName` and `accountId` inputs and the Account OData reference.
4. **2:30–3:30 — Explain the response and read-back:** Show the flow run and `Respond to the agent` output, then open the independently verified row in `SmartPoint Sales Demo`. Compare the returned GUID with the `id` in the record URL. Clarify that automatic clickable-link generation is not yet implemented.
5. **3:30–5:00 — Explain the debugging lesson:** Briefly contrast the raw GUID and wrong `accountId` property failures with the working `accounts(...)` expression using `text_1`.

## Technical discussion prompts

- **Why use a flow?** It provides a controlled write boundary between the agent and Dataverse and allows the agent to request confirmation before the write.
- **Why does the lookup need `accounts(<GUID>)`?** The Dataverse lookup input expects an OData entity reference, not a raw identifier.
- **Why use `text_1` instead of `accountId`?** `accountId` is the visible input label; the trigger body exposed the internal property `text_1`. The actual trigger output is authoritative for expressions.
- **How were the failures diagnosed?** Inspect run history, trigger outputs, and Dataverse action inputs, then compare the lookup payload with the expected OData path.
- **What is the confirmation trade-off?** The demo retains agent-level confirmation and disables the duplicate tool prompt. For production writes, enforce one approval step at the tool boundary without asking twice.

## Remaining verification

The 2026-10-15 test verified successful manual and agent-triggered flow runs, the returned identifier, and independent read-back by opening the matching record in `SmartPoint Sales Demo`. The agent has since been published and installed in Microsoft Teams, but Teams requests fail with a usage-limit error even though the Copilot Studio test chat works. The suspected licensing/capacity cause is not confirmed. `CRM738378` has 1,000 allocated legacy sessions; no Copilot Credits or billing plan were found. An inquiry was emailed to Dominic Bley on 2026-10-15, and his automatic reply states that he will be available again from 2026-10-18. The next action is to evaluate SmartPoint's response and retest in Teams. Do not enable paid billing without explicit approval. The full end-to-end Teams test remains pending. For US-03 / T5, the remaining acceptance work is to return a dynamically generated `opportunityUrl` and display it as a clickable link immediately after successful creation, then verify that it opens the newly created row. The original T5 scenario and required close-date clarification also remain unverified; see the case plan for the exact criteria and preparation sequence.
