# SmartPoint Dynamics Case --- Fragen und Antworten

> **Zweck:** Persönliche Interview- und Demo-Vorbereitung.\
> Dieses Dokument ist bewusst von der eigentlichen Case-Dokumentation
> getrennt und soll **nicht** Bestandteil des vorab versendeten
> Nachweis-PDFs sein.
>
> **Grundlage:** Die Fragen stammen aus den aktuellen „What I Should Be
> Able to Explain"-Abschnitten der Case-Dokumentation 01--07. Die
> Antworten orientieren sich ausschließlich am dokumentierten und
> verifizierten Projektstand.

------------------------------------------------------------------------

## Case 01 --- Room Planning

### 1. Warum sind Location, Room und Booking getrennte Tabellen?

**Antwort:**\
Sie stellen drei unterschiedliche fachliche Objekte dar. Eine Location
beschreibt einen Standort, ein Room einen konkreten Raum an diesem
Standort und ein Booking eine Reservierung dieses Raums für einen
bestimmten Tag und Mitarbeiter. Durch die Trennung werden Daten nicht
unnötig dupliziert und Beziehungen können sauber über Dataverse-Lookups
modelliert werden.

### 2. Wie funktionieren die Beziehungen Location → Room → Booking?

**Antwort:**\
Eine Location kann mehrere Rooms enthalten, daher besteht eine
1:N-Beziehung von Location zu Room. Ein Room kann wiederum viele
Bookings haben, daher besteht eine weitere 1:N-Beziehung von Room zu
Booking. Ein einzelnes Booking verweist jeweils auf genau einen Room.

### 3. Warum verweist Employee auf die bestehende User-Tabelle?

**Antwort:**\
Mitarbeiter sind in Dataverse bereits als System Users vorhanden.
Deshalb wäre eine eigene Employee-Tabelle unnötige Datenverdopplung. Das
Booking verwendet einen Lookup auf die vorhandene User-Tabelle und kann
damit den bestehenden Benutzer direkt referenzieren.

### 4. Warum ist Booking Date ein „Date Only"-Feld?

**Antwort:**\
Die Anforderung sieht ausschließlich ganztägige Buchungen vor. Es werden
daher weder Startzeit noch Endzeit benötigt. Ein Date-Only-Feld bildet
genau diese fachliche Anforderung ab und verhindert unnötige
Zeitinformationen.

### 5. Warum gibt es Maximum Capacity und Planning Capacity getrennt?

**Antwort:**\
Maximum Capacity beschreibt die tatsächliche maximale Raumkapazität.
Planning Capacity beschreibt dagegen die für die Planung gewünschte
Belegung. Damit bleiben physische Kapazität und organisatorische
Planungsgrenze als unterschiedliche Informationen erhalten.

### 6. Wie bilden Maximum Capacity 10 und Planning Capacity 5 die 50-%-Anforderung ab?

**Antwort:**\
Beim Demonstrationsraum beträgt die maximale Kapazität 10 Personen. Die
Planning Capacity wurde auf 5 gesetzt. Damit entspricht die geplante
Belegung genau 50 % der maximalen Kapazität.

### 7. Wie funktioniert die View „Today's Bookings"?

**Antwort:**\
Die View zeigt aktive Bookings, deren Booking Date dem aktuellen Tag
entspricht. Die Buchungen werden nach Booking Date aufsteigend sortiert.
Im Test wurde verifiziert, dass eine Buchung für heute erscheint und
eine Buchung für den folgenden Tag nicht.

### 8. Warum kann trotz Planning Capacity 5 eine sechste Buchung gespeichert werden?

**Antwort:**\
Planning Capacity ist in unserer Lösung bewusst nur eine Informations-
beziehungsweise Planungsgröße. Es gibt keine Dataverse-Regel oder
Custom-Logik, die weitere Bookings verhindert. Deshalb konnte die
sechste Buchung erfolgreich gespeichert werden.

