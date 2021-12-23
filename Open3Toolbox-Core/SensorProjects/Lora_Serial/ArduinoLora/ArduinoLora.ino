/*
 Name:		ArduinoLora.ino
 Created:	8/30/2021 12:45:05 PM
 Author:	kevin
*/

// the setup function runs once when you press reset or power the board
#include <Wire.h>
#include <EEPROM.h>
#include <CayenneLPP.h>
#include <lmic.h>
#include <arduino_lmic_user_configuration.h>
#include <arduino_lmic_lorawan_compliance.h>
#include <arduino_lmic_hal_configuration.h>
#include <arduino_lmic_hal_boards.h>
#include <arduino_lmic.h>
#include <hal/hal.h>
#include <SPI.h>


uint16_t EEPROM_Pointer_Storage = 0;
uint16_t EEPROM_Pointer = 0;
uint8_t SENDBUF_Pointer = 0;
static uint8_t SENDBUF[10];
unsigned long delayStart;
uint16_t delayPeriod;
int sleepCounter = 0;
uint8_t INTERPRET_DOWNLINK = 0;
uint16_t EEPROM_TEMP_START = 400;
uint16_t EEPROM_TEMP_LENGTH = 400;
uint16_t PROGSTART_POINTER = 511;

/*static const uint8_t HARDCODE[] = {
	 61, 230, 60, 9, 0xFF, 0xFF, 2
};*/


#pragma region LMIC



// This EUI must be in little-endian format, so least-significant-byte
// first. When copying an EUI from ttnctl output, this means to reverse
// the bytes. For TTN issued EUIs the last bytes should be 0xD5, 0xB3,
// 0x70.
static const u1_t PROGMEM APPEUI[8] = { 0x1C, 0x2B, 0x82, 0x51, 0x9A, 0x41, 0x40, 0xA8 };
void os_getArtEui(u1_t* buf) { memcpy_P(buf, APPEUI, 8); }

// This should also be in little endian format, see above.
static const u1_t PROGMEM DEVEUI[8] = { 0xD7, 0x4A, 0x04, 0xD0, 0x7E, 0xD5, 0xB3, 0x70 };
void os_getDevEui(u1_t* buf) { memcpy_P(buf, DEVEUI, 8); }

// This key should be in big endian format (or, since it is not really a
// number but a block of memory, endianness does not really apply). In
// practice, a key taken from ttnctl can be copied as-is.
static const u1_t PROGMEM APPKEY[16] = { 0x21, 0x98, 0xC6, 0xF4, 0x77, 0x6E, 0xAF, 0x87, 0xB2, 0x76, 0x33, 0x45, 0x76, 0x24, 0x0F, 0x9B };
void os_getDevKey(u1_t* buf) { memcpy_P(buf, APPKEY, 16); }

static osjob_t sendjob;

// Schedule TX every this many seconds (might become longer due to duty
// cycle limitations).
const unsigned TX_INTERVAL = 60;


// Pin mapping
const lmic_pinmap lmic_pins = {
	.nss = 10,
	.rxtx = LMIC_UNUSED_PIN,
	.rst = 9,
	.dio = {2, 6, 7},
};

