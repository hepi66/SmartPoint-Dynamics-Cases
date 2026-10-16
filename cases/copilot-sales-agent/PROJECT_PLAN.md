# Copilot Studio Sales Agent Project Plan

## Projektkontext

- **Projekt:** Copilot Studio Sales Agent
- **Umgebung:** `CRM738378`
- **Agent:** `SmartPoint Sales Assistant`
- **Aufgabenquelle:** [Hepberger Copilot Studio Beispiel - smartpoint.pdf](Hepberger_Copilot_Studio_Beispiel-smartpoint.pdf)
- **Zusätzlicher Kontext:** [E-Mail von Dominic Bley](email_D_Bley_Copilot_Studio_Beispiel-smartpoint.pdf). Die PDF enthält Zugangsinformationen und darf nicht in Git eingecheckt oder in andere Dokumente kopiert werden.
- **Fallverzeichnis:** `cases/copilot-sales-agent/`
- **Fortschritt:** [PROJECT_PROGRESS.md](PROJECT_PROGRESS.md)

## Statusbegriffe

- **VERIFIZIERT:** Ergebnis wurde in der Zielumgebung praktisch beobachtet.
- **TEILWEISE VERIFIZIERT:** Ein Teil funktioniert; die vollständige Abnahmeanforderung ist noch offen.
- **UNVOLLSTÄNDIG:** Konfiguration oder Umsetzung existiert, aber erforderliche Teile fehlen.
- **BLOCKIERT:** Ein reproduzierter Fehler verhindert aktuell den konkreten Umsetzungspfad. Das sagt allein nichts über alternative Pfade aus.
- **UNVERIFIZIERT / PENDING:** Es gibt keinen ausreichenden Laufzeitnachweis.
- Eine Bestätigungsnachricht gilt nicht als Nachweis für einen anschließenden Dataverse-Schreibvorgang.

## MVP-Ziel und Abnahmekriterien

**MVP-Abnahme:** Ein funktionierender Copilot Studio Sales Agent in `CRM738378`, der in Microsoft Teams nutzbar ist und:

1. Kunden- und Opportunity-Daten aus Dataverse liest.
2. Eine Produktfrage anhand von SharePoint-Wissen mit einem Quellenlink beantwortet.
3. Nach ausdrücklicher Bestätigung über einen kontrollierten Flow eine Opportunity erstellt.
4. einen nutzbaren Link zum erstellten Datensatz zurückgibt.
5. die neu erstellte Opportunity wieder abrufen kann.

**Architecture First:** (1) Zuerst wird die minimale, funktionierende Ende-zu-Ende-Architektur für dieses MVP aufgebaut. (2) Danach werden alle übrigen Fallanforderungen implementiert. (3) Anschließend werden Robustheit, Sicherheit und Antwortqualität verbessert. Diese Reihenfolge erweitert oder ändert den vereinbarten MVP-Umfang nicht.

## Approved Story Workflow and Audit Baseline (updated 2026-10-16)

Work on exactly one user story at a time: **Plan -> Implement -> Test -> Fix -> Accept -> Presentation -> Next story.** S01 / US-01 was functionally accepted after T1 and T2 passed. S02 / US-02 is functionally accepted after T3 and T4 passed in the Copilot Studio test chat. After each accepted story, prepare a presentation covering requirements, implementation, architecture, and test evidence before the next story's implementation or testing. S03 / US-03 is next for preparation; its implementation and T5 test have not started. A story is accepted only after its criteria are met and evidenced. Do not repeat full-project audits unless requirements or architecture materially change.