### 9. Warum wurde keine Overbooking-Validierung programmiert?

**Antwort:**\
Die SmartPoint-Aufgabe verlangt keine eigene Overbooking-Validierung.
Die 50-%-Belegung sollte im Datenmodell abgebildet werden, aber Custom
Programming zur Verhinderung weiterer Buchungen war ausdrücklich nicht
erforderlich. Deshalb wurde diese zusätzliche Komplexität bewusst
außerhalb des Scopes gelassen.

### 10. Wo finde ich App und Implementierung?

**Antwort:**\
Die Laufzeit-App ist in Power Apps im Environment `CRM816895` als
`Room Planning` verfügbar. Dort kann ich Locations, Rooms und Bookings
sowie die View `Today's Bookings` zeigen. Die Implementierung finde ich
im Maker Portal unter Solutions → `Achim Beispiel` bei den Tabellen
Location, Room und Booking beziehungsweise bei der `Room Planning` App.

------------------------------------------------------------------------

## Case 02 --- Booking Confirmation

### 1. Was startet den Flow und aus welchen vier Schritten besteht er?

**Antwort:**\
Der Flow startet, wenn in Dataverse ein neues Booking angelegt wird.
Danach wird zuerst der zugehörige Employee/User geladen, anschließend
der zugehörige Room, und zuletzt wird über Office 365 Outlook eine
Bestätigungs-E-Mail gesendet. Zusammen mit dem Dataverse-Trigger sind
das die vier sichtbaren Flow-Schritte.

### 2. Warum wird der Employee/User zusätzlich aus Dataverse geladen?

**Antwort:**\
Im Booking steckt ein Lookup auf den Employee/User. Für die E-Mail
benötigen wir Daten aus dem zugehörigen User-Datensatz, insbesondere die
Primary Email als Empfängeradresse. Deshalb wird der referenzierte User
über „Get a row by ID" aufgelöst.

### 3. Warum wird der Room zusätzlich geladen?

**Antwort:**\
Auch der Room ist im Booking als Lookup gespeichert. Für die
verständliche Bestätigungsmail brauchen wir den Room Name. Deshalb wird
der referenzierte Room mit einem zweiten „Get a row by ID"-Schritt
geladen.

### 4. Welche Daten sind in der empfangenen E-Mail relevant?

**Antwort:**\
Der Empfänger ist die E-Mail-Adresse des gebuchten Mitarbeiters. Die
Mail enthält das Booking Date und den Room Name. Im verifizierten Test
wurde die Mail mit dem Betreff `Room booking confirmation`, dem
korrekten Datum und `Graz Meeting Room 01` tatsächlich empfangen.

### 5. Welche Rolle spielt Dataverse und welche Outlook?

**Antwort:**\
Dataverse ist die Daten- und Ereignisquelle: Dort wird das Booking
gespeichert und dort liegen die referenzierten User- und Room-Daten.
Office 365 Outlook übernimmt ausschließlich die Zustellung der E-Mail.
Power Automate verbindet diese beiden Teile im Flow.

### 6. Warum beweisen erfolgreicher Run und empfangene Mail den End-to-End-Erfolg?

**Antwort:**\
Ein grüner Flow-Run zeigt, dass die technischen Schritte erfolgreich
ausgeführt wurden. Die tatsächlich empfangene Mail beweist zusätzlich,
dass das Ergebnis das Zielsystem erreicht hat. Zusammen zeigen beide
Nachweise die vollständige Kette vom neuen Dataverse-Booking bis zur
E-Mail im Postfach.

### 7. Wo finde ich Flow, Run History und Run Details?

**Antwort:**\
In Power Automate im Environment `CRM816895` öffne ich
`Booking Confirmation`. Auf der Overview-Seite sehe ich den
28-Tage-Ausführungsverlauf und kann über „Alle Ausführungen" die Runs
öffnen. In einem konkreten Run kann ich jeden Trigger und jede Action
samt Status, Inputs und Outputs untersuchen.

