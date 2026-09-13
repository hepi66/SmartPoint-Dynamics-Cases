# SmartPoint Dynamics Case --- Questions and Answers

> **Purpose:** Personal interview and demo preparation.\
> This document is deliberately separated from the formal case
> documentation and should **not** be included in the evidence PDF sent
> in advance.
>
> **Basis:** The questions come from the current "What I Should Be Able
> to Explain" sections of Case documentation 01--07. The answers reflect
> only the documented and verified project state.

------------------------------------------------------------------------

## Case 01 --- Room Planning

### 1. Why are Location, Room and Booking separate tables?

**Answer:**\
They represent three different business concepts. A Location represents
a site, a Room represents a specific room at that site, and a Booking
represents a reservation of that room for a specific day and employee.
Keeping them separate avoids unnecessary data duplication and allows the
relationships to be modeled cleanly with Dataverse lookups.

### 2. How do the Location → Room → Booking relationships work?

**Answer:**\
One Location can contain multiple Rooms, so Location to Room is a
one-to-many relationship. One Room can have many Bookings, so Room to
Booking is another one-to-many relationship. Each individual Booking
references one Room.

### 3. Why does Employee reference the existing User table?

**Answer:**\
Employees already exist in Dataverse as system users. Creating another
Employee table would duplicate existing data. The Booking therefore uses
a lookup to the existing User table and references the existing user
record directly.

### 4. Why is Booking Date a Date Only field?

**Answer:**\
The requirement is for whole-day bookings only. There is no need for a
start or end time. A Date Only column represents that business
requirement directly without introducing unnecessary time information.

### 5. Why are Maximum Capacity and Planning Capacity separate fields?

**Answer:**\
Maximum Capacity represents the physical maximum capacity of a room.
Planning Capacity represents the occupancy level intended for planning.
Keeping them separate preserves the distinction between a physical limit
and an organizational planning target.

### 6. How do Maximum Capacity 10 and Planning Capacity 5 represent the 50% requirement?

**Answer:**\
The demonstration room has a maximum capacity of 10 and a planning
capacity of 5. The planning value is therefore exactly 50% of the
physical maximum.

### 7. How does the Today's Bookings view work?

**Answer:**\
The view shows active Bookings whose Booking Date is the current day and
sorts them by Booking Date ascending. The test verified that a booking
for the current day appears while a booking for the following day does
not.

### 8. Why can a sixth booking be saved when Planning Capacity is 5?

**Answer:**\
Planning Capacity is deliberately informational in this solution. There
is no Dataverse rule or custom logic that blocks additional bookings.
Therefore a sixth booking was successfully saved during verification.

### 9. Why was no custom overbooking validation implemented?

**Answer:**\
The SmartPoint case does not require custom overbooking validation. The
50% concept had to be represented in the model, but custom programming
to block further bookings was explicitly outside the required scope. I
therefore avoided adding unnecessary complexity.

### 10. Where can I find the runtime app and the implementation?

**Answer:**\
The runtime app is `Room Planning` in Power Apps in environment
`CRM816895`. It contains Locations, Rooms, Bookings and the
`Today's Bookings` view. The implementation is in the Maker portal under
Solutions → `Achim Beispiel`, where I can inspect the Location, Room and
Booking tables and the `Room Planning` app.

------------------------------------------------------------------------

## Case 02 --- Booking Confirmation

### 1. What triggers Booking Confirmation and what are its four steps?

**Answer:**\
The flow starts when a new Booking row is created in Dataverse. It then
retrieves the related Employee/User, retrieves the related Room, and
finally sends the confirmation through Office 365 Outlook. Together with
the Dataverse trigger, these are the four visible flow steps.

### 2. Why is the related Employee/User retrieved?

**Answer:**\
The Booking contains a lookup to the Employee/User. The email needs
information from that related User row, especially the Primary Email
used as the recipient address. Therefore the flow resolves the
referenced User with `Get a row by ID`.

### 3. Why is the related Room retrieved?

**Answer:**\
The Booking also contains a Room lookup. The confirmation email should
contain a human-readable Room Name, so a second `Get a row by ID` action
retrieves the referenced Room.

