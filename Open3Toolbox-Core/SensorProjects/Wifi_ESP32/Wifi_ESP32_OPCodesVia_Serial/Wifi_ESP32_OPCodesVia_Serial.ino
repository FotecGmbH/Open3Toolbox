
/*
 Name:    ArduinoLora.ino
 Created: 8/30/2021 12:45:05 PM
 Author:  kevin
*/

#include <AsyncUDP.h>
#include <WiFi.h>
#include <Wire.h>
#include <EEPROM.h>
#include <SPI.h>
#include <HardwareSerial.h>

#define DEBUGSERIAL Serial2

bool debugmode = true;

//CHANGE THIS!!
char* ssid = "RTG-Control-Unit";
char* pw = "pw";


uint16_t EEPROM_Pointer_Storage = 0;
uint16_t EEPROM_Pointer = 0;
uint8_t SENDBUF_Pointer = 0;
static uint8_t SENDBUF[100];
static uint8_t TRANSMISSIONBUFFER[101];
uint16_t doubleValues = 0;
uint16_t boolValues = 0;
unsigned long delayStart;
uint16_t delayPeriod;
int sleepCounter = 0;
uint8_t INTERPRET_DOWNLINK = 0;
uint16_t EEPROM_TEMP_START = 400;
uint16_t EEPROM_TEMP_LENGTH = 400;
uint16_t PROGSTART_POINTER = 511;
AsyncUDP udp;

uint8_t IncomingBytes[100];
uint8_t SensorId = 26; // hardcoded


void setup() {
	TRANSMISSIONBUFFER[0] = SensorId;

	Serial.begin(9600); // opens serial port, sets data rate to 115200 bps
	Serial2.begin(115200, SERIAL_8N1, 25, 26); // opens serial port, sets data rate to 115200 bps
	DebugPrintln("Setup");
	initWiFi();

	EEPROM.begin(512);
	//delay(5000);
	WRITE_EEPROM();
	Wire.begin(21, 22);
	EEPROM_TOSERIAL();
}