### 8. Was bedeuten grüne Schritte, Retry oder Failed?

**Antwort:**\
Grün bedeutet, dass der Schritt erfolgreich abgeschlossen wurde. Retry
bedeutet, dass Power Automate die Action erneut versucht, typischerweise
wegen eines temporären Problems. Failed bedeutet, dass die Action
endgültig fehlgeschlagen ist; dann müssen Status und Fehlermeldung des
konkreten Schritts untersucht werden.

### 9. Wie erkläre ich das Hostname-Problem, ohne fälschlich die Business-Daten zu beschuldigen?

**Antwort:**\
Beim späteren Retest meldete Dataverse `UnresolvableHostName`
beziehungsweise `HostNotFound` und referenzierte einen veralteten
Hostnamen. Die Inputs wie Tabelle und Record ID sahen plausibel aus.
Deshalb war das ein Hinweis auf Connection-/Backend-/Discovery-Probleme
und nicht automatisch auf falsche Booking- oder User-Daten. Ein frisch
kopierter Flow mit neu gebundenen Dataverse-Verbindungen funktionierte
anschließend wieder end-to-end.

------------------------------------------------------------------------

## Case 03 --- JavaScript Account Form Notification

### 1. Was startet das JavaScript und wo wird das konfiguriert?

**Antwort:**\
Die Funktion wird beim Laden des Account-Hauptformulars über das
OnLoad-Event gestartet. Konfiguriert ist das im Maker Portal im Account
Main Form unter Events → OnLoad. Dort ist die Library
`cr0c9_AccountFormNotification.js` und die Funktion
`showCompanyNumberNotification` registriert.

### 2. Was sind executionContext und formContext und warum muss der Context übergeben werden?

**Antwort:**\
`executionContext` wird vom Formularereignis bereitgestellt und enthält
den Kontext des aktuellen Events. Daraus holt die Funktion mit
`getFormContext()` das konkrete `formContext`. Über dieses Objekt kann
das Script auf Attribute und UI des geöffneten Accounts zugreifen. Ohne
übergebenen Execution Context könnte die Funktion nicht zuverlässig auf
das aktuelle Formular zugreifen.

### 3. Woher kommen Account Name und Firmennummer?

**Antwort:**\
Beide Werte kommen aus den Attributen des aktuell geöffneten
Account-Datensatzes in Dataverse. Das Script liest `name` für den
Account Name und `cr0c9_firmennummer` für die Firmennummer.

### 4. Warum verwendet der Code `name` und `cr0c9_firmennummer` statt der sichtbaren Labels?

**Antwort:**\
JavaScript greift über die logischen Dataverse-Spaltennamen auf
Attribute zu. Display Labels können übersetzt oder geändert werden und
sind nicht die technische Identität der Spalte. Deshalb wird
beispielsweise `cr0c9_firmennummer` und nicht das sichtbare Label
`Firmennummer` verwendet.

### 5. Wie wird der Satz erzeugt und angezeigt?

**Antwort:**\
Das Script liest Firmenname und Firmennummer, setzt beide Werte in den
vorgegebenen deutschen Satz ein und zeigt ihn mit
`formContext.ui.setFormNotification()` als `INFO`-Notification im
Formular an.

### 6. Warum gibt es eine stabile Notification ID?

**Antwort:**\
Die feste ID `companyNumberNotification` erlaubt es, genau diese Meldung
später gezielt zu ersetzen oder zu löschen. Dadurch wird nicht
versehentlich eine andere Formularmeldung entfernt.

### 7. Was passiert, wenn Attribute oder benötigte Werte fehlen?

**Antwort:**\
Dann wird keine unvollständige Meldung angezeigt. Das Script unterdrückt
die Notification und löscht gegebenenfalls die vorherige Meldung mit
derselben ID. Auch leere beziehungsweise nur aus Leerzeichen bestehende
Werte werden so behandelt.