### 4. Which values matter in the received email?

**Answer:**\
The recipient is the booked employee's email address. The message
contains the Booking Date and Room Name. In the verified test, the email
with subject `Room booking confirmation`, the correct date and
`Graz Meeting Room 01` was actually received.

### 5. What is Dataverse's role versus Outlook's role?

**Answer:**\
Dataverse is the data and event source: it stores the Booking and the
related User and Room data. Office 365 Outlook is responsible for
delivering the email. Power Automate connects those two parts.

### 6. Why do the successful run and received email establish end-to-end success?

**Answer:**\
A successful green flow run proves that the technical flow actions
completed. The received email additionally proves that the business
result reached the destination mailbox. Together they demonstrate the
complete chain from a new Dataverse Booking to the delivered email.

### 7. Where do I find the flow overview, run history and run details?

**Answer:**\
In Power Automate in environment `CRM816895`, I open
`Booking Confirmation`. The overview contains the 28-day run history and
`All runs`. Opening a specific run lets me inspect the trigger and each
action, including status, inputs and outputs.

### 8. How do I interpret successful, retrying and failed actions?

**Answer:**\
A green step means the action completed successfully. Retrying means
Power Automate is attempting the action again, often because of a
temporary problem. Failed means the action ultimately failed, so I
inspect that specific action's status and error details.

### 9. How do I explain the hostname troubleshooting without assuming a business-data error?

**Answer:**\
During a later retest, Dataverse reported `UnresolvableHostName` /
`HostNotFound` and referenced a stale hostname, while inputs such as the
table and record ID looked plausible. That pointed to a
connection/backend/discovery issue rather than immediate proof of bad
Booking or User data. A fresh flow copy with rebound Dataverse
connections subsequently worked end-to-end.

------------------------------------------------------------------------

## Case 03 --- JavaScript Account Form Notification

### 1. What triggers the JavaScript and where is OnLoad configured?

**Answer:**\
The function runs when the Account main form loads through the form's
OnLoad event. It is configured in the Maker portal on the Account main
form under Events → OnLoad, with library
`cr0c9_AccountFormNotification.js` and function
`showCompanyNumberNotification`.

### 2. What are executionContext and formContext, and why must the context be passed?

**Answer:**\
`executionContext` is supplied by the form event and represents the
current event context. The function calls `getFormContext()` on it to
obtain the specific `formContext`. That object provides access to the
attributes and UI of the currently opened Account form. Without the
passed execution context, the function could not reliably work with the
current form.

### 3. Where do Account Name and Firmennummer come from?

**Answer:**\
Both values come from attributes of the currently opened Account row in
Dataverse. The script reads `name` for Account Name and
`cr0c9_firmennummer` for Firmennummer.

### 4. Why does the code use `name` and `cr0c9_firmennummer` instead of display labels?

**Answer:**\
JavaScript accesses Dataverse attributes through their logical names.
Display labels can be translated or renamed and are not the technical
identity of the column. Therefore the code uses `cr0c9_firmennummer`,
not the visible label `Firmennummer`.

### 5. How is the sentence constructed and displayed?

**Answer:**\
The script reads the Account Name and Firmennummer, inserts both values
into the required German sentence and displays it with
`formContext.ui.setFormNotification()` at `INFO` level.

### 6. Why use a stable notification ID?

**Answer:**\
The stable ID `companyNumberNotification` lets the script specifically
replace or clear this notification later without affecting unrelated
form notifications.

### 7. What happens when an attribute or required value is missing?

**Answer:**\
The script does not display an incomplete message. It suppresses the
notification and clears any previous notification with the same ID.
Empty or whitespace-only values are treated as missing as well.

### 8. Where can I locate the web resource, source file and enabled OnLoad handler?

**Answer:**\
The web resource is in solution `Achim Beispiel` as
`Account Form Notification` / `cr0c9_AccountFormNotification.js`. The
handler is registered on the Account main form's OnLoad event. The
repository source is `src/javascript/AccountFormNotification.js`.

### 9. How do I demonstrate matching field and notification values?