void onEvent(ev_t ev) {
	Serial.print(os_getTime());
	Serial.print(F(": "));
	switch (ev) {
	case EV_SCAN_TIMEOUT:
		break;
	case EV_BEACON_FOUND:
		break;
	case EV_BEACON_MISSED:
		break;
	case EV_BEACON_TRACKED:
		break;
	case EV_JOINING:
		break;
	case EV_JOINED:
		//////Serial.print(F("E"));
		{
			u4_t netid = 0;
			devaddr_t devaddr = 0;
			u1_t nwkKey[16];
			u1_t artKey[16];
			LMIC_getSessionKeys(&netid, &devaddr, nwkKey, artKey);
			Serial.print(F("netid: "));
			//////Serial.print(netid, DEC);
			Serial.print(F("devaddr: "));
			//////Serial.print(devaddr, HEX);
			Serial.print(F("AppSKey: "));
			//////Serial.print();
		}
		// Disable link check validation (automatically enabled
		// during join, but because slow data rates change max TX
	// size, we don't use it in this example.
		LMIC_setLinkCheckMode(0);
		break;
		/*
		|| This event is defined but not used in the code. No
		|| point in wasting codespace on it.
		||
		|| case EV_RFU1:
		||     //////Serial.print(F("EV_RFU1"));
		||     break;
		*/
	case EV_JOIN_FAILED:
		//////Serial.print(F("EV_JOIN_FAILED"));
		break;
	case EV_REJOIN_FAILED:
		//////Serial.print(F("EV_REJOIN_FAILED"));
		break;
	case EV_TXCOMPLETE:
		//////Serial.print(F("EV_TXCOMPLETE (includes waiting for RX windows)"));
		if (LMIC.txrxFlags & TXRX_ACK)
			//////Serial.print(F("Received ack"));
		if (LMIC.dataLen) {
			Serial.print(F("Received "));
			Serial.print(LMIC.dataLen);
			Serial.print(F(" bytes of payload:"));
			for (int i = 0; i < LMIC.dataLen; i++) {
				if (LMIC.frame[LMIC.dataBeg + i] < 0x10) {
					Serial.print(F("0"));
				}
				Serial.print(LMIC.frame[LMIC.dataBeg + i], HEX);

				if (i < LMIC.dataLen - 1) {
					Serial.print("-");
				}
			}
			//////Serial.print();
			if (INTERPRET_DOWNLINK > 0x0) {
				//////Serial.print("INTERPRETING");
				EXECUTE_TEMPORARY(LMIC.frame, LMIC.dataBeg, LMIC.dataLen);
			}
			//////Serial.print();

		}

			LMIC_clrTxData();
		// Schedule next transmission
		//os_setTimedCallback(&sendjob, os_getTime() + sec2osticks(TX_INTERVAL), do_send);
		break;
	case EV_LOST_TSYNC:
		//////Serial.print(F("EV_LOST_TSYNC"));
		break;
	case EV_RESET:
		//////Serial.print(F("EV_RESET"));
		break;
	case EV_RXCOMPLETE:
		// data received in ping slot
		//////Serial.print(F("EV_RXCOMPLETE"));
		break;
	case EV_LINK_DEAD:
		//////Serial.print(F("EV_LINK_DEAD"));
		break;
	case EV_LINK_ALIVE:
		//////Serial.print(F("EV_LINK_ALIVE"));
		break;
		/*
		|| This event is defined but not used in the code. No
		|| point in wasting codespace on it.
		||
		|| case EV_SCAN_FOUND:
		||    //////Serial.print(F("EV_SCAN_FOUND"));
		||    break;
		*/
	case EV_TXSTART:
		//////Serial.print(F("EV_TXSTART"));
		break;
	case EV_TXCANCELED:
		//////Serial.print(F("EV_TXCANCELED"));
		break;
	case EV_RXSTART:
		/* do not print anything -- it wrecks timing */
		break;
	case EV_JOIN_TXCOMPLETE:
		//////Serial.print(F("EV_JOIN_TXCOMPLETE: no JoinAccept"));
		break;

	default:
		Serial.print(F("Unknown event: "));
		//////Serial.print((unsigned)ev);
		break;
	}
}



void do_send(osjob_t* j) {
	// LMIC init
	os_init();
	// Reset the MAC state. Session and pending data transfers will be discarded.
	LMIC_reset();
	// Check if there is not a current TX/RX job running
	if (LMIC.opmode & OP_TXRXPEND) {
		Serial.println(F("OP_TXRXPEND, not sending"));
	}
	else {
		// Prepare upstream data transmission at the next possible time.
		LMIC_setTxData2(1, (char*)SENDBUF, sizeof(SENDBUF), 0);
		Serial.println(F("Packet queued"));
	}
	// Next TX is scheduled after TX_COMPLETE event.
}
#pragma endregion