void loop() {
	uint8_t opcode;

	if (delayPeriod > 0 && delayStart > 0 && (delayStart + delayPeriod) > millis()) {
		opcode = 90;
		if ((sleepCounter++ % 500000) == 0) {

			DebugPrint(F("Sleeping.. "));
			DebugPrint(F("Resuming in: "));
			DebugPrint((delayStart + delayPeriod - millis()) / 1000);
			DebugPrintln(F(" seconds"));

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

void initWiFi() {
	WiFi.mode(WIFI_STA);
	WiFi.begin(ssid, pw);
	DebugPrintln("Connecting to WiFi ..");
	while (WiFi.status() != WL_CONNECTED) {
		Serial.print('.');
		delay(1000);
	}

	DebugPrintln(WiFi.localIP().toString());
}

void EXECUTE_OPCODE(uint8_t opcode) {

	// WRITE_PROGSTART(C, D)
	if (opcode == 1) {

		uint8_t addrM = EEPROM.read(EEPROM_Pointer++);
		uint8_t addrL = EEPROM.read(EEPROM_Pointer++);
		uint16_t addr = (addrM << 8) | addrL;

		SET_PROGSTART(addr);
		DebugPrint(F("Executing WRITE_PROGSTART("));
		DebugPrint(addr);
		DebugPrintln(F(")"));
	}

	if (opcode == 2) {
		uint16_t PROGSTART = READ_PROGSTART();
		EEPROM_Pointer = PROGSTART;
		DebugPrintln(F("Executing SET_EEPROMTOPROGSTART()"));
		DebugPrint(F("EEPROM_POINTER set to: "));
		DebugPrintln(PROGSTART);
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


		DebugPrint(F("Executing SOFTDELAY("));
		DebugPrint(period);
		DebugPrintln(F(")"));
	}

	// DELAY(MS) 
	if (opcode == 10) {
		uint8_t ms = EEPROM.read(EEPROM_Pointer++);
		DebugPrint(F("Executing DELAY("));
		DebugPrint(ms);
		DebugPrintln(F(")"));

		delay(ms);
	}

	// SET_EEPROMPONTER(ADDR)
	if (opcode == 11) {
		uint8_t addrM = EEPROM.read(EEPROM_Pointer++);
		uint8_t addrL = EEPROM.read(EEPROM_Pointer++);
		uint16_t addr = (addrM << 8) | addrL;

		DebugPrint(F("Executing SET_EEPROMPOINTER("));
		DebugPrint(addr);
		DebugPrintln(F(")"));

		EEPROM_Pointer = addr;
	}

	// I2C.READ(C, D) C = addr, D = length
	if (opcode == 20) {
		uint8_t addr = EEPROM.read(EEPROM_Pointer++);
		uint8_t len = EEPROM.read(EEPROM_Pointer++);

		DebugPrint(F("Executing I2C.READ("));
		DebugPrint(addr);
		DebugPrint(F(", "));
		DebugPrint(len);
		DebugPrintln(F(")"));

		Wire.requestFrom(addr, len);

		for (size_t i = 0; i < 8; i++)
		{
			if (Wire.available()) {
				int val = Wire.read();// slave may send less than requested
				SENDBUF[SENDBUF_Pointer++] = val; // receive a byte as character
			}
			else {
				SENDBUF[SENDBUF_Pointer++] = 0; // receive a byte as character
			}
		}
		doubleValues = doubleValues + 1;
	}

	// I2C.WRITEREAD(C, D, E) C = addr, D = writeByte, E = length
	if (opcode == 21) {
		uint8_t addr = EEPROM.read(EEPROM_Pointer++);
		uint8_t writeByte = EEPROM.read(EEPROM_Pointer++);
		uint8_t len = EEPROM.read(EEPROM_Pointer++);

		DebugPrint(F("Executing I2C.WRITEREAD("));
		DebugPrint(addr);
		DebugPrint(F(", "));
		DebugPrint(writeByte, HEX);
		DebugPrint(F(", "));
		DebugPrint(len);
		DebugPrintln(F(")"));

		Wire.beginTransmission(addr);
		Wire.write(writeByte);
		Wire.endTransmission(false);

		Wire.requestFrom(addr, len);

		for (size_t i = 0; i < 8; i++)
		{
			if (Wire.available()) {
				int val = Wire.read();// slave may send less than requested
				SENDBUF[SENDBUF_Pointer++] = val; // receive a byte as character
			}
			else {
				SENDBUF[SENDBUF_Pointer++] = 0; // receive a byte as character
			}
		}

		doubleValues = doubleValues + 1;
	}

	// I2C.WRITE(C, D)
	if (opcode == 25) {
		uint8_t addr = EEPROM.read(EEPROM_Pointer++);
		uint8_t byte = EEPROM.read(EEPROM_Pointer++);

		DebugPrint(F("Executing I2C.WRITE("));
		DebugPrint(addr);
		DebugPrint(F(", "));
		DebugPrint(byte, HEX);
		DebugPrintln(F(")"));

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

		DebugPrint(F("Executing I2C.WRITE2("));
		DebugPrint(addr);
		DebugPrint(F(", "));
		DebugPrint(byte1, HEX);
		DebugPrint(F(", "));
		DebugPrint(byte2, HEX);
		DebugPrintln(F(")"));

		Wire.beginTransmission(addr);
		Wire.write(buf, 2);
		Wire.endTransmission();
	}

	// GPIO.SET_PINMODE(C, D)  // MODE: INPUT = 0x0, OUTPUT = 0x1, INPUT_PULLUP 0x2
	if (opcode == 30) {
		uint8_t pin = EEPROM.read(EEPROM_Pointer++);
		uint8_t mode = EEPROM.read(EEPROM_Pointer++);

		DebugPrint(F("Executing GPIO.SET_PINMODE("));
		DebugPrint(pin);
		DebugPrint(F(", "));
		DebugPrint(mode);
		DebugPrintln(F(")"));

		pinMode(pin, mode);
	}

	// GPIO.DIGITAL_WRITE(C, D) // STATE: LOW = 0x0, HIGH = 0x1
	if (opcode == 31) {
		uint8_t pin = EEPROM.read(EEPROM_Pointer++);
		uint8_t state = EEPROM.read(EEPROM_Pointer++);

		DebugPrint(F("Executing GPIO.DIGITAL_WRITE("));
		DebugPrint(pin);
		DebugPrint(F(", "));
		DebugPrint(state);
		DebugPrintln(F(")"));

		digitalWrite(pin, state);
	}

	// GPIO.DIGITAL_READ(C) // READS TO SENDBUFFER
	if (opcode == 32) {
		uint8_t pin = EEPROM.read(EEPROM_Pointer++);

		DebugPrint(F("Executing GPIO.DIGITAL_READ("));
		DebugPrint(pin);
		DebugPrintln(F(")"));
		DebugPrint(F("READ VALUE: "));
		uint8_t value = digitalRead(pin);
		DebugPrintln(value, HEX);
		SENDBUF[SENDBUF_Pointer++] = value;

		boolValues = boolValues + 1;
	}


	// INCREASE_SEND_COUNTER()
	if (opcode == 12) {
		uint8_t curr = EEPROM.read(510);
		EEPROM.write(510, (curr + 1));
		EEPROM.commit();
	}

	//TRANSMIT_SENDBUFFER_WHEN_COUNTER(TILL_COUNTER)  //WILL CLEAR SENDBUFFER
	if (opcode == 63) {

		uint8_t curr = EEPROM.read(510);
		uint8_t till = EEPROM.read(EEPROM_Pointer++);
		if (curr >= till)
		{
			SEND_DATA();
		}
	}

	if (opcode == 60) {
		SEND_DATA();
	}

	// ENABLE_DOWNLINKINTERPRETATION()  // IF ENABLED DOWNLINK BYTES WILL BE INTERPRETED AS OPCODE (ONE COMMAND ONLY) (DEFAULT = off)
	if (opcode == 61) {
		DebugPrintln(F("Enabling downlinkexecution"));
		INTERPRET_DOWNLINK = 0x1;
	}

	//DISABLE_DOWNLINKINTERPRETATION()
	if (opcode == 62) {
		DebugPrintln(F("Disabling downlinkexecution"));
		INTERPRET_DOWNLINK = 0x0;
	}

	// WRITE SENDBUF TO SERIAL
	if (opcode == 230) {
		DebugPrintln(F("SENDBUF CONTENT:"));

		for (int i = 0; i < sizeof(SENDBUF); i++) {
			DebugPrint(F("SENDBUF: "));
			DebugPrintln(SENDBUF[i]);
		}
	}

	if (opcode == 231) {
		EEPROM_TOSERIAL();
	}
}


void EXECUTE_TEMPORARY(uint8_t opcode[], uint8_t start, uint8_t len) {
	DebugPrintln(F("TEMPCODE: "));
	DebugPrint(F("LEN: "));
	DebugPrintln(len);
	DebugPrint(F("Start: "));
	DebugPrintln(start);

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
		EEPROM.write(TEMP_POINTER++, opcode[i + start]);
	}

	// Adding Opcode to return to the previous EEPROM address after executing the temporaray code.
	EEPROM.write(TEMP_POINTER++, 11);
	EEPROM.write(TEMP_POINTER++, (EEPROM_Pointer_Storage >> 8));
	EEPROM.write(TEMP_POINTER++, (EEPROM_Pointer_Storage & 0xFF));
	EEPROM.commit();

	EEPROM_TEMP_TOSERIAL();
}

void CLEAR_TEMP_EEPROM() {
	for (int i = EEPROM_TEMP_START; i < (EEPROM_TEMP_START + EEPROM_TEMP_LENGTH); i++) {
		EEPROM.write(i, 0x0);
	}
	EEPROM.commit();
}

void SET_PROGSTART(uint16_t addr) {
	EEPROM.write(PROGSTART_POINTER, (addr >> 8));
	EEPROM.write(PROGSTART_POINTER + 1, (addr & 0xFF));
	EEPROM.commit();

	READ_PROGSTART();
}

uint16_t READ_PROGSTART() {
	uint8_t msb = EEPROM.read(PROGSTART_POINTER);
	uint8_t lsb = EEPROM.read(PROGSTART_POINTER + 1);
	uint16_t progstart = (msb << 8) | lsb;

	DebugPrint("PROGSTART: ");
	DebugPrintln(progstart);

	return progstart;
}

void WRITE_EEPROM() { /*byte* opCodes*/
	DebugPrintln("Write_EEPROM");
	uint8_t counter = 0;
	uint8_t timeout = 0;
	bool exitSetup = false;
	while (!exitSetup && timeout++ < 10) {
		delay(500);
		DebugPrintln("Waiting");
		DebugPrintln(Serial.available());
		if (Serial.available() > 0 && Serial.read() == 90) {
			if (Serial.read() == SensorId)
			{
				while (Serial.available() > 0) {
					uint8_t incomingByte = Serial.read();
					if (incomingByte == 90) {
						exitSetup = true;
						break;
					}
					else
					{
						EEPROM.write(counter, incomingByte);
						DebugPrint(F("Writing Byte: "));
						DebugPrintln(incomingByte);
						counter = counter + 1;
					}
				}
			}
		}
	}
	EEPROM.commit();
}

void CLEAR_EEPROM() {
	for (int i = 0; i < EEPROM.length() - 2; i++) {
		EEPROM.write(i, 0x0);
	}
	EEPROM.commit();
}

void EEPROM_TOSERIAL() {
	uint8_t value = 0xFF;
	uint16_t pointer = 0;
	uint8_t zeroCounter = 0;

	Serial2.println(F("EEPROM CONTENT:"));
	while (zeroCounter < 5) {
		value = EEPROM.read(pointer++);
		DebugPrint(F("EEPROM: "));
		DebugPrintln(value);
		if (value == 0x0) {
			zeroCounter++;
		}
		else {
			zeroCounter = 0;
		}
	}
}

void RESET_SENDCOUNTER() {
	EEPROM.write(510, 0);
	EEPROM.commit();
}

void EEPROM_TEMP_TOSERIAL() {
	uint8_t value = 0xFF;
	uint16_t pointer = EEPROM_TEMP_START;
	uint8_t zeroCounter = 0;

	Serial.println(F("EEPROM_TEMPCODESECTION CONTENT:"));
	while (zeroCounter < 3 && pointer < (EEPROM_TEMP_START + EEPROM_TEMP_LENGTH)) {
		value = EEPROM.read(pointer++);
		DebugPrint(F("TEMPCODE: "));
		DebugPrintln(value);
		if (value == 0x0) {
			zeroCounter++;
		}
		else {
			zeroCounter = 0;
		}
	}
}

void SENDBUFFER_TOSERIAL() {
	uint8_t value = 0xFF;
	uint16_t pointer = 0;
	uint8_t zeroCounter = 0;

	DebugPrintln(F("SENDBUFFER CONTENT:"));
	while (zeroCounter < 10 && pointer < sizeof(SENDBUF) - 1) {
		value = SENDBUF[pointer++];
		DebugPrint(F("SENDBUFFER: "));
		DebugPrintln(value);
		if (value == 0x0) {
			zeroCounter++;
		}
		else {
			zeroCounter = 0;
		}
	}
}

void SEND_DATA() {
	uint16_t transmittingBytes = 8 * doubleValues + boolValues;

	RESET_SENDCOUNTER();
	DebugPrintln("SENDING...");
	DebugPrint("Lenght: ");
	DebugPrintln(transmittingBytes);

	// Copying Sendbuffer into transmissionbuffer with sensorid at index 0
	memcpy(&TRANSMISSIONBUFFER[1], SENDBUF, sizeof(SENDBUF));

	SENDBUFFER_TOSERIAL();

	// Broadcasting data.
	udp.broadcastTo(TRANSMISSIONBUFFER, transmittingBytes + 1, 1234);

	//Clearing Sendbuffer
	CLEAR_SENDBUFFER();

	SENDBUF_Pointer = 0;
	doubleValues = 0;
	boolValues = 0;
}

void CLEAR_SENDBUFFER() {
	for (int i = 0; i < sizeof(SENDBUF); i++) {
		SENDBUF[i] = 0x0;
	}
}

#pragma region DebugPrint
void DebugPrintln(unsigned char message) {
	if (debugmode) {
		DEBUGSERIAL.println(message);
	}
}

void DebugPrint(unsigned char message) {
	if (debugmode) {
		DEBUGSERIAL.print(message);
	}
}

void DebugPrintln(const __FlashStringHelper* message) {
	if (debugmode) {
		DEBUGSERIAL.println(message);
	}
}

void DebugPrint(const __FlashStringHelper* message) {
	if (debugmode) {
		DEBUGSERIAL.print(message);
	}
}

void DebugPrintln(String message) {
	if (debugmode) {
		DEBUGSERIAL.println(message);
	}
}

void DebugPrint(String message) {
	if (debugmode) {
		DEBUGSERIAL.print(message);
	}
}

void DebugPrintln(uint message, int format) {
	if (debugmode) {
		DEBUGSERIAL.println(message, format);
	}
}

void DebugPrint(uint message, int format) {
	if (debugmode) {
		DEBUGSERIAL.print(message, format);
	}
}
#pragma endregion

void SCAN_I2C() {
	byte error, address;
	int nDevices;
	DebugPrintln("Scanning...");
	nDevices = 0;
	for (address = 1; address < 127; address++) {
		Wire.beginTransmission(address);
		error = Wire.endTransmission();
		if (error == 0) {
			DebugPrint("I2C device found at address 0x");
			if (address < 16) {
				DebugPrint("0");
			}
			DebugPrintln(address, HEX);
			nDevices++;
		}
		else if (error == 4) {
			DebugPrint("Unknow error at address 0x");
			if (address < 16) {
				DebugPrint("0");
			}
			DebugPrintln(address, HEX);
		}
	}
	if (nDevices == 0) {
		DebugPrintln("No I2C devices found\n");
	}
	else {
		DebugPrintln("done\n");
	}
	delay(5000);
}