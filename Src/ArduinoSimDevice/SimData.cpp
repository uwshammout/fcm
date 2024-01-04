#include "SimData.h"

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
uint16_t* pFCTotalVoltage = &holding_registers[0];
uint16_t* pFCTotalCurrentVh = &holding_registers[1];
uint16_t* pFCTotalCurrentVl = &holding_registers[2];
uint16_t* pFCCellVl = &holding_registers[3];
uint16_t* pFCCellV2 = &holding_registers[4];
uint16_t* pFCCellV3 = &holding_registers[5];
uint16_t* pFCCellV4 = &holding_registers[6];
uint16_t* pFCCellV5 = &holding_registers[7];
uint16_t* pFCCellV6 = &holding_registers[8];
uint16_t* pFCCellV7 = &holding_registers[9];
uint16_t* pFCCellV8 = &holding_registers[10];
uint16_t* pFCCellV9 = &holding_registers[11];
uint16_t* pELTotalVoltage = &holding_registers[12];
uint16_t* pELTotalCurrentVh = &holding_registers[13];
uint16_t* pELTotalCurrentVl = &holding_registers[14];


/*************************************************************/
/* Generic data                                              */
/*************************************************************/
#ifdef GENERIC_DATA

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

void fill_sim_data() {
  *pFCTotalVoltage = 0;
  *pFCTotalCurrentVh = 0;
  *pFCTotalCurrentVl = 0;
  *pFCCellVl = 0;
  *pFCCellV2 = 0;
  *pFCCellV3 = 0;
  *pFCCellV4 = 0;
  *pFCCellV5 = 0;
  *pFCCellV6 = 0;
  *pFCCellV7 = 0;
  *pFCCellV8 = 0;
  *pFCCellV9 = 0;
  *pELTotalVoltage = 0;
  *pELTotalCurrentVh = 0;
  *pELTotalCurrentVl = 0;
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