void setup() {
	Serial.begin(9600);
	//Serial.println(F("Starting"));

 WRITE_EEPROM();



//for (int i = 0; i < 20; i++){
  //Serial.println("waiting in loop");
  //delay(1);
  //if(Serial.available() > 3){
  //  break;
  //}
//}
//Serial.println("waiting");
//int incomingByte = 0;
/*byte *incomingBytes = (byte*)malloc(0);
delay(1100);

//for (int i = 0; i < 20; i++){
//delay(1);
Serial.println("out while1");

while(Serial.available() > 0){
  //if(Serial.available() > 0)
  //{
   Serial.println("in while1");
   int sizeOfIncomingBytes = sizeof(incomingBytes);
   byte currArray[sizeOfIncomingBytes];
   for(int i = 0; i < sizeOfIncomingBytes;i++){
    currArray[i] = incomingBytes[i];
   }

Serial.println("in while2");
  
    incomingBytes = (byte*)realloc(incomingBytes,(sizeOfIncomingBytes + 1));

    Serial.println("in while3");
    
    // read the incoming byte:
    incomingBytes[sizeof(incomingBytes) - 1] = Serial.read();

    Serial.print("incomingByte111: ");
    Serial.println(incomingBytes[sizeof(incomingBytes) - 1]);

    for(int i = 0; i < sizeOfIncomingBytes;i++){
    incomingBytes[i] = currArray[i];
   }
    Serial.print("incomingByte111: ");
    Serial.println(incomingBytes[sizeof(incomingBytes) - 1]);
}
delay(5000);


for(int j = 0; j < sizeof(incomingBytes); j++){
  Serial.print("inside incomingBytes: ");
  Serial.println(incomingBytes[j]);
}*/
	LMIC_setClockError(MAX_CLOCK_ERROR * 1 / 100);
	Wire.begin();

	// LMIC init
	os_init();
	// Reset the MAC state. Session and pending data transfers will be discarded.
	LMIC_reset();

	//// Start job (sending automatically starts OTAA too)
	//do_send(&sendjob);

#pragma region USERCODE

	//WRITE_EEPROM(incomingBytes);
 
#pragma endregion

}