### 8. Wo finde ich Web Resource, Source File und OnLoad Handler?

**Antwort:**\
Die Web Resource liegt in der Solution `Achim Beispiel` als
`Account Form Notification` beziehungsweise
`cr0c9_AccountFormNotification.js`. Der Handler ist im Account Main Form
unter OnLoad registriert. Die lokale Source-Datei liegt im Repository
unter `src/javascript/AccountFormNotification.js`.

### 9. Wie demonstriere ich, dass Feldwert und Notification zusammenpassen?

**Antwort:**\
Ich öffne `SmartPoint Test Account` in der App `Account Management`,
zeige die aktuelle Firmennummer und vergleiche sie mit der
Formularmeldung. Im final dokumentierten Zustand lautet der Wert `1`,
und die Notification enthält ebenfalls `1`.

### 10. Was ist der Unterschied zwischen Dataverse-Datenspeicherung und clientseitigem Formularverhalten?

**Antwort:**\
Dataverse speichert Account Name und Firmennummer dauerhaft. Das
JavaScript ändert in diesem Case keine Daten, sondern läuft clientseitig
im geöffneten Formular und stellt vorhandene Daten als UI-Notification
dar.

------------------------------------------------------------------------

## Case 04 --- C# Dataverse Console Application

### 1. Warum eignet sich eine Console Application für diese Aufgabe?

**Antwort:**\
Die Nummerierung ist ein manuell gestarteter Batch-Vorgang: Der Benutzer
gibt einmal eine Startnummer ein und das Programm verarbeitet danach
alle Accounts. Dafür ist eine kleine Console Application einfach,
kontrollierbar und gut nachvollziehbar; eine permanente
Benutzeroberfläche oder ein Serverprozess wäre unnötig.

### 2. Was sind ServiceClient, QueryExpression, Entity und SDK Update?

**Antwort:**\
`ServiceClient` stellt die authentifizierte Verbindung zu Dataverse her.
`QueryExpression` beschreibt die Abfrage der Account-Datensätze. Ein
`Entity`-Objekt repräsentiert einen Dataverse-Datensatz. Mit der
SDK-`Update`-Operation wird anschließend die neue Firmennummer am
jeweiligen Account gespeichert.

### 3. Wie funktionieren Entra Credentials, Dataverse Application User und Security Role zusammen?

**Antwort:**\
Die Console authentifiziert sich mit einer Entra Application über Tenant
ID, Client ID und Client Secret. In Dataverse existiert dazu ein
Application User, der diese App repräsentiert. Dessen Security Role
bestimmt, welche Dataverse-Daten die Anwendung lesen oder ändern darf.
Für den praktischen Case wurde System Administrator verwendet; das ist
keine Empfehlung für Least Privilege in Produktion.

### 4. Welche vier Environment Variables gibt es und warum steht das Secret nicht im Source Code?

**Antwort:**\
Verwendet werden `SMARTPOINT_DATAVERSE_URL`, `SMARTPOINT_TENANT_ID`,
`SMARTPOINT_CLIENT_ID` und `SMARTPOINT_CLIENT_SECRET`. Konfiguration und
insbesondere Secrets bleiben damit außerhalb des Repositorys. So wird
das Client Secret weder committed noch in öffentlich sichtbarem Source
Code gespeichert.

### 5. Was muss ich nach einem neuen Terminal wiederherstellen?

**Antwort:**\
Die vier benötigten Environment Variables müssen in der Session
vorhanden sein, bevor die Console gestartet wird. Insbesondere das
Secret wird lokal und sicher gesetzt und nicht aus dem Repository
geladen. Danach kann die Anwendung mit der Dataverse-Umgebung verbinden.

### 6. Wie werden Accounts gelesen und warum ist Paging notwendig?

