#include "ModbusRTUSlave.h"
#include "CyclicTaskExecutor.h"

#define SLAVE_ID     0x0001

void fill_holding_registers();
void fill_input_registers();

const byte potPins[6] = { A0, A1, A2, A3, A4, A5 };

uint16_t holdingRegisters[16];
uint16_t inputRegisters[16];

ModbusRTUSlave modbus(Serial);

void setup() {
  pinMode(A0, INPUT);
  pinMode(A1, INPUT);
  pinMode(A2, INPUT);
  pinMode(A3, INPUT);
  pinMode(A4, INPUT);
  pinMode(A5, INPUT);

  modbus.configureHoldingRegisters(holdingRegisters, 16);
  // modbus.configureInputRegisters(inputRegisters, 16);
  
  modbus.begin(SLAVE_ID, 115200);
}



void loop() {
  fill_holding_registers();
  // fill_input_registers(); 
  
  modbus.poll();
}

void fill_holding_registers() {
  holdingRegisters[0] = map(analogRead(potPins[0]), 0, 1023, 0, 255);
  holdingRegisters[1] = map(analogRead(potPins[1]), 0, 1023, 0, 255);
  holdingRegisters[2] = map(analogRead(potPins[2]), 0, 1023, 0, 255);
  holdingRegisters[3] = map(analogRead(potPins[3]), 0, 1023, 0, 255);
  holdingRegisters[4] = map(analogRead(potPins[4]), 0, 1023, 0, 255);
  holdingRegisters[5] = map(analogRead(potPins[5]), 0, 1023, 0, 255);
  holdingRegisters[6] = 100;
  holdingRegisters[7] = 100;
  holdingRegisters[8] = 1000;
  holdingRegisters[9] = 1000;
  holdingRegisters[10] = 0;
  holdingRegisters[11] = 1;
  holdingRegisters[12] = 0;
  holdingRegisters[13] = 10;
  holdingRegisters[14] = 0;
  holdingRegisters[15] = 100;
}

void fill_input_registers() {
  inputRegisters[0] = map(analogRead(potPins[0]), 0, 1023, 0, 255);
  inputRegisters[1] = map(analogRead(potPins[1]), 0, 1023, 0, 255);
  inputRegisters[2] = map(analogRead(potPins[2]), 0, 1023, 0, 255);
  inputRegisters[3] = map(analogRead(potPins[3]), 0, 1023, 0, 255);
  inputRegisters[4] = map(analogRead(potPins[4]), 0, 1023, 0, 255);
  inputRegisters[5] = map(analogRead(potPins[5]), 0, 1023, 0, 255);
  inputRegisters[6] = 100;
  inputRegisters[7] = 100;
  inputRegisters[8] = 1000;
  inputRegisters[9] = 1000;
  inputRegisters[10] = 0;
  inputRegisters[11] = 1;
  inputRegisters[12] = 0;
  inputRegisters[13] = 10;
  inputRegisters[14] = 0;
  inputRegisters[15] = 100;
}
