# Copilot Studio Account Creator

Status: **CONFIGURED, but NOT RUNTIME VERIFIED**. Implementation/configuration is complete; runtime verification is blocked by external licensing/capacity, with clarification pending. [PROJECT_PLAN.md](../PROJECT_PLAN.md) remains the authoritative status source. The following records the user-provided verified configuration and administrative findings.

## Agent Configuration

- Environment: `CRM816895`.
- Agent: `Account Creator`; model shown in Copilot Studio: `GPT-5 Chat`.
- Web Search is enabled for all websites.
- Instructions make Microsoft Dataverse Account creation the primary task. The agent must determine the Account name from the request and ask for it if missing or ambiguous. Missing business information must not be invented. Successful creation should be confirmed to the user.

## Dataverse Tool

The tool is successfully attached to the agent.

| Setting | Configured value |
| --- | --- |
| Tool name | Create Dataverse Account |
| Microsoft Dataverse action | Add a new row to the selected environment |
| Authentication | User |
| Environment input | Fixed custom value: `CRM816895` |
| Table input | Fixed custom value: `Accounts` |
| Account Name | Dynamically filled by AI from the user's request |
| Other Account fields | Not artificially populated |

## Attempted Runtime Verification

Test request: `Create an account called SmartPoint Copilot Test Account.`

Copilot Studio stopped the preview **before tool execution** with `EnforcementUsageCredits`, indicating that the environment had no Copilot Studio credits available. The Dataverse action was not runtime-verified, and no successful Account creation is claimed. This is an external licensing/capacity blocker, not a demonstrated implementation failure.

## Capacity Findings and Pending Clarification

- Administrative verification showed 0 current Copilot Credits for the tenant. `CRM816895` had no allocated Copilot Studio capacity and no pay-as-you-go configuration.
- A one-month Microsoft Copilot Studio Trial was available in the Microsoft 365 Admin Center Marketplace. Activation was deliberately not completed because the organization/billing profile had a fixed country/region and required additional organization/address information. No billing profile or tenant organization data was changed.
- The environment owner/contact has been asked whether to enable capacity or accept the documented configured state for the case. External clarification is pending.

If capacity is enabled, repeat the preview test and verify the created Account in Dataverse before recording runtime success. Until then, retain the status CONFIGURED, but NOT RUNTIME VERIFIED.