**Antwort:**\
Die Anwendung fragt Accounts über das Dataverse SDK ab. Paging ist
implementiert, damit nicht nur die erste Ergebnisseite verarbeitet wird,
wenn sehr viele Accounts vorhanden sind. Unser Drei-Account-Test hat die
Paging-Logik nicht über mehrere Seiten praktisch beansprucht, aber die
Implementierung ist dafür vorbereitet.

### 7. Wie funktioniert die alphabetische Sortierung und warum gibt es einen Account-ID-Tie-Breaker?

**Antwort:**\
Nach dem Abruf werden Accounts case-insensitive nach Account Name
sortiert. Wenn zwei Namen gleich sortieren, dient die Account ID als
zusätzlicher stabiler Tie-Breaker. Dadurch bleibt die Reihenfolge
deterministisch.

### 8. Warum gibt der Benutzer nur eine Startnummer ein?

**Antwort:**\
Die eingegebene Zahl ist der Startwert. Das Programm hält anschließend
einen Zähler und erhöht ihn für jeden alphabetisch sortierten Account um
eins. Bei Start `3000` entstanden im Test deshalb `3000`, `3001` und
`3002`.

### 9. Was bedeutet `cr0c9_firmennummer` und warum wird in einen String konvertiert?

**Antwort:**\
`cr0c9_firmennummer` ist der logische Name der Dataverse-Spalte
Firmennummer. Diese Spalte wurde als Single Line of Text angelegt.
Deshalb werden die berechneten numerischen Werte vor dem Speichern als
String geschrieben.

### 10. Wie funktionieren Persistenz, Überschreiben und Teilfortschritt bei Fehlern?

**Antwort:**\
Jeder erfolgreiche SDK-Update schreibt die Firmennummer dauerhaft nach
Dataverse und überschreibt einen vorhandenen Wert. Die Updates erfolgen
Account für Account. Wenn später ein Fehler auftritt, können vorherige
Updates bereits gespeichert sein; deshalb muss man bei einem Fehler den
tatsächlichen Dataverse-Stand prüfen, bevor man erneut startet.

### 11. Was beweist der Drei-Account-Test?

**Antwort:**\
Die Console zeigte bei Startnummer `3000` die alphabetische Verarbeitung
`Alpha Test Account`, `SmartPoint Test Account`, `Zebra Test Account`
und die Nummern `3000`, `3001`, `3002`. Die Account-Liste in
`Account Management` zeigte anschließend dieselbe Zuordnung. Damit sind
Verarbeitung und Persistenz gemeinsam nachgewiesen.

### 12. Was unterscheidet Case 04 von Case 05?

**Antwort:**\
Case 04 wird manuell gestartet und übernimmt eine vom Benutzer
eingegebene Startnummer. Case 05 wird durch einen Timer ausgelöst und
beginnt bei jeder Ausführung automatisch wieder bei `1`.

------------------------------------------------------------------------

## Case 05 --- Timer-triggered Azure Function

### 1. Warum passt eine Timer-triggered Function zu dieser Aufgabe?

**Antwort:**\
Die Nummerierung soll automatisch in regelmäßigen Abständen laufen und
benötigt keine Benutzerinteraktion. Ein Timer Trigger ist genau für
solche geplanten Hintergrundaufgaben geeignet.

### 2. Was bedeutet .NET isolated worker und welche Rolle spielt timerTrigger?

**Antwort:**\
Die Function läuft im .NET-Isolated-Worker-Modell, also in einem
separaten .NET Worker Process des Azure-Functions-Modells. Der
`timerTrigger` ist der Auslöser der Function und startet
`NumberDataverseAccounts` entsprechend dem konfigurierten Zeitplan.

### 3. Wie funktioniert der Schedule und warum ist RunOnStartup false?

