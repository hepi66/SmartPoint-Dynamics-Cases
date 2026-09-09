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

Design the minimal Dataverse data model for the room-planning application before creating any tables.

## Roadmap and Implementation Status

| Priority | Implementation item | Status |
| --- | --- | --- |
| 1 | Room-planning Dataverse model and model-driven app | NOT STARTED |
| 2 | Booking confirmation Power Automate flow | NOT STARTED |
| 3 | JavaScript Account form notification | NOT STARTED |
| 4 | C# Dataverse console application | NOT STARTED |
| 5 | Timer-triggered Azure Function | NOT STARTED |
| 6 | Copilot Studio Account Creator | NOT STARTED |
| 7 | Optional Opportunity notification flow | NOT STARTED |

## Requirements

### 1. Room-planning Dataverse Model and Model-driven App

Status: NOT STARTED

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
- No implementation work has been completed. All implementation items remain NOT STARTED.

## Next Steps

1. Design the minimal Dataverse data model for the room-planning application before creating any tables.
2. Record the proposed tables, relationships, fields, forms, views, and approach to the conceptual 50% occupancy requirement in `docs/`.
3. Update this plan as implementation and verification actually occur, following the priority order above.