void loop() {
	os_runloop_once();

#pragma region USERCODE
	uint8_t opcode;

	if (delayPeriod > 0 && delayStart > 0 && (delayStart + delayPeriod) > millis()) {
		opcode = 90;
		if ((sleepCounter++ % 10000) == 0) {

			Serial.print(F("Sleeping.. "));
			//Serial.print(F("delayStart: "));
			//Serial.print(delayStart);
			//Serial.print(F("    delayTime: "));
			//Serial.print(delayPeriod);
			//Serial.print(F("    millis: "));
			//Serial.println(millis());
			Serial.print(F("Resuming in: "));
			Serial.print((delayStart + delayPeriod - millis()) / 1000);
			Serial.println(F(" seconds"));
		}
	}
	else {
		delayPeriod = 0;
		delayStart = 0;
		opcode = EEPROM.read(EEPROM_Pointer++);
		//Serial.print(F("Executing Opcode: "));
		//Serial.println(opcode);
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
		Serial.print(F("Executing WRITE_PROGSTART("));
		Serial.print(addr);
		Serial.println(F(")"));
	}

	if (opcode == 2) {
		uint16_t PROGSTART = READ_PROGSTART();
		EEPROM_Pointer = PROGSTART;
		Serial.println(F("Executing SET_EEPROMTOPROGSTART()"));
		Serial.print(F("EEPROM_POINTER set to: "));
		Serial.println(PROGSTART);
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


		Serial.print(F("Executing SOFTDELAY("));
		Serial.print(period);
		Serial.println(F(")"));
	}

	// DELAY(MS) 
	if (opcode == 10) {
		uint8_t ms = EEPROM.read(EEPROM_Pointer++);
		Serial.print(F("Executing DELAY("));
		Serial.print(ms);
		Serial.println(F(")"));

		delay(ms);
	}

	// SET_EEPROMPONTER(ADDR)
	if (opcode == 11) {
		uint8_t addrM = EEPROM.read(EEPROM_Pointer++);
		uint8_t addrL = EEPROM.read(EEPROM_Pointer++);
		uint16_t addr = (addrM << 8) | addrL;

		Serial.print(F("Executing SET_EEPROMPOINTER("));
		Serial.print(addr);
		Serial.println(F(")"));

		EEPROM_Pointer = addr;
	}

	// I2C.READ(C, D) C = addr, D = length
	if (opcode == 20) {
		uint8_t addr = EEPROM.read(EEPROM_Pointer++);
		uint8_t len = EEPROM.read(EEPROM_Pointer++);

		Serial.print(F("Executing I2C.READ("));
		Serial.print(addr);
		Serial.print(F(", "));
		Serial.print(len);
		Serial.println(F(")"));

		Wire.requestFrom(addr, len);

		while (Wire.available()) {
			int val = Wire.read();// slave may send less than requested
			SENDBUF[SENDBUF_Pointer++] = val; // receive a byte as character
			Serial.println(val);
		}
	}

	// I2C.WRITEREAD(C, D, E) C = addr, D = writeByte, E = length
	if (opcode == 21) {
		uint8_t addr = EEPROM.read(EEPROM_Pointer++);
		uint8_t writeByte = EEPROM.read(EEPROM_Pointer++);
		uint8_t len = EEPROM.read(EEPROM_Pointer++);

		Serial.print(F("Executing I2C.WRITEREAD("));
		Serial.print(addr);
		Serial.print(F(", "));
		Serial.print(writeByte, HEX);
		Serial.print(F(", "));
		Serial.print(len);
		Serial.println(F(")"));

		Wire.beginTransmission(addr);
		Wire.write(writeByte);
		Wire.endTransmission(false);

		Wire.requestFrom(addr, len);

		while (Wire.available()) {
			int val = Wire.read();// slave may send less than requested
			SENDBUF[SENDBUF_Pointer++] = val; // receive a byte as character
			Serial.println(val);
		}
	}

	// I2C.WRITE(C, D)
	if (opcode == 25) {
		uint8_t addr = EEPROM.read(EEPROM_Pointer++);
		uint8_t byte = EEPROM.read(EEPROM_Pointer++);

		Serial.print(F("Executing I2C.WRITE("));
		Serial.print(addr);
		Serial.print(F(", "));
		Serial.print(byte, HEX);
		Serial.println(F(")"));

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

		Serial.print(F("Executing I2C.WRITE2("));
		Serial.print(addr);
		Serial.print(F(", "));
		Serial.print(byte1, HEX);
		Serial.print(F(", "));
		Serial.print(byte2, HEX);
		Serial.println(F(")"));

		Wire.beginTransmission(addr);
		Wire.write(buf, 2);
		Wire.endTransmission();
	}

	// GPIO.SET_PINMODE(C, D)  // MODE: INPUT = 0x0, OUTPUT = 0x1, INPUT_PULLUP 0x2
	if (opcode == 30) {
		uint8_t pin = EEPROM.read(EEPROM_Pointer++);
		uint8_t mode = EEPROM.read(EEPROM_Pointer++);

		Serial.print(F("Executing GPIO.SET_PINMODE("));
		Serial.print(pin);
		Serial.print(F(", "));
		Serial.print(mode);
		Serial.println(F(")"));

		pinMode(pin, mode);
	}

	// GPIO.DIGITAL_WRITE(C, D) // STATE: LOW = 0x0, HIGH = 0x1
	if (opcode == 31) {
		uint8_t pin = EEPROM.read(EEPROM_Pointer++);
		uint8_t state = EEPROM.read(EEPROM_Pointer++);

		Serial.print(F("Executing GPIO.DIGITAL_WRITE("));
		Serial.print(pin);
		Serial.print(F(", "));
		Serial.print(state);
		Serial.println(F(")"));

		digitalWrite(pin, state);
	}

	// GPIO.DIGITAL_READ(C) // READS TO SENDBUFFER
	if (opcode == 32) {
		uint8_t pin = EEPROM.read(EEPROM_Pointer++);

		Serial.print(F("Executing GPIO.DIGITAL_READ("));
		Serial.print(pin);
		Serial.println(F(")"));

		SENDBUF[SENDBUF_Pointer++] = digitalRead(pin);
	}


	// TRANSMIT_SENDBUFFER()  WILL CLEAR SENDBUFFER
	if (opcode == 60) {

		if (LMIC.opmode & OP_TXRXPEND) {
			Serial.println(F("OP_TXRXPEND, not sending"));
			// Reset the MAC state. Session and pending data transfers will be discarded.
			//LMIC_reset();
			LMIC_clrTxData();
		}
		else {
			LMIC_clrTxData();
			// Prepare upstream data transmission at the next possible time.
			lmic_tx_error_t result = LMIC_setTxData2(1, (char*)SENDBUF, sizeof(SENDBUF), 0); // 0: success | -1: busy | -2: message too large | -3: message not feasable for datarate | -4: failed due to other reason
			Serial.print(F("SENDING...Result: "));
			Serial.println(result);

			for (int i = 0; i < sizeof(SENDBUF); i++) {
				SENDBUF[i] == 0;
			}

			SENDBUF_Pointer = 0;
		}
	}

	// ENABLE_DOWNLINKINTERPRETATION()  // IF ENABLED DOWNLINK BYTES WILL BE INTERPRETED AS OPCODE (ONE COMMAND ONLY) (DEFAULT = off)
	if (opcode == 61) {
		Serial.println(F("Enabling downlinkexecution"));
		INTERPRET_DOWNLINK = 0x1;
	}

	//DISABLE_DOWNLINKINTERPRETATION()
	if (opcode == 62) {
		Serial.println(F("Disabling downlinkexecution"));
		INTERPRET_DOWNLINK = 0x0;
	}

	// WRITE SENDBUF TO SERIAL
	if (opcode == 230) {
		Serial.println(F("SENDBUF CONTENT:"));

		for (int i = 0; i < sizeof(SENDBUF); i++) {
			Serial.print(F("SENDBUF: "));
			Serial.println(SENDBUF[i]);
		}
	}

	if (opcode == 231) {
		EEPROM_TOSERIAL();
	}
}

