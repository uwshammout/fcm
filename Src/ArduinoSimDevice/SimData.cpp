#include "SimData.h"

#include <Arduino.h>

#define SIM_DATA_TYPE_ANALOG_READ      0x00
#define SIM_DATA_TYPE_01               0x01
#define SIM_DATA_TYPE_02               0x02
#define SIM_DATA_TYPE_03               0x03
#define SIM_DATA_TYPE_04               0x04
#define SIM_DATA_TYPE_05               0x05
#define SIM_DATA_TYPE_06               0x06
#define SIM_DATA_TYPE_07               0x07
#define SIM_DATA_TYPE_08               0x08
#define SIM_DATA_TYPE_09               0x09
#define SIM_DATA_TYPE_10               0x0A

#define SIM_DATA_TYPE  SIM_DATA_TYPE_01
#define GENERIC_DATA

uint16_t holding_registers[TOTAL_HOLDING_REGISTERS];

//- Mapping
#define FCTotalVoltage       holding_registers[0]
#define FCTotalCurrentVh     holding_registers[1]
#define FCTotalCurrentVl     holding_registers[2]
#define FCCellVl             holding_registers[3]
#define FCCellV2             holding_registers[4]
#define FCCellV3             holding_registers[5]
#define FCCellV4             holding_registers[6]
#define FCCellV5             holding_registers[7]
#define FCCellV6             holding_registers[8]
#define FCCellV7             holding_registers[9]
#define FCCellV8             holding_registers[10]
#define FCCellV9             holding_registers[11]
#define ELTotalVoltage       holding_registers[12]
#define ELTotalCurrentVh     holding_registers[13]
#define ELTotalCurrentVl     holding_registers[14]


/*************************************************************/
/* Generic data                                              */
/*************************************************************/
#ifdef GENERIC_DATA

#define TOTAL_VALUES 72

uint16_t const sines[TOTAL_VALUES] = { 140, 151, 163, 173, 184, 194, 204, 214, 223, 231, 238, 245, 251, 256, 261, 264, 266, 268, 268, 268, 266, 264, 261, 256, 251, 245, 238, 231, 223, 214, 204, 194, 184, 173, 163, 151, 140, 129, 118, 107, 97, 86, 76, 67, 58, 50, 42, 35, 29, 24, 20, 17, 14, 13, 12, 13, 14, 17, 20, 24, 29, 35, 42, 50, 58, 67, 76, 86, 97, 107, 118, 129 };
uint16_t const cosines[TOTAL_VALUES] = { 268, 268, 266, 264, 261, 256, 251, 245, 238, 231, 223, 214, 204, 194, 184, 173, 163, 151, 140, 129, 118, 107, 97, 86, 76, 67, 58, 50, 42, 35, 29, 24, 20, 17, 14, 13, 12, 13, 14, 17, 20, 24, 29, 35, 42, 50, 58, 67, 76, 86, 97, 107, 118, 129, 140, 151, 163, 173, 184, 194, 204, 214, 223, 231, 238, 245, 251, 256, 261, 264, 266, 268 };

#endif

/*************************************************************/
/* Implementing simulated data                               */
/*************************************************************/

#if SIM_DATA_TYPE == SIM_DATA_TYPE_ANALOG_READ

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

#elif  SIM_DATA_TYPE == SIM_DATA_TYPE_01

void init_sim_data() {}

static uint16_t data_index = 0;
static uint16_t data_index2 = 5;

void fill_sim_data() {
  
  FCTotalVoltage = sines[data_index];
  FCTotalCurrentVh = cosines[data_index];
  FCTotalCurrentVl = cosines[data_index2];

  uint16_t tenth_part = FCTotalVoltage / 10;
  FCCellVl = FCTotalVoltage - tenth_part * 9;
  FCCellV2 = FCTotalVoltage - tenth_part * 8;
  FCCellV3 = FCTotalVoltage - tenth_part * 7;
  FCCellV4 = FCTotalVoltage - tenth_part * 6;
  FCCellV5 = FCTotalVoltage - tenth_part * 5;
  FCCellV6 = FCTotalVoltage - tenth_part * 4;
  FCCellV7 = FCTotalVoltage - tenth_part * 3;
  FCCellV8 = FCTotalVoltage - tenth_part * 2;
  FCCellV9 = FCTotalVoltage - tenth_part;
  ELTotalVoltage = sines[data_index];
  ELTotalCurrentVh = cosines[data_index];
  ELTotalCurrentVl = cosines[data_index2];

  data_index++;
  data_index2++;

  if (data_index >= TOTAL_VALUES) {
    data_index = 0;
  }
  if (data_index2 >= TOTAL_VALUES) {
    data_index2 = 0;
  }
}

#elif  SIM_DATA_TYPE == SIM_DATA_TYPE_02
#  error "SIM_DATA_TYPE_02 not implemented"
#elif  SIM_DATA_TYPE == SIM_DATA_TYPE_03
#  error "SIM_DATA_TYPE_03 not implemented"
#elif  SIM_DATA_TYPE == SIM_DATA_TYPE_04
#  error "SIM_DATA_TYPE_04 not implemented"
#elif  SIM_DATA_TYPE == SIM_DATA_TYPE_05
#  error "SIM_DATA_TYPE_05 not implemented"
#elif  SIM_DATA_TYPE == SIM_DATA_TYPE_06
#  error "SIM_DATA_TYPE_06 not implemented"
#elif  SIM_DATA_TYPE == SIM_DATA_TYPE_07
#  error "SIM_DATA_TYPE_07 not implemented"
#elif  SIM_DATA_TYPE == SIM_DATA_TYPE_08
#  error "SIM_DATA_TYPE_08 not implemented"
#elif  SIM_DATA_TYPE == SIM_DATA_TYPE_09
#  error "SIM_DATA_TYPE_09 not implemented"
#elif  SIM_DATA_TYPE == SIM_DATA_TYPE_10
#  error "SIM_DATA_TYPE_10 not implemented"
#else
#  error "SIM_DATA_TYPE not defined"
#endif
