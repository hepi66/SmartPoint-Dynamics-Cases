# 1. Room Planning

## 1. Original Requirement

Support multiple office locations containing rooms with different capacities, whole-day employee bookings, and a daily booking/occupancy overview. Offices are conceptually occupied up to 50%; custom overbooking validation or programming is not required.

## 2. Case in One Sentence

Dataverse tables and the Room Planning model-driven app support whole-day employee room bookings and a daily overview with an informational 50% planning limit.

## 3. What We Built

Location, Room and Booking Dataverse tables and the Room Planning model-driven app. Planning Capacity expresses the intended 50% limit as information.

## 4. Where to Find It

- **Daily booking overview:** Power Apps → environment `CRM816895` → app `Room Planning` → Bookings → view `Today's Bookings`.
- **Locations and rooms:** Room Planning → Locations or Rooms; known records are `Graz` and `Graz Meeting Room 01`.
- **Data model:** In environment `CRM816895`, locate unmanaged solution `Achim Beispiel` and tables Location, Room and Booking. Exact solution-editor click path: PENDING verification.
- **Repository reference:** [PROJECT_PLAN.md — model and evidence](../../PROJECT_PLAN.md).
- **Pending navigation details:** direct app URL, custom table/column logical names and exact form/view editor paths are not recorded.

## 5. How It Works

Location has primary column Location Name. Room has Room Name, a required Location lookup, and required whole-number Maximum Capacity and Planning Capacity (minimum 1). Booking has Booking Name, required Date Only Booking Date, and required Room and Employee lookups. Relationships are Location 1:N Room, Room 1:N Booking, and User 1:N Booking. Employee references the existing managed `systemuser` table; that table was not created by this project.

## 6. Result and Verification

COMPLETED to the extent required for the SmartPoint case. App navigation and basic booking creation were functionally tested. Today's Bookings included a current-day booking and excluded a following-day booking. The test room has Maximum Capacity 10 and Planning Capacity 5; a sixth same-day booking was successfully created. Multiple-location operation is supported by the documented model but not separately evidenced by the test data.

## 7. Offline Evidence

Evidence status: PENDING

Recommended screenshot:
The Room Planning app showing Today's Bookings with the booked day, room and employee visible. Use dedicated demonstration data and avoid unnecessary personal information.

Intended storage: `docs/case-guide/evidence/`. No image exists or is linked yet. Follow the [evidence instructions](evidence/README.md). Repository text can be shown offline now; they do not replace captured runtime evidence.

## 8. Three Real-World Use Cases

Explanatory examples only; these are not additional implemented SmartPoint functionality.

1. Daily desk reservations across office sites.
2. Shared meeting-room booking for internal teams.
3. Training-room planning using room capacity information.

## 9. What I Should Be Able to Explain

- [ ] Explain tables, required lookups and one-to-many relationships.
- [ ] Explain Date Only and whole-day booking.
- [ ] Distinguish informational planning capacity from enforced validation.
- [ ] Explain why Employee uses the existing User table.

## 10. Troubleshooting and Important Notes

Planning Capacity is informational only. The sixth booking test confirms that exceeding it is allowed deliberately within scope. A future demo needs current-day data; historical bookings will not prove today's filter. Exact view filters and form layouts are not captured in the repository.

[Back to Case Guide](README.md)
