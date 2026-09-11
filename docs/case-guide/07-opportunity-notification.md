# 7. Optional Opportunity Notification

## 1. Original Requirement

Optional: when a new Opportunity is created, send a simple email to the record owner including the Opportunity title and date.

## 2. Case in One Sentence

The optional Power Automate requirement is to email an Opportunity owner when a record is created, but no implementation exists yet.

## 3. What We Built

OPTIONAL — NOT IMPLEMENTED. No flow configuration or runtime result is documented.

## 4. Where to Find It

- **Requirement only:** Repository → [PROJECT_PLAN.md](../../PROJECT_PLAN.md) → work package 7.
- **Implementation location:** NOT AVAILABLE — no flow name, direct link, app, connector choice or implementation artifact is recorded.
- **Future navigation:** PENDING implementation and verification; no UI path is claimed.
- **Project context only:** environment `CRM816895`, target unmanaged solution `Achim Beispiel`; these do not establish that the optional flow exists.

## 5. How It Works

Only the required event-to-email behavior is known: new Opportunity, then owner notification containing title and date. Specific trigger settings, owner resolution and email action have not been designed or implemented in this project.

## 6. Result and Verification

OPTIONAL — NOT IMPLEMENTED. Nothing is functionally verified. After any future implementation, evidence would need to establish that a new Opportunity triggers the flow and its owner receives the email with the correct title and date.

## 7. Offline Evidence

Evidence status: PENDING

Recommended screenshot:
Only after implementation: the received owner notification showing the actual Opportunity title and date. No result screenshot is currently available.

Use dedicated demonstration data and avoid unnecessary personal information.

Intended storage: `docs/case-guide/evidence/`. No image exists or is linked yet. Follow the [evidence instructions](evidence/README.md). Repository text can be shown offline now; they do not replace captured runtime evidence.

## 8. Three Real-World Use Cases

Explanatory examples only; these are not additional implemented SmartPoint functionality.

1. Alert a salesperson to a newly assigned sales lead.
2. Notify a manager when a new deal enters the pipeline.
3. Send an owner a reminder that a new business record needs follow-up.

## 9. What I Should Be Able to Explain

- [ ] Explain the known requirement without presenting a design as implemented.
- [ ] Explain the difference between record creation and email delivery evidence.
- [ ] Identify the record owner as the required recipient.

## 10. Troubleshooting and Important Notes

This component is optional and remains NOT IMPLEMENTED (NOT STARTED in the authoritative roadmap). No implementation work or speculative connector configuration is part of this consolidation phase.

[Back to Case Guide](README.md)
