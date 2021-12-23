
/*
 Name:    ArduinoLora.ino
 Created: 8/30/2021 12:45:05 PM
 Author:  kevin
*/

#include <Wire.h>
#include <EEPROM.h>
#include <SPI.h>


uint16_t EEPROM_Pointer_Storage = 0;
uint16_t EEPROM_Pointer = 0;
uint16_t SENDBUF_Pointer = 0;
static uint8_t SENDBUF[500];
unsigned long delayStart;
uint16_t delayPeriod;
int sleepCounter = 0;
uint8_t INTERPRET_DOWNLINK = 0;
uint16_t EEPROM_TEMP_START = 400;
uint16_t EEPROM_TEMP_LENGTH = 400;
uint16_t PROGSTART_POINTER = 511;

uint8_t IncomingBytes[100];

uint8_t SensorId = 8; // hardcoded



void setup() {
  Serial.begin(9600); // opens serial port, sets data rate to 9600 bps
  EEPROM.update(510, 0);
  WRITE_EEPROM();
 
Wire.begin();

}

void loop() {
  uint8_t opcode;

  if (delayPeriod > 0 && delayStart > 0 && (delayStart + delayPeriod) > millis()) {
    opcode = 90;
    if ((sleepCounter++ % 10000) == 0) {

    }
  }
  else {
    delayPeriod = 0;
    delayStart = 0;
    opcode = EEPROM.read(EEPROM_Pointer++);
  }

  EXECUTE_OPCODE(opcode);
#pragma endregion
}