**Antwort:**\
Der Schedule kommt aus `SMARTPOINT_ACCOUNT_NUMBERING_SCHEDULE`. Für den
lokalen Test wurde temporär ein kurzer Zeitplan von 30 Sekunden
verwendet. `RunOnStartup=false` bedeutet, dass ein bloßer Host-Start
noch keine fachliche Ausführung erzwingt; die Function soll durch den
Timer und nicht durch jeden Neustart ausgelöst werden.

### 4. Wofür brauchen wir Azurite und zwei Terminals?

**Antwort:**\
Der lokale Functions Host benötigt Storage für die Timer-Infrastruktur.
Azurite stellt diesen Azure-Storage-Dienst lokal bereit. Deshalb läuft
in einem Terminal Azurite und in einem zweiten Terminal die Function.
`AzureWebJobsStorage=UseDevelopmentStorage=true` verbindet den lokalen
Host mit Azurite.

### 5. Welche sieben Environment Variables müssen vorhanden sein?

**Antwort:**\
Die Function benötigt die vier Dataverse-Werte
`SMARTPOINT_DATAVERSE_URL`, `SMARTPOINT_TENANT_ID`,
`SMARTPOINT_CLIENT_ID`, `SMARTPOINT_CLIENT_SECRET` sowie
`SMARTPOINT_ACCOUNT_NUMBERING_SCHEDULE`, `AzureWebJobsStorage` und
`FUNCTIONS_WORKER_RUNTIME`. Secrets werden lokal gesetzt und nicht
ausgegeben oder committed.

### 6. Wie funktionieren OAuth, Application User und Berechtigungen?

**Antwort:**\
Wie bei der Console authentifiziert sich die Function über die Entra
Application mit Client Credentials. Der dazugehörige Dataverse
Application User repräsentiert die Anwendung in `CRM816895`, und seine
Security Role gibt die benötigten Lese- und Schreibrechte auf Accounts.

### 7. Wie funktionieren Retrieval, Paging, Sortierung und Tie-Breaking?

**Antwort:**\
Die Function liest Accounts über das Dataverse SDK mit Paging, sortiert
sie case-insensitive alphabetisch und verwendet die Account ID als
deterministischen Tie-Breaker bei gleichen Namen. Damit entspricht die
Reihenfolge der Console-Logik.

### 8. Warum beginnt jede Invocation bei 1?

**Antwort:**\
Anders als Case 04 gibt es keinen Benutzereingabewert. Bei jedem
Timer-Aufruf wird der Zähler neu mit `1` initialisiert. Deshalb
überschreibt jede erfolgreiche Ausführung die Accounts erneut mit einer
Sequenz `1..n`.

### 9. Warum sind die Werte Strings und was bedeutet Overwrite/Partial Update?

**Antwort:**\
Firmennummer ist eine Textspalte, deshalb werden `1`, `2`, `3` als
Strings gespeichert. Vorhandene Werte werden bei jedem Lauf
überschrieben. Da Accounts einzeln aktualisiert werden, können bei einem
späteren Fehler frühere Updates bereits persistiert sein.

### 10. Was ist der wichtigste Unterschied zu Case 04?

**Antwort:**\
Case 04 ist ein manuell kontrollierter Batch mit frei gewählter
Startnummer. Case 05 ist eine automatisch geplante
Hintergrundverarbeitung und nummeriert bei jedem Lauf immer ab `1`.

### 11. Was beweisen Runtime- und Dataverse-Screenshots?

**Antwort:**\
Der Runtime-Nachweis zeigt zwei erfolgreiche Timer Invocations, die drei
Accounts verarbeitet haben. Der Dataverse-Nachweis zeigt anschließend
`Alpha Test Account = 1`, `SmartPoint Test Account = 2` und
`Zebra Test Account = 3`. Damit ist sowohl die automatische Ausführung
als auch die Persistenz belegt.

### 12. Warum reicht lokale Ausführung ohne Azure Deployment?