| Story | Status (updated 2026-10-16) | Implemented and verified | Open / acceptance | Related tests |
| --- | --- | --- | --- | --- |
| US-01 / S01 — Customer overview | FUNCTIONALLY COMPLETE / ACCEPTED (2026-10-15); known non-blocking limitations | T1 verified four open Huber Logistik opportunities with stage, estimated revenue and expected close date, plus an open quote and recent customer activity. T2 verified clarification between `Huber Logistik` and `Huber Maschinenbau GmbH`. | Numeric Choice value is returned for sales stage; currency display is not reliably verified and must not be assumed to be EUR; end-to-end response time below 10 seconds is not verified. These limitations do not block the stated functional acceptance. | T1 PASS; T2 PASS |
| US-02 / S02 — Product knowledge | FUNCTIONALLY COMPLETE / ACCEPTED (2026-10-16), based on Copilot Studio test evidence | T3 PASS: the agent compared AX-200 and AX-300 and provided SharePoint citations. T4 PASS after correction: the agent stated that Italian VAT information was unavailable in the company knowledge base. | T4 observation: the initial response used general model knowledge even though web search was disabled. After correction, the agent stated the information gap. Do not infer a permanent fix or claim more testing than this evidence supports. | T3 PASS; T4 PASS |
| US-03 / S03 — Create Opportunity | PREPARATION / START NEXT; core create path verified, full acceptance open | The controlled flow, one confirmation, Opportunity creation, returned ID, and independent read-back in `SmartPoint Sales Demo` were verified on 2026-10-15. | The exact T5 scenario, required field collection, missing close-date clarification, summary/confirmation for that scenario, and dynamically returned clickable record link remain open. The verified app URL was opened manually; the flow does not yet return it dynamically. | T5 — partial evidence; acceptance open |
| US-04 — Discount approval (bonus) | Teilweise implementierte Datengrundlage; Workflow offen | Custom Table `Discount Request` samt N:1-Beziehung, Formular, Ansicht und App-Datensatz ist verifiziert. | Rabattgenehmigung über 10 %, Protokollierung des Ergebnisses auf der Opportunity und Teams-Benachrichtigung sind nicht implementiert/verifiziert. | T6 — Bonus, offen |
| US-05 — Regional permissions | UNVERIFIED | Keine regionale Zugriffsdurchsetzung oder Benutzerkontext-Abnahme dokumentiert. | Entra-/Dataverse-Berechtigungen, Ausführung als angemeldeter Benutzer und Ablehnung eines nicht erlaubten Region-Nord-Zugriffs müssen getestet werden. | T7 — offen |

**Kanalabhängigkeit:** Der Agent ist in Microsoft Teams veröffentlicht und installiert, aber Teams-Anfragen scheitern derzeit mit einem Usage-Limit-Fehler. Der Copilot Studio-Testchat funktioniert. In `CRM738378` sind 1.000 Legacy Sessions zugewiesen; Copilot Credits und ein Billing Plan wurden nicht gefunden. Lizenz/Kapazität ist eine vermutete, nicht bestätigte Ursache. Die Antwort von Dominic Bley auf die Anfrage vom 2026-10-15 steht aus; seine automatische Antwort nennt den 2026-10-18 als Rückkehrdatum. Sobald eine Antwort vorliegt, Guidance auswerten und Teams erneut testen. Keine kostenpflichtige Abrechnung ohne ausdrückliche Genehmigung aktivieren. Dieser Blocker bleibt dokumentiert und ändert nicht die aktive Story-Reihenfolge.

Der vorhandene Agent und die verifizierte Dataverse-Verbindung sind abgeschlossene Grundlagen, keine neuen Implementierungsaufgaben. Die agentengesteuerte Account-Erstellung bleibt ein begrenzter Funktionsnachweis und erfüllt nicht die bestätigungsgesteuerte Opportunity-Erstellung.

Die Umsetzung folgt der Architektur der SmartPoint-Aufgabenquelle. Geeignete vorhandene Dataverse-Tabellen werden genutzt; wo Sales-Komponenten fehlen, sind einfache Custom Tables mit wenigen Spalten, Beziehungen und Formularen ausdrücklich zulässig. Dynamics 365 Sales wird nicht nachgebaut.

**Nach dem MVP offen:** Verbleibende Pflichtanforderungen und -tests, insbesondere vollständige Quote-/Activity-Abdeckung, Entra-/Dataverse-Berechtigungsnachweise, regionale Zugriffsbeschränkung und T7. Bonusumfang (US-04, Sales Hub, zusätzliche Clarification Skills und erweitertes ALM) bleibt ebenfalls offen. Nichts davon gilt durch die MVP-Abnahme als erledigt.

## Umfang und aktueller Status

