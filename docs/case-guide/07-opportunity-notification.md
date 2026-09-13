# 7. Optional Opportunity Notification

## 1. Objective

Optional: configure a Power Automate flow that sends a simple email to the record owner when a new standard Dynamics 365 / Dataverse Opportunity is created. Include the Opportunity title and date.

## 2. Intended Design

The following is an intended design only; no flow was implemented or runtime-tested:

- Microsoft Dataverse trigger for a newly created Opportunity, with organization scope.
- Use the Opportunity owner as the email recipient.
- Include the Opportunity title and date in the email.
- Use Office 365 Outlook — `Send an email (V2)`, or the equivalent configured mail action.

Exact field mappings and owner resolution were not configured or verified.

## 3. Environment Verification

In environment `CRM816895`:

- The standard Opportunity table could not be selected in the Microsoft Dataverse Power Automate trigger. Searches for `Opportunity` and `Verkaufschance` returned no table.
- The Dataverse table list was also checked outside the solution; Opportunity was not available.
- The unmanaged solution `Achim Beispiel` contains existing tables such as Account, Booking, Location, Room and User, but no Opportunity table.
- Power Platform Admin Center exposes optional Dynamics 365 Sales-related packages, including `Dynamics 365 Sales Demo Hub`. No additional Sales or demo package was installed, and that package was not verified as the intended prerequisite.

These checks establish the environment limitation; they are not a flow runtime test.

## 4. Decision

The optional Opportunity Notification flow was not implemented because the standard Opportunity table is not available in the provided CRM816895 environment. No custom replacement table or additional Dynamics 365 Sales/demo package was installed solely for this optional exercise.

A custom substitute would not faithfully implement the standard sales Opportunity concept. Installing an unverified package would alter the provided environment unnecessarily for an optional exercise.

## 5. Result / Status

**OPTIONAL — EVALUATED; INTENTIONALLY NOT IMPLEMENTED due to the verified environment limitation.**

This is a deliberate scope decision, not a failed implementation. No Opportunity flow, email delivery or runtime result is claimed. [PROJECT_PLAN.md](../../PROJECT_PLAN.md) records this decision; Cases 01–06 retain their existing statuses.

## 6. What I Should Be Able to Explain

- Distinguish the intended event-to-email design from implemented functionality.
- Explain why the standard Opportunity table was required and how its absence was checked.
- Explain why neither a custom replacement nor an unverified Sales/demo package was introduced.

Suggested interview/demo explanation:

> I intended to use the standard Opportunity table rather than creating a custom substitute. Because that table is not provisioned in the provided environment, I chose not to alter the environment by installing an unverified Sales/demo package solely for an optional exercise.

[Back to Case Guide](README.md)