**Antwort:**\
Die SmartPoint-Aufgabe verlangt für diesen Case ausdrücklich nur die
lokale Ausführung; ein Azure Deployment ist nicht erforderlich. Die
Function läuft mit Azure Functions Core Tools und Azurite lokal und
führt dabei die echte Dataverse-Verarbeitung aus. Damit ist die
geforderte Funktionalität demonstriert, ohne einen nicht verlangten
Cloud-Deployment-Schritt hinzuzufügen.

------------------------------------------------------------------------

## Case 06 --- Copilot Studio Account Creator

### 1. Wo finde ich den getesteten Agent, Instructions, Tool und Test Pane?

**Antwort:**\
In Copilot Studio im Environment `CRM816895` öffne ich den Agent
`Account Creator Standard Test`. Auf der Overview-Seite sind
Instructions und Modell sichtbar. Unter Tools befindet sich das
aktivierte Dataverse-Add-Row-Tool, und über Test öffne ich das Test Pane
für die Anfrage.

### 2. Was ist der Unterschied zwischen conversational instructions und dem Dataverse Tool?

**Antwort:**\
Die Instructions steuern, wie der Agent die Benutzeranfrage versteht und
wie er sich verhalten soll. Sie schreiben selbst keine Daten. Das
Dataverse Tool ist die konkrete Action, die tatsächlich einen
Account-Datensatz in Dataverse anlegt.

### 3. Was ist fest konfiguriert und was wird von AI befüllt?

**Antwort:**\
Environment `CRM816895` und die Tabelle `Accounts` sind fest vorgegeben.
Der Account Name wird aus der Benutzeranfrage dynamisch mit AI befüllt.
Dadurch kann der Agent natürliche Sprache verstehen, ohne frei
entscheiden zu dürfen, in welches Environment oder welche Tabelle
geschrieben wird.

### 4. Warum darf fehlende Business-Information nicht erfunden werden?

**Antwort:**\
Ein Agent, der Dataverse-Daten schreibt, sollte keine geschäftlichen
Stammdaten halluzinieren. Die Instructions verlangen deshalb, bei
fehlendem oder mehrdeutigem Account Name nachzufragen und keine
fehlenden Business-Daten zu erfinden. Der separate Ambiguous-Name-Pfad
wurde allerdings nicht zusätzlich runtime-getestet.

### 5. Wie läuft die Anfrage bis zum sichtbaren Account?

**Antwort:**\
Der Benutzer fordert beispielsweise
`Create an account called SmartPoint Copilot Final Test.` an. Der Agent
erkennt den Account Name, übergibt ihn an das konfigurierte Dataverse
Add-Row Tool, und das Tool erstellt den Account. Danach kann der neue
Datensatz in `Account Management` gefunden werden.

### 6. Was beweisen Agent Response und Account-Management-Screenshot jeweils?

**Antwort:**\
Die Agent Response zeigt, dass die Tool-Ausführung im Test erfolgreich
zurückgemeldet wurde und liefert unter anderem die Account ID. Der
Account-Management-Nachweis zeigt unabhängig davon, dass der Datensatz
tatsächlich in Dataverse persistiert wurde. Erst zusammen ist der
End-to-End-Nachweis vollständig.

### 7. Was war der frühere Credit Blocker und warum ist er nicht der finale Status?

**Antwort:**\
Ein früherer Versuch stoppte mit `EnforcementUsageCredits`, bevor das
Tool ausgeführt wurde, weil keine Copilot Credits verfügbar waren. Der
spätere Test mit `Account Creator Standard Test` im Standard-Harness war
erfolgreich und erstellte tatsächlich einen Account. Deshalb ist der
Credit-Fehler Troubleshooting-Historie und nicht der Endstatus. Wir
behaupten dabei keine nicht verifizierte interne Lizenzänderung als
Ursache.

### 8. Was ist der Unterschied zwischen Testing, Publishing und ungetesteten Enrichment-Funktionen?

