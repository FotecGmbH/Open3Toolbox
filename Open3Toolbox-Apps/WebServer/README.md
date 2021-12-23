# WebServer

## TODO:
* Dummies in DcExPublisher �berschreiben (GetDcExUIds und StoreDcExPairs)
* Datenbank Fehlern besser behandeln (Aktor/Messung mit Id/Name kommt schon vor usw.)
* Immer erst alles nach Benutzer filtern
* Beschreibung/Description Property von Aktoren und Messungen in dem ExchangeLibrary addieren.
* Update Feature bei Wiederver�ffentlichung eines Projekts kann noch buggy sein, da es in der letzte Minute als einen Convinience Feature implementiert wurde.

## Zusammenfassung:
Der Webserver fungiert nur als eine Art Relay zwischen den Clients, Datenbank, Gateways und Sensoren.  
Alles befindet sich im **DataConnector/ConfigurationTool** und **DataConnector/Statistics**

## Anmerkungen:
Die Code Generatoren Funktionieren M�glicherweise nicht richtig.  
User- => Dinge die der Benutzer Beliebig �ndern kann.  
Published- => Dinge die schon ver�ffentlicht wurden.