| ID | Pflichtanforderung | Status | Abnahmekriterium |
| --- | --- | --- | --- |
| M-01 | Dataverse-Kontext mit Kunden, Opportunities, Quotes und Activities | VERIFIED FOR S01 / T1 FUNCTIONAL ACCEPTANCE | T1 returned four open opportunities for `Huber Logistik` with sales stage, estimated revenue, expected close date, an open sales quote, and recent customer activity. Sales stage is returned as a numeric Choice value; currency is not reliably verified; sub-10-second end-to-end response time is not verified. |
| M-02 | Mindestens eine eigene Dataverse-Tabelle mit Beziehungen | VERIFIZIERT — MVP-Dataverse-Grundlage | `Discount Request` (`alp_rabattanfrage`) ist als Custom Table mit N:1-Beziehung zu Sales Opportunity konfiguriert; Formular, Ansicht und verknüpfter Datensatz wurden in der laufenden App geprüft. |
| M-03 | Kundensuche, einschließlich mehrdeutiger Namen | T2 VERIFIED; broader criterion partially verified | An ambiguous `Huber` lookup prompted clarification between `Huber Logistik` and `Huber Maschinenbau GmbH`. Lookup by customer number was not separately verified. |
| M-04 | SharePoint-Wissen aus 3–5 Dokumenten mit Quellenangaben | COMPLETED / VERIFIZIERT (2026-10-15) | Der Agent ruft alle vier Alpentec-DOCX-Dokumente ab und liefert Dokumentquellen. Die erneute Prüfung nach Entfernung der temporären Test-Wissensquelle war erfolgreich. |
| M-05 | Opportunity creation with confirmation and record link | CORE CREATE / CONFIRMATION / READ-BACK VERIFIED; LINK AND FULL T5 ACCEPTANCE OPEN | Manual and agent-triggered flow runs succeeded after one confirmation. The flow returned `opportunityId`; the agent displayed its GUID. The created Opportunity was independently found and opened in `SmartPoint Sales Demo`; its ID matches the returned ID and verified record URL. The flow does not dynamically return that URL as a clickable link in the agent response, so the record-link acceptance criterion remains open. The exact T5 scenario is also unverified. See [Opportunity flow notes](SALES_OPPORTUNITY_FLOW.md). |
| M-06 | Entra ID-Authentifizierung und wirksame Dataverse-Berechtigungen | UNVERIFIZIERT | Identität und Ausführungsmodus sind dokumentiert; erlaubte und verweigerte Dataverse-Abfragen/-Aktionen werden mit den vorgesehenen Rollen getestet. Verfügbarkeit von Tools allein beweist keine Zugriffsdurchsetzung. |
| M-07 | Benutzerkontext und regionale Einschränkung | UNVERIFIZIERT | Benutzer `Markus` erhält Daten aus DACH-Süd und keinen Zugriff auf einen Kunden aus Region Nord; Abfragen erfolgen nachweislich im Benutzerkontext, nicht über einen Service-Account. |
| M-08 | Bereitstellung und Nutzung in Microsoft Teams | BLOCKED — PUBLISHED/INSTALLED; USAGE-LIMIT ERROR | `SmartPoint Sales Assistant` is published and installed in Teams, but Teams requests fail with a usage-limit error. The Copilot Studio test chat works. `CRM738378` has 1,000 allocated legacy sessions; no Copilot Credits or billing plan were found. Licensing/capacity is a suspected, not confirmed, cause. An inquiry was emailed to Dominic Bley on 2026-10-15; his automatic reply says he is available again from 2026-10-18. |
| M-09 | Demo scenarios T1–T7, mandatory cases prioritized | IN PROGRESS — T1–T4 PASS; T5–T7 PENDING | Scenarios from the source are recorded in the test matrix with input, expected behavior, observed result, and evidence. T1–T5 and T7 take priority over bonus T6. |
| M-10 | Architektur-, Sicherheits- und Demo-Dokumentation | PENDING | Flow-Notizen und einzelne Verifikationsnachweise existieren, ersetzen aber kein vollständiges Lösungskonzept. Datenmodell, Entscheidung Topics vs. generative Orchestrierung, Identität/Berechtigungen, Blocker, Grenzen und reproduzierbarer Demoablauf müssen zusammenhängend dokumentiert werden. Keine unverifizierten Fähigkeiten werden als abgeschlossen beschrieben. |

Die Agenten-Tools **Dataverse Add Row** und **Dataverse List Rows** sind vorhanden; die Dataverse-Verbindung in `CRM738378` ist verifiziert. Agentengesteuerte Account-Erstellung wurde in Dataverse verifiziert. Das ist ein begrenzter Nachweis für die Account-Aktion; Account-Erstellung ist nicht die zentrale Pflicht-User-Story und ersetzt weder die Opportunity-User-Story US-03 noch den Nachweis eines bestätigungsgesteuerten Schreibvorgangs.

**Dataverse-Lesezugriff (2026-10-14): VERIFIZIERT für die dokumentierten Datensätze.** Das bestehende Tool `Accounts List Rows` rief `Huber Logistik` ab. Zusätzlich wurde das separate schreibgeschützte Tool `Get Sales Opportunities` konfiguriert; es rief `Logistics Fleet Modernization` aus der Tabelle `Sales Opportunities` ab. Die Opportunity verweist über Account ID `177a9152-afc7-f111-a05a-000d3a0a2928` auf Huber Logistik; Opportunity ID: `5aa34222-b0c7-f111-a05a-000d3a0a2928`. Im kombinierten Test beantwortete der Agent die geschäftlich formulierte Anfrage „Was läuft aktuell bei Huber Logistik? Bitte gib mir einen Überblick über den Kunden und seine laufenden Verkaufschancen.“ korrekt mit Kunden- und Opportunity-Daten und bestätigte deren Dataverse-Beziehung. Die abschließende Ausführungsspur wurde nicht separat darauf geprüft, ob beide Tools automatisch ausgewählt wurden; genau diese Orchestrierungsbehauptung gilt daher nicht als verifiziert.