**Antwort:**\
Wir haben die Agent-Funktion im Test Pane end-to-end verifiziert. Wir
haben nicht nachgewiesen, dass der Agent veröffentlicht oder deployed
wurde. Ebenso ist Web Search beziehungsweise Web Enrichment im finalen
Test nicht als funktionierend nachgewiesen. Diese Punkte dürfen deshalb
nicht als abgeschlossen dargestellt werden.

------------------------------------------------------------------------

## Case 07 --- Optional Opportunity Notification

### 1. Was wäre das vorgesehene Design und was wurde tatsächlich implementiert?

**Antwort:**\
Vorgesehen wäre ein Dataverse-Trigger auf eine neu erstellte
Opportunity, anschließend die Ermittlung des Owners und eine E-Mail an
diesen Owner mit Opportunity-Titel und Datum. Dieser Flow wurde jedoch
nicht implementiert oder runtime-getestet, weil die
Standard-Opportunity-Tabelle in der bereitgestellten Umgebung nicht
verfügbar ist.

### 2. Warum war die Standard-Opportunity-Tabelle erforderlich und wie wurde ihr Fehlen geprüft?

**Antwort:**\
Die Aufgabe spricht ausdrücklich von einer Verkaufschance
beziehungsweise Opportunity und damit vom Standard-Sales-Konzept. Wir
haben im Dataverse-Trigger nach `Opportunity` und `Verkaufschance`
gesucht, die Tabellenliste außerhalb der Solution geprüft und auch die
Solution `Achim Beispiel` kontrolliert. Die Standardtabelle war dort
nicht verfügbar.

### 3. Warum wurde weder eine Custom-Tabelle noch ein Sales-/Demo-Paket verwendet?

**Antwort:**\
Eine eigene Ersatz-Tabelle würde die ausdrücklich gemeinte
Standard-Opportunity nur imitieren und wäre keine originalgetreue
Umsetzung. Gleichzeitig wollten wir die bereitgestellte Umgebung für
eine optionale Aufgabe nicht durch ein unbestätigtes
Dynamics-365-Sales-/Demo-Paket verändern. Deshalb wurde der Case bewusst
als evaluiert, aber nicht implementiert dokumentiert.

### Kurze Interviewformulierung

> Ich wollte bewusst die Standard-Opportunity-Tabelle verwenden und
> keine eigene Ersatzstruktur bauen. Da diese Tabelle in der
> bereitgestellten Umgebung nicht provisioniert ist, habe ich die
> Umgebung nicht nur für eine optionale Aufgabe durch ein nicht
> verifiziertes Sales-/Demo-Paket verändert. Ich habe stattdessen das
> vorgesehene Design und die geprüfte Einschränkung dokumentiert.

------------------------------------------------------------------------

## Gesamtbild

Die Cases zeigen unterschiedliche Erweiterungs- und
Automatisierungsmöglichkeiten rund um Dataverse:

-   **Case 01:** Datenmodell und Model-driven App.
-   **Case 02:** Low-Code-Automatisierung mit Power Automate.
-   **Case 03:** Clientseitige Formularerweiterung mit JavaScript.
-   **Case 04:** Manuell gestartete externe Dataverse-Verarbeitung mit
    C#/.NET SDK.
-   **Case 05:** Zeitgesteuerte Hintergrundverarbeitung mit Azure
    Functions.
-   **Case 06:** Conversational AI mit einer Dataverse-Schreibaktion.
-   **Case 07:** Bewertung eines optionalen Flow-Szenarios und bewusste
    Scope-Entscheidung aufgrund der Umgebung.

Der gemeinsame Kern ist Dataverse als zentrale Datenplattform; die Cases
zeigen verschiedene Wege, Daten zu modellieren, zu lesen, zu verändern,
anzuzeigen oder auf Ereignisse zu reagieren.