**Answer:**\
I open `SmartPoint Test Account` in `Account Management`, show the
current Firmennummer and compare it with the form notification. In the
final documented state, the value is `1`, and the notification also
displays `1`.

### 10. What is the difference between Dataverse storage and client-side form behavior?

**Answer:**\
Dataverse permanently stores Account Name and Firmennummer. The
JavaScript does not modify those values in this case; it runs
client-side in the opened form and presents existing data as a UI
notification.

------------------------------------------------------------------------

## Case 04 --- C# Dataverse Console Application

### 1. Why does a console application suit this task?

**Answer:**\
The numbering is a manually controlled batch operation: the user enters
one starting number and the program processes all Accounts. A small
console application is simple, controlled and easy to explain; a
permanent UI or server process would add unnecessary complexity.

### 2. What are ServiceClient, QueryExpression, Entity and SDK Update?

**Answer:**\
`ServiceClient` establishes the authenticated Dataverse connection.
`QueryExpression` describes the Account query. An `Entity` object
represents a Dataverse row. The SDK `Update` operation then persists the
new Firmennummer on the relevant Account.

### 3. How do Entra credentials, the Dataverse Application User and its security role work together?

**Answer:**\
The console authenticates as an Entra application using Tenant ID,
Client ID and Client Secret. Dataverse contains an Application User
representing that application. Its security role determines which
Dataverse data the application can read or modify. System Administrator
was used for this practical case; it is not a production least-privilege
recommendation.

### 4. What are the four environment variables and why is the secret kept out of source?

**Answer:**\
The variables are `SMARTPOINT_DATAVERSE_URL`, `SMARTPOINT_TENANT_ID`,
`SMARTPOINT_CLIENT_ID` and `SMARTPOINT_CLIENT_SECRET`. This keeps
configuration, especially the secret, outside the repository so the
Client Secret is never committed into public source code.

### 5. What must be restored in a fresh terminal session?

**Answer:**\
The four required environment variables must be present in the session
before the console starts. The secret is supplied locally and securely
rather than loaded from the repository. The application can then connect
to the Dataverse environment.

### 6. How are Accounts retrieved and why is paging necessary?

**Answer:**\
The application queries Accounts through the Dataverse SDK. Paging is
implemented so the program can process more than the first result page
when many Accounts exist. The three-Account verification did not
exercise multiple pages, but the implementation supports them.

### 7. How does case-insensitive alphabetical sorting work and why use an Account ID tie-breaker?

**Answer:**\
After retrieval, Accounts are sorted case-insensitively by Account Name.
If two names compare equally, Account ID provides a stable secondary
tie-breaker. This makes the ordering deterministic.

### 8. Why does the user enter only one starting number?

**Answer:**\
That value initializes the counter. The program increments it once for
each alphabetically sorted Account. With starting number `3000`, the
verified sequence was therefore `3000`, `3001`, `3002`.

### 9. What is `cr0c9_firmennummer` and why is the number converted to a string?

**Answer:**\
`cr0c9_firmennummer` is the logical name of the Dataverse Firmennummer
column. That column is Single Line of Text, so calculated numeric values
are converted to strings before being written.

### 10. How do persistence, overwrite behavior and partial completion work?

**Answer:**\
Each successful SDK Update persists the Firmennummer in Dataverse and
overwrites an existing value. Accounts are updated individually. If a
later update fails, earlier updates may already be stored, so the actual
Dataverse state should be inspected before rerunning.

### 11. What does the three-Account verification prove?

**Answer:**\
With starting number `3000`, the console showed alphabetical processing
of `Alpha Test Account`, `SmartPoint Test Account`, and
`Zebra Test Account` with values `3000`, `3001`, and `3002`. Account
Management then showed the same persisted mapping. This verifies both
processing and persistence.

### 12. How does Case 04 differ from Case 05?

**Answer:**\
Case 04 is manually started and uses a user-supplied starting number.
Case 05 is timer-triggered and automatically starts from `1` on every
invocation.

------------------------------------------------------------------------

## Case 05 --- Timer-triggered Azure Function

### 1. Why does a timer-triggered Function fit scheduled automation?

