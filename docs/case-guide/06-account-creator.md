# 6. Copilot Studio Account Creator

## 1. Original Requirement

Create an agent named Account Creator that allows users to create an Account in Dataverse, configure the required tools, and enable Web Search where useful for enrichment.

## 2. Case in One Sentence

The Copilot Studio Account Creator is configured to create Dataverse Accounts from user requests, but runtime verification is blocked by missing credits.

## 3. What We Built

Account Creator is configured with model GPT-5 Chat, Web Search for all websites, and the attached Create Dataverse Account tool. This describes configuration, not a successful Account-creation run.

## 4. Where to Find It

- **Agent:** Copilot Studio → environment `CRM816895` → locate agent `Account Creator`.
- **Configuration objects:** inspect the agent instructions, Web Search and attached tool `Create Dataverse Account`. Exact intervening UI tabs/click paths and direct agent URL: PENDING verification.
- **Repository reference:** [docs/AccountCreator.md](../AccountCreator.md) contains configuration and capacity findings.
- **Solution context:** `Achim Beispiel` is the project target; agent solution membership is not recorded.
- **Runtime:** preview is blocked before tool execution; no created Account can be located as a verified result.

## 5. How It Works

Instructions make Account creation the primary task, infer the name from the request, ask if missing or ambiguous, avoid inventing missing business information and confirm successful creation. The Dataverse add-row tool uses User authentication, fixed environment CRM816895 and table Accounts, with Account Name dynamically supplied by AI. Other fields are not artificially populated.

## 6. Result and Verification

CONFIGURED, but NOT RUNTIME VERIFIED. Preview request `Create an account called SmartPoint Copilot Test Account.` stopped before Dataverse tool execution with `EnforcementUsageCredits`. No successful Account creation, clarification dialogue or tool execution is claimed. External clarification regarding Copilot Studio capacity is pending.

## 7. Offline Evidence

Evidence status: PENDING

Recommended screenshot:
The Account Creator configuration showing the attached Dataverse tool and the preview EnforcementUsageCredits blocker. The screenshot must clearly evidence a configured but blocked state, not a successful creation.

Use dedicated demonstration data and avoid unnecessary personal information.

Intended storage: `docs/case-guide/evidence/`. No image exists or is linked yet. Follow the [evidence instructions](evidence/README.md). Repository text can be shown offline now; they do not replace captured runtime evidence.

## 8. Three Real-World Use Cases

Explanatory examples only; these are not additional implemented SmartPoint functionality.

1. Let service staff create customer records conversationally.
2. Guide sales staff through collecting an account name.
3. Support intake assistants that ask for missing business information before saving.

## 9. What I Should Be Able to Explain

- [ ] Distinguish agent instructions from a tool that changes Dataverse.
- [ ] Explain fixed inputs versus AI-filled Account Name.
- [ ] Explain User authentication and avoiding invented data.
- [ ] Distinguish licensing blockage from an implementation error or a successful test.

## 10. Troubleshooting and Important Notes

The tenant had 0 current Copilot Credits; the environment had no allocated capacity or pay-as-you-go setup. A trial was available but deliberately not activated because additional organization/billing information was required; no such data was changed. Await the owner/contact decision. If capacity is resolved, completing runtime verification takes immediate priority.

[Back to Case Guide](README.md)
