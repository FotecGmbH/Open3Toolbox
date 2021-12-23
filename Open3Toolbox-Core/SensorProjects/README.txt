LMIC Library:
https://github.com/mcci-catena/arduino-lmic

Gerät in TTS erstellt id: xxxxx45fb
ABP = direkt senden
von example: ttn-abp.ino
ttn-abpTRY1.ino
Uplink nachrichten werden gesendet, aber anscheinend keine downlinks
Nachrichten: Forward uplink data message mit "frm_payload": "SGVsbG8sIHdvcmxkIE1BVFQh" => base64 "Hello, world MATT!"
=>endlosschleife mit 
24050281: EV_TXSTART
Packet queued
24181722: EV_TXCOMPLETE (includes waiting for RX windows)
(normalerweise sollte dann der response vom gateway kommen)


Gerät in TTS erstellt id: xxxxx45e7
OTAA = zuerst wird ein join request gesendet, zurückgesendet ob er akzeptiert wird und danach nachrichten gesendet
von example: ttn-otaa.ino
ttn-otaaTRY1.ino
join-request kommt bei tts an und wird akzeptiert "Accept join-request", aber es kommt nichtmehr zum arduino zrück somit beginnt der nie nachrichten zu senden
=>endlosschleife mit 
70424: EV_TXSTART
464225: EV_JOIN_TXCOMPLETE: no JoinAccept


Sachen geändert:
in beiden files die keys reingeschrieben (achtung DEVEUI gehört mit lsb, also umgekehrte bytes reihenfolge)
APPEUI sind nur nulllen=> er schreibt zu jedem anstatt einem speziellem gerät
\Arduino\libraries\MCCI_LoRaWAN_LMIC_library\src\lmic\config.h       #define LMIC_PRINTF_TO Serial (auskommentiert weil bei forum probleme entstanden sind wenn kommentiert)
https://githubmemory.com/repo/ElectronicCats/BastWAN/activity
\Arduino\libraries\MCCI_LoRaWAN_LMIC_library\project_config\limic_project_config.h #define CFG_eu868 1 und #define CFG_sx1276_radio 1
https://www.thethingsnetwork.org/forum/t/uno-dragino-v1-3-failure-radio-c-659/3709/8
in beiden files const lmic_pinmap lmic_pins         geändert, passend für arduino uno mit dragino lora shield
in ABP //LMIC_setupChannel(3, 867100000, DR_RANGE_MAP(DR_SF12, DR_SF7),  BAND_CENTI); (ein paar channels auskommentiert weil anscheinend der zufällige gateway wo ich derzeit hinschicke nicht so viele channels unterstützt)
in OTAA   LMIC_disableChannel(5);                                                      ---------------||--------------
versucht zu verkabeln,  https://wiki.dragino.com/index.php?title=Lora_Shield  ändert sich aber nichts, außer dass er teilweise versucht (ich glaube)gps abzurufen
in OTAA LMIC_setClockError(MAX_CLOCK_ERROR * 10 / 100);  da einige foren schrieben dass timing probleme die ursache seien könnten
Beim ABP Gerät in TTS unter General Settings=>Network layer Expand=> Advanced MAC settings=>Resets Frame Counters habe ich enabled.......er zählt irgendwie immer +1 wenn er nachrichten schickt und durch das wird das verhindert, ansonsten hat er oft probleme und empfängt nur jede xte nachricht (sollte man aber nicht machen wenn released)

In den foren schreibens man soll zuerst ABP zum laufen bringen, weil OTAA das gleiche is nur dass ma zuerst diesen join macht
Gute dokumentation wie man die library verwendet hab ich nicht gfundn außer das pdf LMiC-v1.5.pdf 



















