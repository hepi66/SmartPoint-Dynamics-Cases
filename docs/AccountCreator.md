# Copilot Studio Account Creator

Status: **COMPLETED and end-to-end VERIFIED** through the successful Copilot Studio test on 2026-09-12. [PROJECT_PLAN.md](../PROJECT_PLAN.md) remains authoritative.

## Final Verified Implementation

- Agent: `Account Creator Standard Test`; environment: `CRM816895`; model during test: `GPT-5.5 Chat`.
- Enabled Microsoft Dataverse add-row tool: fixed environment `CRM816895`, fixed table Accounts, Account Name dynamically filled by AI.
- Request: `Create an account called SmartPoint Copilot Final Test.`
- Creation confirmation: Account ID `fc0e3ee7-9cae-f111-aaac-7c1e5206403d`, Active, created `2026-09-12 11:27 UTC`, owner Achim Hepberger.
- Account Management subsequently showed `SmartPoint Copilot Final Test`, verifying persistence. Publishing, ambiguous-name handling and web enrichment are not established by this test.
- Final instructions, navigation and both screenshots: [Case 06 guide](case-guide/06-account-creator.md).

The following sections preserve the earlier configuration and credit-blocked attempt as history. Their model, tool authentication and Web Search settings must not be assumed to describe the final standard agent.

## Earlier Agent Configuration

- Environment: `CRM816895`.
- Agent: `Account Creator`; model shown in Copilot Studio: `GPT-5 Chat`.
- Web Search is enabled for all websites.
- Instructions make Microsoft Dataverse Account creation the primary task. The agent must determine the Account name from the request and ask for it if missing or ambiguous. Missing business information must not be invented. Successful creation should be confirmed to the user.

## Earlier Dataverse Tool

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

## Earlier Attempted Runtime Verification

Test request: `Create an account called SmartPoint Copilot Test Account.`

Copilot Studio stopped the preview **before tool execution** with `EnforcementUsageCredits`, indicating that the environment had no Copilot Studio credits available. The Dataverse action was not runtime-verified, and no successful Account creation is claimed. This is an external licensing/capacity blocker, not a demonstrated implementation failure.

## Historical Capacity Findings

- Administrative verification showed 0 current Copilot Credits for the tenant. `CRM816895` had no allocated Copilot Studio capacity and no pay-as-you-go configuration.
- A one-month Microsoft Copilot Studio Trial was available in the Microsoft 365 Admin Center Marketplace. Activation was deliberately not completed because the organization/billing profile had a fixed country/region and required additional organization/address information. No billing profile or tenant organization data was changed.
- The environment owner/contact has been asked whether to enable capacity or accept the documented configured state for the case. Clarification was pending at that stage.

The final standard-agent test above supersedes the earlier blocked runtime status. No specific licensing change or internal cause of the transition is established.
