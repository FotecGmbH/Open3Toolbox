# ExConfigExchange

## TODO:
* SelfReference-s k�nnen zur StackOverflow f�hren, das muss m�glicherweise in der Zukunft gel�st werden.

## Zusammenfassung:
Mit diesem Bibliothek kann man ausf�hlbare Formulare generieren aus Klassen, ohne dass, der Client diese Klassen und deren Assemblies kennen m�sste.
Mit den Attributen in **Annotations** k�nnen die Fields, Properties und ganze Klassen unterschiedlich konfiguriert werden. (zBs: mit der ValidRangeAttribute kann einen Wert berreich f�r numerische Werte gesetzt werden, die der Client validieren kann.)

## Anmerkungen:
Alles was mit der ConfigureAsAttribute zu tun hat, ist oft problematisch und braucht m�glicherweise einen Rework in der Zukunft.  
**Models/Dummies** beinhaltet Dummyklassen f�r das testen von ExConfigItemManager.  
Der **Services/Interfaces/IExConfigVisitor** interface sollte f�r die Typeunterscheidung verwendet werden. (zBs: im Renderers, Converters usw.)
Alles immer Cachen, da komplette Reflection kann langsam sein.