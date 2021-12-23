# Open³Toolbox
## Beschreibung
Im Zuge des Projekts wurden 2 unterschiedliche Platinen erstellt. Hierbei wurde besonders darauf geachtet, dass eine große Anzahl verschiedener Komponenten integriert werden. Eine der Platinen ist kompatibel mit einem Arduino UNO und wurde als Shield designed. Die andere Platine wurde so entwickelt, dass ein esp32 aufgesteckt werden kann. Insgesamt beinhalten die Platinen eine vielzahl verschiedener Sensoren: GPS, Luftqualität, Temeratur, Luftfeuchtigkeit, Druck und Flammensensor.

Beide Platinen wurden mithilfe von Eagle gezeichnet und können auch damit geöffnet und weiter bearbeitet werden. Auf diesem Wege können zum Beispiel weitere Sensoren hinzugefügt werden. Um die Platinen zu fertigen müssen noch mithilfe von Eagle Gerber-Datein erzeugt werden. Eine detailierte Beschreibung wie dies Funktioniert: https://support.jlcpcb.com/article/137-how-to-generate-gerber-and-drill-files-in-autodesk-eagle
Eine detailierte Beschreibung wie die einzelnen Komponenten mit den Mikrocontrollern verbunden sind ist unter [[Opentoolbox#Open³Toolbox|Hardware]] näher beschrieben. 

ESP32 und Copernicus2-GPS kommunizieren mittels Serieller UART-Schnittstelle, während sowohl der Luftqualizätssensor als auch der Temperatur/Feuchtigkeitssensor mittels I2C angebunden sind.
Bei der Platine für den Arduino wurde hauptsächlich auf eine Analoge kommunikation zurückgegriffen. Sowohl Druck als auch Flammensensor kommunizieren mittels analoger signale. Die Leds werden jedoch mithilfe der digitalen Ausgänge des Arduino ein und ausgeschaltet.

## Komponenten Dokumentation

| Komponente              | Herstellernummer |                                       Dokumentation                                       |
|-------------------------|------------------|:-----------------------------------------------------------------------------------------:|
|  Copernicus2-GPS        | GPS-10922        |   http://cdn.sparkfun.com/datasheets/Sensors/GPS/63530-10_Rev-B_Manual_Copernicus-II.pdf  |
| Luftqualität            | ZMOD4410AI3V     |           https://www.renesas.com/eu/en/document/dst/zmod4410-datasheet?r=454426          |
| Temperatur/Feuchtigkeit | HS3003           |    https://www.mouser.at/datasheet/2/698/REN_HS300x_Datasheet_DST_20210809-1997723.pdf    |
| Infineon Drucksensor    | KP212K1409XTMA1  | https://www.mouser.at/datasheet/2/196/Infineon_KP212K1409_DataSheet_v01_00_EN-1921364.pdf |
| Flammensensor           | DEBO FLAME SENS  |                https://cdn-reichelt.de/documents/datenblatt/A300/SE033.pdf                |

<div style="page-break-after: always;"></div>

## Hardware
### Esp Board
#### PCB Layout
![[esppcb.png|1000]](Images/esppcb.png)

<div style="page-break-after: always;"></div>

#### Schematics
**ESP32**

![[esp.png]](Images/esp.png)


**Copernicus Gps**

![[copernicus.png]](Images/copernicus.png)


**ZMOD Luft Qualität**

![[zmod.png]](Images/zmod.png)

**HS3003 Temperaur/Feuchtigkeit**

![[hs3003.png]](Images/hs3003.png)

<div style="page-break-after: always;"></div>

#### Komponenten
**ESP32**

**Copernicus2 GpsModul**
- Uart Pins: ESP25 => CopernicusTX|ESP26 => CopernicusRX
- Antenne via SMA Connector

**ZMOD4410AI3V Luftqualität**
- I2c: Addr: 0x32
- INT: ESP32
- RESET(activeLow): ESP27

**HS3003 Temperatur/Feuchtigkeit**
- i2c: Addr: 0x44
- Berechnung:
![[formula.png]](Images/formula.png)

### Arduino Board
#### PCB Layout

![[arduinopcb.png]](Images/arduinopcb.png)
<div style="page-break-after: always;"></div>

#### Schematics
**Arduino**

![[arduino.png]](Images/arduino.png)

**Infineon Drucksensor**

![[Infineon.png]](Images/infineondruck.png)


**LEDs**

![[leds.png]](Images/leds.png)

**Flammensensor**

![[flammen.png]](Images/flamesensor.png)


#### Komponenten
**Arduino**

**Infineon Drucksensor**
- AnalogOut => A0 Arduino

**Leds**
- Red => D2 Arduino
- Green => D0 Arduino
- Blue => D1 Arduino

**Flammensensor**
- AnalogOut => A1 Arduino
