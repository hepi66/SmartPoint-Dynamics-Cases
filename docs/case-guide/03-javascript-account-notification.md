# 3. JavaScript Account Form Notification

## 1. Original Requirement

Add an Account form OnLoad notification with the required wording: “Die aktuelle Firmennummer der Firma <Firmenname> lautet <Firmennummer>.”

## 2. Case in One Sentence

An Account form OnLoad JavaScript handler displays the Account name and company number in an INFO notification.

## 3. What We Built

The Account Form Notification web resource, registered on the Account main form OnLoad event, and a published Account Management app used for runtime testing.

## 4. Where to Find It

- **Runtime form:** Power Apps → environment `CRM816895` → app `Account Management` → locate `SmartPoint Test Account`. Exact intervening navigation item: PENDING verification.
- **Web resource:** environment `CRM816895`, solution `Achim Beispiel`, resource `Account Form Notification`; exact editor click path and technical resource name: PENDING verification.
- **Registration:** Account main form → OnLoad handler `showCompanyNumberNotification`; exact form identifier/editor path: PENDING verification.
- **Local source:** Repository → `src/javascript/` → [AccountFormNotification.js](../../src/javascript/AccountFormNotification.js).
- **Verification reference:** [PROJECT_PLAN.md](../../PROJECT_PLAN.md).

## 5. How It Works

The handler accepts executionContext, obtains formContext, and reads `name` and `cr0c9_firmennummer`. It calls setFormNotification with level INFO and ID `companyNumberNotification`. Missing attributes, null or blank values clear that notification instead of showing an incomplete sentence; zero remains a supplied value.

## 6. Result and Verification

COMPLETED and VERIFIED. Runtime displayed “Die aktuelle Firmennummer der Firma SmartPoint Test Account lautet 4711.” in Account Management. Missing-value handling has local mocked checks, not a documented Power Apps runtime test.

## 7. Offline Evidence

Evidence status: PENDING

Recommended screenshot:
The Account form showing SmartPoint Test Account, Firmennummer and the complete INFO notification together. Capture the actual current value, not an invented historical result.

Use dedicated demonstration data and avoid unnecessary personal information.

Intended storage: `docs/case-guide/evidence/`. No image exists or is linked yet. Follow the [evidence instructions](evidence/README.md). Repository text and source code can be shown offline now; they do not replace captured runtime evidence.

## 8. Three Real-World Use Cases

Explanatory examples only; these are not additional implemented SmartPoint functionality.

1. Show a customer reference number to service staff.
2. Display a contextual account-handling reminder on form load.
3. Highlight a missing business identifier to a data steward.

## 9. What I Should Be Able to Explain

- [ ] Explain executionContext versus formContext.
- [ ] Explain display names versus logical names.
- [ ] Explain OnLoad registration and passing execution context.
- [ ] Explain notification level, ID and missing-value handling.

## 10. Troubleshooting and Important Notes

The function requires execution context and access to the two form attributes. Exact technical web-resource name and form identifier are not recorded. The console and timer overwrite the same company-number field: 4711 is historical test evidence, not a guaranteed current value.

[Back to Case Guide](README.md)
