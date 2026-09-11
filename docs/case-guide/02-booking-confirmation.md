# 2. Booking Confirmation

## 1. Original Requirement

When a booking is created, send the employee a confirmation email for the booked day.

## 2. Case in One Sentence

Power Automate turns a new Dataverse Booking into an Outlook confirmation email containing the booked date and room.

## 3. What We Built

The Booking Confirmation Power Automate flow resolves the employee and room, then sends a confirmation through Office 365 Outlook.

## 4. Where to Find It

- **Flow:** Power Automate → environment `CRM816895` → locate flow `Booking Confirmation`. Exact intervening UI menu path and direct flow URL: PENDING verification.
- **Triggering record:** Power Apps → `CRM816895` → app `Room Planning` → Bookings.
- **Result checks:** inspect the flow's run history and the recipient's Microsoft 365 mailbox; exact run-history click path: PENDING verification.
- **Repository reference:** [PROJECT_PLAN.md — flow configuration and troubleshooting](../../PROJECT_PLAN.md).
- **Solution context:** `Achim Beispiel` is the project target; flow solution membership is not separately recorded.

## 5. How It Works

The Microsoft Dataverse trigger “When a row is added, modified or deleted” reacts to Booking creation. One Get a row by ID resolves the Employee/User lookup and Primary Email. A second resolves the Room lookup and Room Name. Booking Date comes from the trigger record. Office 365 Outlook Send an email (V2) sends subject `Room booking confirmation` and a body containing confirmation text, Booking Date and Room Name.

## 6. Result and Verification

COMPLETED and VERIFIED. Booking `Flow Test` for 2026-09-09, room `Graz Meeting Room 01`, and employee Achim Hepberger triggered all four stages successfully. The email was actually received with the correct date and room. No separate error-path testing is documented.

## 7. Offline Evidence

Evidence status: PENDING

Recommended screenshot:
The received confirmation email showing its subject, booking date and room name.

Use dedicated demonstration data and avoid unnecessary personal information.

Intended storage: `docs/case-guide/evidence/`. No image exists or is linked yet. Follow the [evidence instructions](evidence/README.md). Repository text can be shown offline now; they do not replace captured runtime evidence.

## 8. Three Real-World Use Cases

Explanatory examples only; these are not additional implemented SmartPoint functionality.

1. Send a service-request receipt after a record is created.
2. Confirm an equipment reservation to its requester.
3. Notify an employee that a training registration was saved.

## 9. What I Should Be Able to Explain

- [ ] Explain event-triggered automation.
- [ ] Explain why lookup IDs must be resolved to readable values.
- [ ] Trace recipient, date and room from Dataverse to the email.
- [ ] Distinguish a successful run from confirmed email delivery.

## 10. Troubleshooting and Important Notes

Initial environment/connectivity/DNS-related XRM errors prevented Change Type loading; recreating the flow did not help and Connected connections did not establish metadata connectivity. No more specific root cause was verified. A later, separate generic Mail connector restriction affected Send an email notification (V3); replacing it with Office 365 Outlook V2 resolved delivery. Exact trigger scope and dynamic expressions are not captured.

[Back to Case Guide](README.md)