Die dokumentierte MVP-Dataverse-Grundlage umfasst zugängliche Account-Daten sowie Custom Tables `Sales Opportunity` und `Sales Quote` mit gespeicherten, in der veröffentlichten `SmartPoint Sales Demo`-App angezeigten Testdatensätzen. Die separate Lösung `SmartPoint Copilot Sales Case` enthält die Custom Table `Discount Request`; sie ist mit N:1-Beziehung, Hauptformular, `Active Discount Requests`-Ansicht und verknüpftem Testdatensatz verifiziert und in derselben Umgebung in die `SmartPoint Sales Demo`-App eingebunden. Die beiden Lösungen bleiben getrennt. Diese App- und Datensatzprüfungen belegen noch keine Abfrage durch den Copilot-Agenten.

Das Topic **Create Customer** fragt `customerName` ab und zeigt eine Bestätigungsnachricht. Diese Nachricht wurde als funktionierend bestätigt. Die explizite Topic-Aktion **Add Row** wird durch den Fehler `Input binding 'item.name' was not found.` blockiert. Der Fehler wurde dokumentiert und wird in dieser Phase nicht erneut ausgelöst. Weder die Nachricht noch der vorhandene Agent-Account-Test belegen, dass diese Topic-Aktion nach Bestätigung einen Datensatz schreibt.

## Demo-Testmatrix aus der Aufgabenquelle

| Test | Pflicht-/Bonus | Szenario und erwartetes Verhalten | Aktueller Nachweis |
| --- | --- | --- | --- |
| T1 / US-01 | Pflicht | „Was läuft gerade bei Huber Logistik?“ - offene Opportunities mit Phase und Umsatz, offene Quotes und letzte Activity anzeigen. Zielantwortzeit unter 10 Sekunden. | PASS — vier offene Opportunities, sales stage, estimated revenue, expected close date, ein offenes Sales Quote und aktuelle Kundenaktivität wurden funktional bestätigt. Nicht-blockierende Einschränkungen: Sales stage wird als numerischer Choice-Wert statt lesbarer Bezeichnung ausgegeben; Währung ist nicht zuverlässig verifiziert und darf nicht als EUR angenommen werden; End-to-end-Antwortzeit unter 10 Sekunden ist nicht verifiziert. |
| T2 / US-01 | Pflicht | Anfrage zu „Huber“, wenn zwei Kunden passen - Agent fragt, welcher Account gemeint ist. | PASS — der Agent klärte zwischen `Huber Logistik` und `Huber Maschinenbau GmbH`. |
| T3 / US-02 | Mandatory | Compare AX-200 and AX-300 using product information and provide source links. | PASS (2026-10-16) — Copilot Studio test evidence showed the comparison with SharePoint citations. |
| T4 / US-02 | Mandatory | When asked about Italian VAT, state clearly that the company knowledge base has no information; do not invent an answer. | PASS after correction (2026-10-16) — the initial response used general model knowledge despite web search being disabled. After correction, the agent stated the information was unavailable in the company knowledge base. No broader fix is claimed. |
| T5 / US-03 | Pflicht | Opportunity für Huber Logistik, drei AX-200, ca. 450.000 Euro; fehlendes Abschlussdatum erfragen, Zusammenfassung anzeigen, Bestätigung einholen und nach Erstellung Link zurückgeben. | PARTIAL — ein anderes bestätigungsgesteuertes Opportunity-Beispiel, Flow-Erstellung, zurückgegebene ID und unabhängiger Read-back sind verifiziert. Das genaue Szenario, die Klärung des fehlenden Abschlussdatums und der dynamische klickbare Datensatzlink sind offen. Der separate `item.name`-Fehler betrifft `Create Customer`, nicht diesen Opportunity-Flow. |
| T6 / US-04 | Bonus | 15-%-Rabattanfrage für die vorherige Opportunity erfassen, Genehmigung an Vertriebsleitung auslösen und Teams-Benachrichtigung senden. | BONUS / UNVERIFIZIERT |
| T7 / US-05 | Pflicht | `Markus` fragt nach einem Kunden in Region Nord - keine Kundendaten ausgeben und auf fehlende Berechtigung hinweisen. | UNVERIFIZIERT |

## Dataverse-Modell: verifizierte MVP-Grundlage und verbleibende Anforderungen

