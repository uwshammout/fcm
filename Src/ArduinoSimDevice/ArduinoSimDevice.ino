#include "ModbusRTUSlave.h"
#include "CyclicTaskExecutor.h"

#define SLAVE_ID                  0x0001
#define TOTAL_HOLDING_REGISTERS   16
#define BAUD_RATE                 115200

using namespace cblk;

void fill_holding_registers();

const byte potPins[6] = { A0, A1, A2, A3, A4, A5 };

uint16_t holdingRegisters[TOTAL_HOLDING_REGISTERS];

ModbusRTUSlave modbus(Serial);
CyclicTaskExecutor exe_fill_holding_registers(fill_holding_registers, 200);

void setup() {
  pinMode(A0, INPUT);
  pinMode(A1, INPUT);
  pinMode(A2, INPUT);
  pinMode(A3, INPUT);
  pinMode(A4, INPUT);
  pinMode(A5, INPUT);

  modbus.configureHoldingRegisters(holdingRegisters, TOTAL_HOLDING_REGISTERS);
  modbus.begin(SLAVE_ID, BAUD_RATE);
}


void loop() {
  unsigned long currentTime_ms = millis();
  
  exe_fill_holding_registers.update(currentTime_ms);
  
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