void EXECUTE_OPCODE(uint8_t opcode) {

  // WRITE_PROGSTART(C, D)
  if (opcode == 1) {

    uint8_t addrM = EEPROM.read(EEPROM_Pointer++);
    uint8_t addrL = EEPROM.read(EEPROM_Pointer++);
    uint16_t addr = (addrM << 8) | addrL;

    SET_PROGSTART(addr);
  }

  if (opcode == 2) {
    uint16_t PROGSTART = READ_PROGSTART();
    EEPROM_Pointer = PROGSTART;
  }

  //NOP
  if (opcode == 90) {
  }

  // SOFTDELAY(MS1, MS2) 
  if (opcode == 9) {
    sleepCounter = 0;
    uint8_t periodM = EEPROM.read(EEPROM_Pointer++);
    uint8_t periodL = EEPROM.read(EEPROM_Pointer++);
    uint16_t period = (periodM << 8) | periodL;

    delayPeriod = period;
    delayStart = millis();
  }

  // DELAY(MS) 
  if (opcode == 10) {
    uint8_t ms = EEPROM.read(EEPROM_Pointer++);

    delay(ms);
  }

  // SET_EEPROMPONTER(ADDR)
  if (opcode == 11) {
    uint8_t addrM = EEPROM.read(EEPROM_Pointer++);
    uint8_t addrL = EEPROM.read(EEPROM_Pointer++);
    uint16_t addr = (addrM << 8) | addrL;

    EEPROM_Pointer = addr;
  }

  // INCREASE_SEND_COUNTER()
  if (opcode == 12) {      
      uint8_t curr = EEPROM.read(510);
      EEPROM.update(510, (curr + 1));
  }

  // I2C.READ(C, D) C = addr, D = length
  if (opcode == 20) {
    uint8_t addr = EEPROM.read(EEPROM_Pointer++);
    uint8_t len = EEPROM.read(EEPROM_Pointer++);

    Wire.requestFrom(addr, len);
    


    for (size_t i = 0; i < len; i++)
    {
        if(Wire.available()){
            int val = Wire.read();// slave may send less than requested
            SENDBUF[SENDBUF_Pointer++] = val; // receive a byte as character
        }
        else{
            SENDBUF[SENDBUF_Pointer++] = 0; // receive a byte as character
        }
    }
  }

  // I2C.WRITEREAD(C, D, E) C = addr, D = writeByte, E = length
  if (opcode == 21) {
    uint8_t addr = EEPROM.read(EEPROM_Pointer++);
    uint8_t writeByte = EEPROM.read(EEPROM_Pointer++);
    uint8_t len = EEPROM.read(EEPROM_Pointer++);

    Wire.beginTransmission(addr);
    Wire.write(writeByte);
    Wire.endTransmission(false);

    Wire.requestFrom(addr, len);

    for (size_t i = 0; i < len; i++)
    {
        if(Wire.available()){
            int val = Wire.read();// slave may send less than requested
            SENDBUF[SENDBUF_Pointer++] = val; // receive a byte as character
        }
        else{
            SENDBUF[SENDBUF_Pointer++] = 0; // receive a byte as character
        }
    }

  }

  // I2C.WRITE(C, D)
  if (opcode == 25) {
    uint8_t addr = EEPROM.read(EEPROM_Pointer++);
    uint8_t byte = EEPROM.read(EEPROM_Pointer++);

    Wire.beginTransmission(addr);
    Wire.write(byte);
    Wire.endTransmission();
  }

  // I2C.WRITE2(C, D, E)
  if (opcode == 26) {
    uint8_t addr = EEPROM.read(EEPROM_Pointer++);
    uint8_t byte1 = EEPROM.read(EEPROM_Pointer++);
    uint8_t byte2 = EEPROM.read(EEPROM_Pointer++);
    uint8_t buf[] = { byte1, byte2 };

    Wire.beginTransmission(addr);
    Wire.write(buf, 2);
    Wire.endTransmission();
  }

  // GPIO.SET_PINMODE(C, D)  // MODE: INPUT = 0x0, OUTPUT = 0x1, INPUT_PULLUP 0x2
  if (opcode == 30) {
    uint8_t pin = EEPROM.read(EEPROM_Pointer++);
    uint8_t mode = EEPROM.read(EEPROM_Pointer++);

    pinMode(pin, mode);
  }

  // GPIO.DIGITAL_WRITE(C, D) // STATE: LOW = 0x0, HIGH = 0x1
  if (opcode == 31) {
    uint8_t pin = EEPROM.read(EEPROM_Pointer++);
    uint8_t state = EEPROM.read(EEPROM_Pointer++);

    digitalWrite(pin, state);
  }

  // GPIO.DIGITAL_READ(C) // READS TO SENDBUFFER
  if (opcode == 32) {
    uint8_t pin = EEPROM.read(EEPROM_Pointer++);

    SENDBUF[SENDBUF_Pointer++] = digitalRead(pin);
  }


  if (opcode == 60) {
      Serial.write(SensorId);

      for (int i = 0; i < SENDBUF_Pointer; i++) {
          Serial.write(SENDBUF[i]);
          SENDBUF[i] == 0;
      }
      SENDBUF_Pointer = 0;
  }

  // ENABLE_DOWNLINKINTERPRETATION()  // IF ENABLED DOWNLINK BYTES WILL BE INTERPRETED AS OPCODE (ONE COMMAND ONLY) (DEFAULT = off)
  if (opcode == 61) {
    INTERPRET_DOWNLINK = 0x1;
  }

  //DISABLE_DOWNLINKINTERPRETATION()
  if (opcode == 62) {
    INTERPRET_DOWNLINK = 0x0;
  }

  //TRANSMIT_SENDBUFFER_WHEN_COUNTER(TILL_COUNTER)  //WILL CLEAR SENDBUFFER
  if (opcode == 63) {

      uint8_t curr = EEPROM.read(510);
      uint8_t till = EEPROM.read(EEPROM_Pointer++);
      if (curr >= till)
      {
          EEPROM.update(510, 0);

          Serial.write(SensorId);

          for (int i = 0; i < SENDBUF_Pointer; i++) {
              Serial.write(SENDBUF[i]);
              SENDBUF[i] == 0;
          }
          SENDBUF_Pointer = 0;
      }      
  }

  // WRITE SENDBUF TO SERIAL
  if (opcode == 230) {
    for (int i = 0; i < sizeof(SENDBUF); i++) {
    }
  }

  if (opcode == 231) {
    EEPROM_TOSERIAL();
  }
}