Dominic hat bestätigt, dass der Tenant möglicherweise unvollständig provisioniert ist. Einfache eigene Dataverse-Tabellen mit wenigen Spalten und einfachen Formularen sind ausdrücklich zulässig; Dynamics 365 Sales muss nicht nachgebaut werden. Geeignete vorhandene Tabellen werden für das MVP wiederverwendet; fehlende Sales-Komponenten werden nur soweit erforderlich durch schlanke Custom Tables abgedeckt. Eine allgemeine Environment- oder Verbindungsprüfung ist keine neue Aufgabe.

| Tabelle / Rolle | Dokumentierte Felder / Status | Beziehungen / Zweck |
| --- | --- | --- |
| `Account` / Demo Customer | `Huber Logistik` ist als Account-Datensatz zugänglich; vollständiges Feldschema und Kundennummernfeld sind nicht dokumentiert. T2 funktional verifiziert: die mehrdeutige Suche nach `Huber` fragt zwischen `Huber Logistik` und `Huber Maschinenbau GmbH` nach. | Lookup-Metadaten zu den Opportunity- und Quote-Beziehungen sind nicht dokumentiert. |
| `Sales Opportunity` / Demo Opportunity (Custom Table) | T1 funktional bestätigte vier offene Opportunities für Huber Logistik mit Sales Stage, Estimated Revenue und Expected Close Date. Beispiel `Logistics Fleet Modernization`: Estimated Revenue `85000 USD`, Estimated Close Date `2026-12-31`, Sales Stage `New`, Probability `40`. Feldtypen/logische Namen sind nicht dokumentiert; T1-Antwort gibt Sales Stage als numerischen Choice-Wert aus. | T1-Antwort verknüpft die Opportunities mit Huber Logistik; genaue Beziehungskonfiguration nicht dokumentiert. |
| `Sales Quote` (Custom Table) | T1 funktional bestätigte ein offenes Sales Quote. Zusätzlich werden `QUO-2026-001` und `QUO-2026-002` in der App angezeigt; für `QUO-2026-002`: Quote Name, Total Amount `85000 USD`, Quote Date `2026-10-14`, Quote Status `Draft`. | `QUO-2026-002` ist mit `Logistics Fleet Modernization` verknüpft; Beziehungskardinalität/-metadaten nicht dokumentiert. Die T1-Antwortwährung wurde nicht zuverlässig verifiziert und darf nicht als EUR angenommen werden. |
| `Activity` / Demo Activity | T1 funktional bestätigte eine aktuelle Kundenaktivität. Die konkrete Tabelle, Felder und Aktivitätsdatensatzdetails sind nicht dokumentiert; `Subject`, `Activity Date` und `Type` bleiben nur Vorschlagsfelder, keine bestätigte Schemaaufnahme. | T1-Antwort ordnete aktuelle Aktivität dem Kundenüberblick zu; zugrunde liegende Beziehungsmetadaten sind nicht dokumentiert. |
| `Discount Request` (`alp_rabattanfrage`, Custom Table) | VERIFIZIERT: `Name`, `Discount Percentage`, `Reason`, `Approval Status` (`Pending`, `Approved`, `Rejected`). | VERIFIZIERTE N:1-Beziehung zu `Sales Opportunity`; Hauptformular, `Active Discount Requests`-Ansicht und verknüpfter Testdatensatz. Rabattgenehmigungsworkflow US-04 bleibt Bonus und offen. |

Die Tabelle trennt funktional verifizierte S01-Antworten von nicht dokumentierten Schema-Metadaten. Die T1-/T2-Szenarien sind funktional bestanden; die zugrunde liegenden Feld-/Beziehungsdefinitionen und einige Datensatzdetails sind weiterhin nicht dokumentiert. Verifizierte Tabellen, Formulare, Beziehungen und Testdaten werden nicht erneut erstellt.

## MVP-Umsetzungsfolge

Die Zeitangaben für offene Schritte sind geschätzte verbleibende aktive Arbeitszeit; für Schritt 1 ist die ursprüngliche Schätzung historisch. Der vorhandene Agent, die verifizierte Dataverse-Verbindung und die dokumentierte MVP-Dataverse-Grundlage sind abgeschlossene Grundlagen und werden nicht erneut implementiert oder geprüft.

