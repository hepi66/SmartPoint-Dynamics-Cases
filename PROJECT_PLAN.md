# SmartPoint Dynamics Case — Project Plan

This file is the authoritative source for the project roadmap, progress, completed work, and next steps. Update it as work is performed; record completed work only when supported by actual results.

## Project Context

- Project: SmartPoint Dynamics Case
- Power Platform environment: `CRM816895`
- Target unmanaged solution: `Achim Beispiel`
- Local project root: `D:\Projekte\SmartPoint-Dynamics-Cases`
- This repository is completely separate from `Dynamics365-Learning-Lab`.
- Repository content, documentation, file names, and technical terminology are in English. Required platform names and the exact requested German notification text are preserved.

## Current Focus / Next Step

Booking confirmation Power Automate flow

## Roadmap and Implementation Status

| Priority | Implementation item | Status |
| --- | --- | --- |
| 1 | Room-planning Dataverse model and model-driven app | COMPLETED to the extent required for the SmartPoint case |
| 2 | Booking confirmation Power Automate flow | NOT STARTED |
| 3 | JavaScript Account form notification | NOT STARTED |
| 4 | C# Dataverse console application | NOT STARTED |
| 5 | Timer-triggered Azure Function | NOT STARTED |
| 6 | Copilot Studio Account Creator | NOT STARTED |
| 7 | Optional Opportunity notification flow | NOT STARTED |

## Requirements

### 1. Room-planning Dataverse Model and Model-driven App

Status: COMPLETED to the extent required for the SmartPoint case. The Dataverse foundation, Room Planning model-driven app, representative test data, Today's Bookings view, app navigation, and basic booking creation workflow are verified. Planning Capacity is informational only, as documented in the deliberate scope decision below.

- Support multiple locations, each containing rooms.
- Rooms have different maximum capacities.
- Employees create daily room bookings; bookings cover whole days only.
- Provide a daily booking overview so employees can see the current occupancy situation for the day.
- Offices are conceptually occupied only up to 50%.
- No custom overbooking validation or custom programming is required.
- Design the data model, tables, fields, forms, and views appropriately with a minimal implementation.

### 2. Booking Confirmation Power Automate Flow

Status: NOT STARTED

- Trigger when a booking is created.
- Send the employee a confirmation email for the booked day.

### 3. JavaScript Account Form Notification

Status: NOT STARTED

- Add a form notification to the Account form on load.
- Display the required text, substituting the Account name and company number for the placeholders:

  "Die aktuelle Firmennummer der Firma **Firmenname** lautet **Firmennummer**."

### 4. C# Dataverse Console Application

Status: NOT STARTED

- Ask the user for a starting number.
- Read all Accounts from Dataverse using the .NET SDK.
- Sort Accounts alphabetically.
- Assign sequential company numbers beginning with the supplied number.
- Store the value in the Dataverse field `Firmennummer`.

### 5. Timer-triggered Azure Function

Status: NOT STARTED

- Use a timer trigger.
- Read all Accounts from Dataverse and sort them alphabetically.
- Assign sequential company numbers beginning with 1.
- Store the values in `Firmennummer` and overwrite existing numbers on every run.
- Local execution is sufficient.
- Use OAuth/application-user authentication.

### 6. Copilot Studio Account Creator

Status: NOT STARTED

- Agent name: `Account Creator`.
- Allow users to create a new Account in Dataverse.
- Configure the required Dataverse tools.
- Enable Web Search where useful for enrichment.

### 7. Optional Opportunity Notification Flow

Status: NOT STARTED

- Trigger when a new Opportunity is created.
- Send a simple email to the record owner.
- Include the Opportunity title and date.

## Working Principles

- Prioritize demonstrable working results.
- Keep implementation minimal and explainable.
- Avoid unnecessary setup work.
- Keep this repository completely separate from `Dynamics365-Learning-Lab`.
- Do not invent completed work.
- Maintain roadmap, progress, completed work, and next steps in this file.

## Completed Work

- Created the initial repository documentation and minimal `.gitignore` baseline.
- Dataverse room-planning foundation: COMPLETED manually in environment `CRM816895`, unmanaged solution `Achim Beispiel`, based on the user-provided verified implementation state.
- Room Planning Dataverse/model-driven app foundation: COMPLETED to the extent required for the SmartPoint case, based on the user-provided functional verification recorded below. Roadmap priorities 2–7 remain NOT STARTED.

### Completed Dataverse Foundation

| Table | Primary column | Implemented fields and purpose |
| --- | --- | --- |
| Location | Location Name | Represents an office location containing bookable rooms. |
| Room | Room Name | Location: required lookup to Location. Maximum Capacity: required whole number, minimum 1. Planning Capacity: required whole number, minimum 1. |
| Booking | Booking Name | Booking Date: required Date Only field. Room: required lookup to Room. Employee: required lookup to the existing Dataverse User (`systemuser`) table. |

- Implemented relationships: Location 1:N Room, Room 1:N Booking, and User 1:N Booking.
- Planning Capacity represents the intended 50% occupancy planning limit; it is informational and is not enforced by Dataverse.
- User (`systemuser`) is an existing managed Dataverse system table, not a table created by this project. It appears in the solution because the Employee lookup introduces that dependency.
- No custom overbooking validation has been implemented; it is not required for the case.

### Verified Room Planning App and Test Data

- The model-driven app is named `Room Planning` and contains Locations, Rooms, and Bookings.
- Location test data exists for `Graz`.
- Room test data exists for `Graz Meeting Room 01`, with Maximum Capacity = 10 and Planning Capacity = 5.
- Booking records contain Booking Name, Booking Date, Room, and Employee. Employee uses a Dataverse User lookup.
- The `Today's Bookings` view exists and was functionally verified: a booking for the current day is shown, while a booking for the following day is excluded.
- The model-driven app navigation and basic booking creation workflow were functionally tested.

### Known Limitation / Deliberate Scope Decision

Planning Capacity is currently informational only. Dataverse does not prevent bookings beyond this capacity. This behavior was explicitly tested by successfully creating a sixth same-day booking for a room with Planning Capacity = 5. Custom overbooking validation is intentionally not implemented because it is not required by the SmartPoint case scope.

This is a conscious scope decision, not an unnoticed defect. Completion of the Room Planning foundation does not imply enforcement of the planning limit.

## Next Steps

1. Booking confirmation Power Automate flow — NOT STARTED.

Update this plan as implementation and verification actually occur, following the priority order above.
