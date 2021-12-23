# Web-Client

## TODO:
* Restrukturierung (IconProvider und BlazorUIVisitor sind kein Komponents lieber Renderers)
* BreadCrumbs in SensorStatistics und Co. Implementieren (Wenn man kein Id basierten navigierung braucht sollte jetzt einfach sein.)
* Sensor und GatewayConnectors stabilisieren (Zur Zeit der Implementierung war vieles unbekannt, deshalb, was da drinn gibt ist nur eine Temporäre Lösung)
* Notification System vom DataPoints Debuggen, Zur Zeit ist der Modified State oft manuel gesetzt (Ich könnte es nirgedwie zusammen bringen, dass manche Daten (zBs: Configuration) als modifiziert erkannt werden, wenn diese verändert wurden)
* Physical Ansicht Implementieren (Sehe Mockups)
* Import Feature Implementieren => Fragwürdig ob man das braucht, wegen Publish (Sehe Mockups)
* KonfigurationsTool vom SensorStatistics, also in 2 Apps trennen
* Telerik sachen entfernen (Lizenz Probleme)
* Project Publisher Fertigstellen (Zur Zeit der Implementierung war noch vieles unbekannt)
* ActorDetailsPage Fertigstellen (Zur Zeit war noch nichts bekannt und wurde als unwichtig eingestuft)
* Version Kontroll vom Projekten muss noch überlegt werden.

## Zusammenfassung
Momentan ist es so gedacht, dass eine Benutzer seinen Projekte Kofigurieren, die Gateways und Sensoren von einem ausgewählten Projekt verbinden und dann abschließend den Projekt Veröffentlichen kann.  
Wurde der Projekt veröffentlicht, kann der Benutzer dessen Aktoren und Messungen in dem SensorStatistics und Co. seine Logische Übersicht erzeugen (zBs: Haus, Garten usw.) und Aktoren bzw. Messungen zuweisen sowie die Messwerte von Messungen ansehen und Aktoren Befehlen Senden.