**Answer:**\
The numbering should run automatically on a schedule and does not
require user interaction. A timer trigger is designed for exactly this
kind of scheduled background processing.

### 2. What is the isolated worker and what is the role of timerTrigger?

**Answer:**\
The Function uses the .NET isolated worker model, meaning the .NET code
runs in a separate worker process within the Azure Functions model. The
`timerTrigger` is the Function trigger and invokes
`NumberDataverseAccounts` according to the configured schedule.

### 3. How does the schedule work and why is RunOnStartup false?

**Answer:**\
The schedule comes from `SMARTPOINT_ACCOUNT_NUMBERING_SCHEDULE`. A
temporary 30-second schedule was used for local verification.
`RunOnStartup=false` means simply starting the host does not force a
business execution; the Function is intended to run because the timer
fires.

### 4. What are Azurite, local storage and the two-terminal startup?

**Answer:**\
The local Functions host needs storage for timer infrastructure. Azurite
provides a local Azure Storage emulator. One terminal therefore runs
Azurite while a second runs the Function host.
`AzureWebJobsStorage=UseDevelopmentStorage=true` connects the host to
local Azurite.

### 5. What are the seven environment variables?

**Answer:**\
The Function needs `SMARTPOINT_DATAVERSE_URL`, `SMARTPOINT_TENANT_ID`,
`SMARTPOINT_CLIENT_ID`, `SMARTPOINT_CLIENT_SECRET`,
`SMARTPOINT_ACCOUNT_NUMBERING_SCHEDULE`, `AzureWebJobsStorage`, and
`FUNCTIONS_WORKER_RUNTIME`. Secrets remain local and are neither printed
nor committed.

### 6. How do OAuth credentials, the Application User and permissions work?

**Answer:**\
Like the console, the Function authenticates with the Entra application
using client credentials. The corresponding Dataverse Application User
represents that application in `CRM816895`, and its security role
supplies the Account read/write permissions.

### 7. How do Account retrieval, paging, sorting and ID tie-breaking work?

**Answer:**\
The Function retrieves Accounts through the Dataverse SDK with paging,
sorts them case-insensitively by Account Name, and uses Account ID as a
deterministic tie-breaker for equal names. This mirrors the console's
ordering logic.

### 8. Why does every invocation start at 1?

**Answer:**\
Unlike Case 04, there is no user input. Every timer invocation
initializes its counter to `1`, so every successful run overwrites the
Accounts with a fresh sequence from `1` to `n`.

### 9. Why are Firmennummer values strings and what do overwrite and partial update mean?

**Answer:**\
Firmennummer is a text column, so values such as `1`, `2`, and `3` are
stored as strings. Existing values are overwritten on every run. Since
Accounts are updated one by one, earlier writes may already be persisted
if a later update fails.

### 10. What is the key contrast with Case 04?

**Answer:**\
Case 04 is a manually controlled batch with a user-selected starting
number. Case 05 is scheduled background automation and always numbers
from `1` on every invocation.

### 11. What do the runtime and persisted-result screenshots prove?

**Answer:**\
The runtime evidence shows two successful timer invocations processing
three Accounts. The Dataverse evidence then shows
`Alpha Test Account = 1`, `SmartPoint Test Account = 2`, and
`Zebra Test Account = 3`. Together they prove automatic execution and
persistence.

### 12. Why does local execution satisfy the case without Azure deployment?

**Answer:**\
The SmartPoint requirement explicitly allows local execution and does
not require Azure deployment. The Function runs locally with Azure
Functions Core Tools and Azurite while still performing real Dataverse
operations. That demonstrates the requested functionality without adding
an unnecessary cloud deployment step.

------------------------------------------------------------------------

## Case 06 --- Copilot Studio Account Creator

### 1. Where can I locate the tested agent, instructions, tool and test pane?

**Answer:**\
In Copilot Studio in environment `CRM816895`, I open
`Account Creator Standard Test`. The Overview shows the instructions and
model. Tools contains the enabled Dataverse add-row tool, and Test opens
the pane used for the creation request.

### 2. What is the difference between conversational instructions and the Dataverse tool?