void EXECUTE_TEMPORARY(uint8_t opcode[], uint8_t start, uint8_t len) {
  // Writing current EEPROM_Pointer into Temporary storage
  EEPROM_Pointer_Storage = EEPROM_Pointer;

  // Writing address in EEPROM where temporary code is stored into EEPROM_Pointer
  EEPROM_Pointer = EEPROM_TEMP_START;

  // Clearing EEPROM_Range which will contain the temporary Code
  CLEAR_TEMP_EEPROM();

  // Writing startaddress of temporary EEPROM_range into temporary variable.
  uint16_t TEMP_POINTER = EEPROM_TEMP_START;

  // Writing temporary Opcode to the EEPROM.
  for (int i = 0; i < len; i++) {
    EEPROM.update(TEMP_POINTER++, opcode[i + start]);
  }

  // Adding Opcode to return to the previous EEPROM address after executing the temporaray code.
  EEPROM.update(TEMP_POINTER++, 11);
  EEPROM.update(TEMP_POINTER++, (EEPROM_Pointer_Storage >> 8));
  EEPROM.update(TEMP_POINTER++, (EEPROM_Pointer_Storage & 0xFF));

  EEPROM_TEMP_TOSERIAL();
}

void CLEAR_TEMP_EEPROM() {
  for (int i = EEPROM_TEMP_START; i < (EEPROM_TEMP_START + EEPROM_TEMP_LENGTH); i++) {
    EEPROM.update(i, 0x0);
  }
}

void SET_PROGSTART(uint16_t addr) {
  EEPROM.update(PROGSTART_POINTER, (addr >> 8));
  EEPROM.update(PROGSTART_POINTER + 1, (addr & 0xFF));

  READ_PROGSTART();
}

uint16_t READ_PROGSTART() {
  uint8_t msb = EEPROM.read(PROGSTART_POINTER);
  uint8_t lsb = EEPROM.read(PROGSTART_POINTER + 1);
  uint16_t progstart = (msb << 8) | lsb;
  return progstart;
}

void WRITE_EEPROM() { 
uint8_t counter = 0;
bool exitSetup = false;
  while(!exitSetup) {
delay(500);
    if(Serial.available() > 0 && Serial.read()==90){
      delay(1000);
      if(Serial.read()==SensorId)
      {
        while(Serial.available() > 0){
          uint8_t incomingByte = Serial.read();
          if(incomingByte==90){
            exitSetup = true;
            break;
          }
          else
          {
            EEPROM.update(counter, incomingByte);
            counter = counter +1;
          }
        }
      }
    }
  }

    EEPROM_Pointer=0;
    EEPROM.update(510, 0);
}

void CLEAR_EEPROM() {
  for (int i = 0; i < EEPROM.length() - 2; i++) {
    EEPROM.update(i, 0x0);
  }
}

int MostSigLeastSigBytes(byte byteM,byte byteL)
{
  return ((byteM << 8) | byteL);
}


void EEPROM_TOSERIAL() {
  uint8_t value = 0xFF;
  uint16_t pointer = 0;
  uint8_t zeroCounter = 0;

  ////Serial.println(F("EEPROM CONTENT:"));
  while (zeroCounter < 3) {
    value = EEPROM.read(pointer++);
    if (value == 0x0) {
      zeroCounter++;
    }
    else {
      zeroCounter = 0;
    }
  }
}

void EEPROM_TEMP_TOSERIAL() {
  uint8_t value = 0xFF;
  uint16_t pointer = EEPROM_TEMP_START;
  uint8_t zeroCounter = 0;

  ////Serial.println(F("EEPROM_TEMPCODESECTION CONTENT:"));
  while (zeroCounter < 3 && pointer < (EEPROM_TEMP_START + EEPROM_TEMP_LENGTH)) {
    value = EEPROM.read(pointer++);
    if (value == 0x0) {
      zeroCounter++;
    }
    else {
      zeroCounter = 0;
    }
  }
}
