#include "SimData.h"


uint16_t holding_registers[TOTAL_HOLDING_REGISTERS];
static const byte pot_pins[6] = { A0, A1, A2, A3, A4, A5 };

void init_sim_data() {
  pinMode(A0, INPUT);
  pinMode(A1, INPUT);
  pinMode(A2, INPUT);
  pinMode(A3, INPUT);
  pinMode(A4, INPUT);
  pinMode(A5, INPUT);
}

void fill_sim_data() {
  holding_registers[0] = map(analogRead(pot_pins[0]), 0, 1023, 0, 255);
  holding_registers[1] = map(analogRead(pot_pins[1]), 0, 1023, 0, 255);
  holding_registers[2] = map(analogRead(pot_pins[2]), 0, 1023, 0, 255);
  holding_registers[3] = map(analogRead(pot_pins[3]), 0, 1023, 0, 255);
  holding_registers[4] = map(analogRead(pot_pins[4]), 0, 1023, 0, 255);
  holding_registers[5] = map(analogRead(pot_pins[5]), 0, 1023, 0, 255);
  holding_registers[6] = 100;
  holding_registers[7] = 100;
  holding_registers[8] = 1000;
  holding_registers[9] = 1000;
  holding_registers[10] = 0;
  holding_registers[11] = 1;
  holding_registers[12] = 0;
  holding_registers[13] = 10;
  holding_registers[14] = 0;
  holding_registers[15] = 100;
}