**Answer:**\
The instructions guide how the agent understands the user's request and
how it should behave. They do not write data themselves. The Dataverse
tool is the concrete action that actually creates the Account row.

### 3. Which inputs are fixed and which input is AI-filled?

**Answer:**\
Environment `CRM816895` and table `Accounts` are fixed. Account Name is
dynamically filled from the user's request by AI. This lets the agent
understand natural language without allowing it to freely choose a
different environment or table.

### 4. Why must missing business information not be invented?

**Answer:**\
An agent that writes business data into Dataverse should not hallucinate
master data. The instructions therefore require the agent to ask when
the Account Name is missing or ambiguous and not invent missing business
information. That ambiguous-name path was not separately runtime-tested.

### 5. How do I trace the request through tool execution to the visible Account?

**Answer:**\
The user submits a request such as
`Create an account called SmartPoint Copilot Final Test.` The agent
identifies the Account Name, passes it to the configured Dataverse Add
Row tool, and the tool creates the Account. The new row can then be
found in `Account Management`.

### 6. What do the agent response and Account Management screenshot each demonstrate?

**Answer:**\
The agent response shows that the tool execution returned success in the
test and includes details such as the Account ID. The Account Management
evidence independently shows that the row was actually persisted in
Dataverse. Together they provide the end-to-end proof.

### 7. How do I distinguish the earlier credit blocker from the successful final test?

**Answer:**\
An earlier attempt stopped with `EnforcementUsageCredits` before tool
execution because no Copilot Credits were available. The later
`Account Creator Standard Test` in the standard harness successfully
created an Account. The credit error is therefore troubleshooting
history, not the final status. We do not claim an unverified licensing
change as the cause of the later success.

### 8. What is the difference between testing, publishing and untested enrichment capabilities?

**Answer:**\
The agent's creation behavior was verified end-to-end in the test pane.
Publishing or deployment was not demonstrated. Web Search or web
enrichment was also not established by the final test. Those
capabilities therefore must not be presented as verified.

------------------------------------------------------------------------

## Case 07 --- Optional Opportunity Notification

### 1. What was the intended design and what was actually implemented?

**Answer:**\
The intended design was a Dataverse trigger for a newly created
Opportunity, followed by resolving the Owner and sending that Owner an
email containing the Opportunity title and date. The flow was not
implemented or runtime-tested because the standard Opportunity table is
not available in the provided environment.

### 2. Why was the standard Opportunity table required and how was its absence checked?

**Answer:**\
The task explicitly refers to a sales Opportunity, so the intended
target is the standard sales concept rather than a custom imitation. We
searched for `Opportunity` and `Verkaufschance` in the Dataverse
trigger, checked the Dataverse table list outside the solution, and
checked solution `Achim Beispiel`. The standard table was not available.

### 3. Why was neither a custom replacement nor an unverified Sales/demo package introduced?

**Answer:**\
A custom table would only imitate the standard Opportunity and would not
faithfully implement the requested scenario. At the same time, I did not
want to alter the provided environment for an optional exercise by
installing an unverified Dynamics 365 Sales/demo package. The case was
therefore documented as evaluated but intentionally not implemented.

### Short interview wording

> I deliberately intended to use the standard Opportunity table instead
> of creating a custom substitute. Because that table is not provisioned
> in the provided environment, I chose not to alter the environment by
> installing an unverified Sales/demo package solely for an optional
> exercise. I documented the intended design and the verified limitation
> instead.

------------------------------------------------------------------------

## Overall Picture

The cases demonstrate different ways of extending and automating
Dataverse:

-   **Case 01:** Data model and model-driven app.
-   **Case 02:** Low-code automation with Power Automate.
-   **Case 03:** Client-side form extension with JavaScript.
-   **Case 04:** Manually triggered external Dataverse processing with
    C#/.NET SDK.
-   **Case 05:** Scheduled background processing with Azure Functions.
-   **Case 06:** Conversational AI with a Dataverse write action.
-   **Case 07:** Evaluation of an optional flow scenario and a
    deliberate scope decision caused by the environment.

The common foundation is Dataverse as the central data platform; the
cases demonstrate different ways to model, read, modify and present data
or react to events.