| Schritt / Status | Ziel | Ergebnis | Abnahmekriterien | Lernwert | Aufwandsschätzung / Restaufwand |
| --- | --- | --- | --- | --- | --- |
| 1 - COMPLETED / MVP-DATAVERSE-GRUNDLAGE VERIFIZIERT | Minimales Dataverse-Modell mit Beziehungen, Formularen und Testdaten bereitstellen. | In der veröffentlichten App sind Account-Daten zugänglich; Custom Sales Opportunity und Sales Quote-Listen/Formulare sowie verknüpfte Testdatensätze funktionieren. Discount Request erfüllt mit Custom Table, N:1-Beziehung, Formular, Ansicht und Testdatensatz die MVP-Anforderung an eine eigene relationale Tabelle. | Verifizierte Dataverse-Grundlage für MVP-Lese- und Schreibpfade vorhanden. Dies schließt weder vollständige Schema-Metadaten noch weitergehende Full-Case-Anforderungen ein. | Dataverse-Modellierung, Lookups, Beziehungen und Low-Code-Formulare. | 45–75 min (ursprüngliche Schätzung) |
| 2 - COMPLETED / DATAVERSE-LESEZUGRIFF VERIFIZIERT | Kunden- und Opportunity-Daten über die Agenten-Tools lesen. | `Accounts List Rows` ruft `Huber Logistik` ab; das schreibgeschützte Tool `Get Sales Opportunities` ruft `Logistics Fleet Modernization` ab. Eine kombinierte geschäftliche Anfrage erhielt eine korrekte Antwort mit der Dataverse-Beziehung. | Beide dokumentierten Datensätze wurden erfolgreich gelesen und in einer kombinierten Antwort korrekt dargestellt. Die automatische Auswahl beider Tools wurde in der finalen Ausführungsspur nicht separat verifiziert. Disambiguierung unklarer Kundennamen bleibt M-03/T2 offen. | Dataverse-Abfragen, Agent-Kontext und verknüpfte Geschäftsdaten. | 30–45 min (ursprüngliche Schätzung) |
| 3 - COMPLETED / SHAREPOINT-ABRUF VERIFIZIERT (2026-10-15) | SharePoint-Wissen für Produktfragen integrieren und den Abruf verifizieren. | Der Agent ruft über die finale Wissensquelle `Alpentec Sales Knowledge` alle vier Alpentec-DOCX-Dokumente ab und liefert Dokumentquellen. Die temporäre Wissensquelle `Alpentec Sales Documents Test` wurde entfernt; ein Wiederholungstest rief weiterhin alle vier Dokumente ab. | Abruf aller vier Dokumente und Ausgabe von Dokumentquellen wurden erfolgreich getestet. Die manuelle Neuindizierung ging dem erfolgreichen Abruf voraus; ein ursächlicher Zusammenhang ist nicht abschließend belegt. | Grounding, Wissensquellen und Quellenangaben. | 30–60 min (abgeschlossen) |
| 4 - CORE CREATE / CONFIRMATION / READ-BACK VERIFIED; FULL T5 AND RECORD LINK OPEN | Create Opportunities through a controlled flow after confirmation. | `Create Sales Opportunity via Flow` calls `Create Sales Opportunity`; the flow creates the Opportunity and returns `opportunityId`. The agent confirms once and reports success. The record was independently opened in `SmartPoint Sales Demo`. | Manual and agent-triggered executions, confirmation, returned ID, and independent app read-back were verified on 2026-10-15. Full T5 acceptance and a dynamically returned clickable `opportunityUrl` remain open. Do not retry the separate documented `item.name` failure path. | Controlled writes, confirmation, and flow integration. | Core path completed; remaining work follows US-03 story acceptance. |
| 5 - BLOCKED / PUBLISHED AND INSTALLED; AWAITING SMARTPOINT RESPONSE | Verify successful use of the published agent in Microsoft Teams after evaluating SmartPoint's licensing/capacity guidance. | The agent is published and installed; the Copilot Studio test chat works, but Teams requests fail with a usage-limit error. `CRM738378` has 1,000 allocated legacy sessions; no Copilot Credits or billing plan were found. Licensing/capacity is a suspected, not confirmed, cause. An inquiry was emailed to Dominic Bley on 2026-10-15; his automatic reply says he is available again from 2026-10-18. | Evaluate Dominic Bley's response, retest a representative Teams interaction, and record the result. Do not enable paid billing without explicit approval. | Channel behavior and licensing/capacity diagnosis without assuming a root cause. | 30–45 min after the blocker is addressed |
| 6 - PENDING / FOLLOWING MILESTONE | Demonstrate and verify the MVP end to end. | Reproducible journey from Dataverse reads and SharePoint answer through confirmed Opportunity creation, returned identifier/link, and read-back in Teams. | Test all five MVP acceptance points in Teams and document results; keep untested capabilities marked open. | End-to-end testing, reproducible demo, and verification. | 30–45 min |

## Zeitplan

Die folgenden ursprünglichen Zieltermine werden als Planhistorie erhalten. Erreichbarkeit/Termine werden anhand des tatsächlichen Fortschritts neu bewertet; ein Datum ist kein Erledigungsnachweis.