void EXECUTE_TEMPORARY(uint8_t opcode[], uint8_t start, uint8_t len) {
	Serial.println(F("TEMPCODE: "));
	Serial.print(F("LEN: "));
	Serial.println(len);
	Serial.print(F("Start: "));
	Serial.println(start);

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

	Serial.print("PROGSTART: ");
	Serial.println(progstart);

	return progstart;
}

void WRITE_EEPROM() { /*byte* opCodes*/
	//Serial.println(F("CLEARING EEPROM"));
	//CLEAR_EEPROM();

int counter = 0; 

delay(300);
while(Serial.available() > 0){
  
    
    int incomingByte = Serial.read();

    //Serial.println(F("WRITING EEPROM"));
    //Serial.println(incomingByte);
    EEPROM.update(counter, incomingByte);
    //Serial.println(incomingByte);
    counter = counter + 1;
    }
delay(300);
  

	/*Serial.println(F("WRITING EEPROM"));
	for (int i = 0; i < sizeof(opCodes); i++) {
		EEPROM.update(i, opCodes[i]);
    Serial.println(opCodes[i]);
	}*/
	Serial.println(F("EEPROM WRITTEN..."));
	EEPROM_TOSERIAL();
}

void CLEAR_EEPROM() {
	for (int i = 0; i < EEPROM.length() - 2; i++) {
		EEPROM.update(i, 0x0);
	}
}

void EEPROM_TOSERIAL() {
	uint8_t value = 0xFF;
	uint16_t pointer = 0;
	uint8_t zeroCounter = 0;

	Serial.println(F("EEPROM CONTENT:"));
	while (zeroCounter < 3) {
		value = EEPROM.read(pointer++);
		Serial.print(F("EEPROM: "));
		Serial.println(value);
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

	Serial.println(F("EEPROM_TEMPCODESECTION CONTENT:"));
	while (zeroCounter < 3 && pointer < (EEPROM_TEMP_START + EEPROM_TEMP_LENGTH)) {
		value = EEPROM.read(pointer++);
		Serial.print(F("TEMPCODE: "));
		Serial.println(value);
		if (value == 0x0) {
			zeroCounter++;
		}
		else {
			zeroCounter = 0;
		}
	}
}
