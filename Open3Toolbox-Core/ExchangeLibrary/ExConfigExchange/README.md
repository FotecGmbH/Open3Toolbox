# ExConfigExchange

## TODO:
* SelfReference-s können zur StackOverflow führen, das muss möglicherweise in der Zukunft gelöst werden.

## Zusammenfassung:
Mit diesem Bibliothek kann man ausfühlbare Formulare generieren aus Klassen, ohne dass, der Client diese Klassen und deren Assemblies kennen müsste.
Mit den Attributen in **Annotations** können die Fields, Properties und ganze Klassen unterschiedlich konfiguriert werden. (zBs: mit der ValidRangeAttribute kann einen Wert berreich für numerische Werte gesetzt werden, die der Client validieren kann.)

## Anmerkungen:
Alles was mit der ConfigureAsAttribute zu tun hat, ist oft problematisch und braucht möglicherweise einen Rework in der Zukunft.  
**Models/Dummies** beinhaltet Dummyklassen für das testen von ExConfigItemManager.  
Der **Services/Interfaces/IExConfigVisitor** interface sollte für die Typeunterscheidung verwendet werden. (zBs: im Renderers, Converters usw.)
Alles immer Cachen, da komplette Reflection kann langsam sein.