| Zieltermin (2026) | Ursprünglicher Fokus |
| --- | --- |
| 13. Oktober | Umgebung, Demodaten, Lizenzen und Agent prüfen; erste Dataverse-Abfrage. |
| 14. Oktober | Kundenübersicht, SharePoint und Tests T1–T4. |
| 15. Oktober | Opportunity-Erstellung, Sicherheit und Teams. |
| 16. Oktober vor 13:00 | Verifizierten Status und E-Mail an Dominic. |
| 17. Oktober | Verbleibende Pflichtanforderungen, danach optionale Bonusarbeit. |
| 18. Oktober | Demo-Abnahme und Dokumentation. |

## Entscheidungen und Schutzregeln

- Dominic hält eine unvollständige Tenant-Provisionierung für möglich und hat einfache Custom Tables mit wenigen Spalten und Formularen als akzeptable Lösung bestätigt. Dynamics 365 Sales wird nicht nachgebaut.
- Geeignete vorhandene Dataverse-Tabellen werden wiederverwendet; wo Sales-Komponenten fehlen, sind einfache Custom Tables mit wenigen Spalten, Beziehungen und Formularen zulässig. Die zuvor bestätigten Environment-/Verbindungsprüfungen werden nicht wiederholt.
- Account-Erstellung wurde agentengesteuert verifiziert, ist aber nicht die zentrale Pflicht-User-Story. Sie belegt nicht die bestätigungsgesteuerte Create-Customer-Topic-Aktion und nicht US-03.
- Den Fehler `Input binding 'item.name' was not found.` nicht erneut auslösen. Der MVP verwendet einen kontrollierten Flow-Pfad; der bekannte Fehler wird dabei nicht erneut ausgeführt.
- Zugangsdaten, Tokens und Secrets gehören nicht in Markdown, Git oder Screenshots. Die E-Mail-PDF enthält Zugangsinformationen; PDF nicht einchecken und sensible Inhalte daraus nicht weitergeben.
- Plan und Fortschritt werden nach jedem Arbeitspaket aktualisiert. Keine anderen Cases oder Umgebung `CRM816895` ändern.

## SharePoint-Wissensintegration: Nachweis und offene Folgepunkte

**Umgebung:** `CRM738378`; **Agent:** `SmartPoint Sales Assistant`; **finale Wissensquelle:** `Alpentec Sales Knowledge`.

- **Site:** https://crm738378.sharepoint.com/sites/SmartPointSalesKnowledge
- **Dokumentbibliothek:** https://crm738378.sharepoint.com/sites/SmartPointSalesKnowledge/Shared%20Documents/ . Die Site-URL stammt von der tatsächlichen SharePoint-Site, die Bibliotheks-URL aus der Browseradresse der vorhandenen `Shared Documents`-Bibliothek. Die Dokumentlinks wurden anhand der Quellenverweise des Agenten geprüft; diese tenant-spezifischen URLs stammen nicht aus Microsoft-Dokumentation.
- Der Agent ruft alle vier hochgeladenen Alpentec-DOCX-Dokumente ab und gibt Dokumentquellen an. Nach Entfernung der temporären Wissensquelle `Alpentec Sales Documents Test` lieferte ein Wiederholungstest erneut alle vier Dokumente.
- Bei der anfänglichen Fehlersuche waren Suchsichtbarkeit und automatische Indexverwaltung aktiviert. Die Bibliothek wurde über **Library Settings → More Library Settings → Advanced Settings → Reindex Document Library** mit dem Grund `Library content missing from search results` zur Neuindizierung eingereicht. Der erfolgreiche Abruf wurde danach beobachtet; die Neuindizierung ist eine vermutete, aber nicht abschließend nachgewiesene Ursache der Verbesserung.
- **Optionale technische Validierung (nicht blockierend):** Ein fünftes Testdokument hochladen und prüfen, ob es ohne manuelle Neuindizierung auffindbar wird.
- **Präsentationsreife:** Die SharePoint-Oberfläche zeigt derzeit Deutsch und Niederländisch gemischt, obwohl die Microsoft-365-Benutzereinstellungen Deutsch anzeigen. Site-Sprache, Regionseinstellungen und Mehrsprachigkeit untersuchen und eine konsistente deutsche UI vor einer kundengerichteten Präsentation herstellen. Spracheinstellungen sind im Rahmen dieses Dokumentationsupdates nicht geändert.

## S02 — Product Knowledge: T3/T4 results

**Status:** Both tests passed in Copilot Studio test chat on 2026-10-16. The final source `Alpentec Sales Knowledge` contains four uploaded Alpentec DOCX documents and returns source references. T3 evidence supports the AX-200/AX-300 comparison with SharePoint citations. For T4, the initial answer used general model knowledge despite web search being disabled; after correction, the agent stated that Italian VAT information was unavailable in the company knowledge base. Do not claim a permanent correction beyond the observed response. Teams remains blocked by the documented usage-limit error; this does not invalidate the test-chat results.

| Test | Exact requirement from the case | Expected behavior | Evidence to capture at execution |
| --- | --- | --- | --- |
| T3 / US-02 | Compare Alpentec AX-200 and AX-300 using product information and provide source links. | PASS — the comparison used the SharePoint knowledge source and included citations. | Copilot Studio test evidence: comparison and SharePoint citations. |
| T4 / US-02 | When asked about Italian VAT, clearly state that the knowledge base has no information; do not invent an answer. | PASS after correction — the final response stated that Italian VAT information was unavailable in the company knowledge base. The initial response used general model knowledge despite web search being disabled. | Copilot Studio test evidence: initial response observation and corrected answer. No separate persistent-fix verification is claimed. |

**Approved presentation artifacts:** The user approved the German version 3 reports `SmartPoint_US01_Customer_Overview_Technical_Report_DE_v3.pdf` and `SmartPoint_US02_Product_Knowledge_Technical_Report_DE_v3.pdf`. Neither exact file was found locally in the repository. No PDF was copied or substituted; add these approved originals only if made locally accessible. Do not regenerate, translate, or edit them.

## US-03 / S03 preparation — next story

**Original user story:** “As an inside sales employee, I want to create an Opportunity by chat so that sales opportunities are entered into the system immediately.”

**Exact mandatory T5 scenario:** “Create an Opportunity: Huber Logistik wants three AX-200, approximately EUR 450,000.” The agent must ask for the missing expected close date, show a summary, obtain confirmation, create the record, and return a usable link to the Opportunity. This is the original acceptance scope; do not substitute the already verified AX-300 scenario.

**Existing foundation and resources:** `SmartPoint Sales Assistant` in `CRM738378`; existing Account lookup and Dataverse read tools; SharePoint source `Alpentec Sales Knowledge` with four Alpentec documents; agent tool `Create Sales Opportunity via Flow`; cloud flow `Create Sales Opportunity`; custom Dataverse table `Sales Opportunity`; and published `SmartPoint Sales Demo` app with a verified record URL pattern. The flow currently accepts only `opportunityName` and `accountId`. Its observed `opportunityId` output was an OData record URL, from which the agent displayed the GUID. It does not yet return a dynamically constructed clickable `opportunityUrl`.

**Open T5 requirements:** Collect customer, topic/product, estimated revenue, and expected close date; ask for missing information; show the complete proposed Opportunity summary; require explicit confirmation before writing; create the Opportunity through the controlled flow; return a clickable link that opens the newly created row. The exact T5 scenario, missing-date question, and link return have not been verified. Keep the separate `Create Customer` error `Input binding 'item.name' was not found.` documented and do not retry that path.

**Preparation and implementation/test sequence:** (1) Inspect the existing flow trigger, create-row mappings, agent tool inputs, and response outputs against T5. (2) Plan the smallest additions needed for product/topic, revenue, close date, and dynamic URL using existing table/app metadata; do not assume a URL expression before confirming its inputs. (3) Configure the agent to ask for the missing close date and present all required values in one summary. (4) Retain one explicit confirmation before the write and avoid duplicate prompts. (5) Return the created row's usable app link to the agent after success. (6) Only after implementation, run the exact T5 prompt in Copilot Studio test chat: verify the close-date clarification, summary, one confirmation, successful create, returned link opening the created record, and independent read-back. Capture the conversation, flow run, and opened record. None of these S03 steps are being implemented or tested in this update.

**Prerequisites and dependencies:** The working Copilot Studio test chat and current Dataverse/SharePoint foundations are available. Teams remains blocked by a usage-limit error; licensing/capacity is suspected, not confirmed, and paid billing must not be enabled without explicit approval. The Teams blocker affects full MVP acceptance but does not prevent S03 preparation or test-chat development. Complete the S02 presentation gate before S03 implementation/testing. No additional table or Sales package is currently identified as necessary. **Planning estimate for remaining S03 implementation and T5 verification: approximately 45–75 minutes**, assuming the existing flow and metadata remain available; this is an estimate, not a case requirement.

**Exactly one next project action:** Begin the S03 / US-03 Plan stage by inspecting the existing `Create Sales Opportunity` flow and mapping its current inputs, Dataverse fields, and response against the original T5 acceptance criteria. Do not implement or test S03 in this preparation update. S01's limitations remain non-blocking and unchanged; S02 T3/T4 are PASS; Teams remains BLOCKED pending SmartPoint guidance.
