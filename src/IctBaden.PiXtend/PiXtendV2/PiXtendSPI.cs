using IctBaden.PiXtend.WiringPi;

namespace IctBaden.PiXtend.PiXtendV2
{

    // ReSharper disable once InconsistentNaming
    public static class PiXtendSPI
    {
        public const byte ModelIdV2 = 83;

        private static ushort Crc16Calc(ushort crc, byte data)
        {
            crc ^= data;
            for (var i = 0; i < 8; ++i)
            {
                if ((crc & 1) != 0)
                {
                    crc = (ushort) ((crc >> 1) ^ 0xA001);
                }
                else
                {
                    crc = (ushort) (crc >> 1);
                }
            }
            return crc;
        }

        // ReSharper disable once InconsistentNaming
        public static int AutoModeDAC(OutputDataDAC outputDataDac)
        {
            SetAout(0, outputDataDac.AnaOut0);
            SetAout(1, outputDataDac.AnaOut1);
            return 0;
        }


        public static int AutoMode(OutputData outputData, InputData inputData)
        {
            ushort wTempValue;
            int i;
            const int spiOutputLength = 67;
            var spiOutput = new byte[spiOutputLength];
            const int spiDevice = 0;

            spiOutput[0] = outputData.byModelOut;
            spiOutput[1] = outputData.byUCMode;
            spiOutput[2] = outputData.byUCCtrl0;
            spiOutput[3] = outputData.byUCCtrl1;
            spiOutput[4] = 0;  //Reserved
            spiOutput[5] = 0;  //Reserved
            spiOutput[6] = 0;  //Reserved
            spiOutput[7] = 0; // Reserved for Header CRC value
            spiOutput[8] = 0; // Reserver for Header CRC value
            spiOutput[9] = outputData.byDigitalInDebounce01;
            spiOutput[10] = outputData.byDigitalInDebounce23;
            spiOutput[11] = outputData.byDigitalInDebounce45;
            spiOutput[12] = outputData.byDigitalInDebounce67;
            spiOutput[13] = outputData.byDigitalOut;
            spiOutput[14] = outputData.byRelayOut;
            spiOutput[15] = outputData.byGPIOCtrl;
            spiOutput[16] = outputData.byGPIOOut;
            spiOutput[17] = outputData.byGPIODebounce01;
            spiOutput[18] = outputData.byGPIODebounce23;
            spiOutput[19] = outputData.byPWM0Ctrl0;
            spiOutput[20] = (byte)(outputData.wPWM0Ctrl1 & 0xFF);
            spiOutput[21] = (byte)((outputData.wPWM0Ctrl1 >> 8) & 0xFF);
            spiOutput[22] = (byte)(outputData.wPWM0A & 0xFF);
            spiOutput[23] = (byte)((outputData.wPWM0A >> 8) & 0xFF);
            spiOutput[24] = (byte)(outputData.wPWM0B & 0xFF);
            spiOutput[25] = (byte)((outputData.wPWM0B >> 8) & 0xFF);
            spiOutput[26] = outputData.byPWM1Ctrl0;
            spiOutput[27] = outputData.byPWM1Ctrl1;
            spiOutput[28] = 0;     //Reserved
            spiOutput[29] = outputData.byPWM1A;
            spiOutput[30] = 0;     //Reserved
            spiOutput[31] = outputData.byPWM1B;
            spiOutput[32] = 0;     //Reserved

            //Add Retain data to SPI output          
            for (i = 0; i <= 31; i++)
            {
                spiOutput[33 + i] = outputData.abyRetainDataOut[i];
            }

            spiOutput[65] = 0; //Reserved for data CRC
            spiOutput[66] = 0; //Reserved for data CRC            
                                //Save physical jumper setting given by user for this call          
            var byJumper10V = outputData.byJumper10V;

            //Calculate CRC16 Header Transmit Checksum
            ushort crcSumHeader = 0xFFFF;
            for (i = 0; i < 7; i++)
            {
                crcSumHeader = Crc16Calc(crcSumHeader, spiOutput[i]);
            }
            spiOutput[7] = (byte) (crcSumHeader & 0xFF);    //CRC Low Byte
            spiOutput[8] = (byte) (crcSumHeader >> 8);  //CRC High Byte

            //Calculate CRC16 Data Transmit Checksum
            ushort crcSumData = 0xFFFF;
            for (i = 9; i < 65; i++)
            {
                crcSumData = Crc16Calc(crcSumData, spiOutput[i]);
            }
            spiOutput[65] = (byte) (crcSumData & 0xFF); //CRC Low Byte
            spiOutput[66] = (byte) (crcSumData >> 8);     //CRC High Byte

            //-------------------------------------------------------------------------
            //Initialise SPI Data Transfer with OutputData
            SPI.WiringPiSPIDataRW(spiDevice, spiOutput, spiOutputLength);
            //-------------------------------------------------------------------------

            //Calculate Header CRC16 Receive Checksum
            ushort crcSumHeaderRxCalc = 0xFFFF;
            for (i = 0; i <= 6; i++)
            {
                crcSumHeaderRxCalc = Crc16Calc(crcSumHeaderRxCalc, spiOutput[i]);
            }

            var crcSumHeaderRx = (ushort) ((spiOutput[8] << 8) + spiOutput[7]);

            //Check that CRC sums match
            if (crcSumHeaderRx != crcSumHeaderRxCalc)
                return -1;

            if (spiOutput[2] != 83)
                return -2;

            //-------------------------------------------------------------------------
            // Data received is OK, CRC and model matched
            //-------------------------------------------------------------------------
            //spiOutput now contains all returned data, assign values to InputData

            inputData.byFirmware = spiOutput[0];
            inputData.byHardware = spiOutput[1];
            inputData.byModelIn = spiOutput[2];
            inputData.byUCState = spiOutput[3];
            inputData.byUCWarnings = spiOutput[4];
            //spiOutput[5]; //Reserved
            //spiOutput[6]; //Reserved
            //spiOutput[7]; //CRC Reserved
            //spiOutput[8]; //CRC Reserved

            //Calculate Data CRC16 Receive Checksum
            ushort crcSumDataRxCalc = 0xFFFF;
            for (i = 9; i <= 64; i++)
            {
                crcSumDataRxCalc = Crc16Calc(crcSumDataRxCalc, spiOutput[i]);
            }

            var crcSumDataRx = (ushort) ((spiOutput[66] << 8) + spiOutput[65]);

            if (crcSumDataRxCalc != crcSumDataRx)
                return -3;

            inputData.byDigitalIn = spiOutput[9];
            inputData.wAnalogIn0 = (ushort) ((ushort)(spiOutput[11] << 8) | (spiOutput[10]));
            inputData.wAnalogIn1 = (ushort) ((ushort)(spiOutput[13] << 8) | (spiOutput[12]));
            inputData.byGPIOIn = spiOutput[14];
            //----------------------------------------------------------------------------------------------------
            //Check Temp0 and Humid0 for value 255, meaning read error
            if (spiOutput[16] == 255 && spiOutput[15] == 255 && spiOutput[18] == 255 && spiOutput[17] == 255)
            {
                inputData.byTemp0Error = 1;
            }
            else
            {
                inputData.wTemp0 = (ushort) ((ushort)(spiOutput[16] << 8) | (spiOutput[15]));
                inputData.wHumid0 = (ushort) ((ushort)(spiOutput[18] << 8) | (spiOutput[17]));
                inputData.byTemp0Error = 0;
            }
            //----------------------------------------------------------------------------------------------------
            //Check Temp1 and Humid1 for value 255, meaning read error
            if (spiOutput[20] == 255 && spiOutput[19] == 255 && spiOutput[22] == 255 && spiOutput[21] == 255)
            {
                inputData.byTemp1Error = 1;
            }
            else
            {
                inputData.wTemp1 = (ushort) ((ushort)(spiOutput[20] << 8) | (spiOutput[19]));
                inputData.wHumid1 = (ushort) ((ushort)(spiOutput[22] << 8) | (spiOutput[21]));
                inputData.byTemp1Error = 0;
            }
            //----------------------------------------------------------------------------------------------------
            //Check Temp2 and Humid2 for value 255, meaning read error
            if (spiOutput[24] == 255 && spiOutput[23] == 255 && spiOutput[26] == 255 && spiOutput[25] == 255)
            {
                inputData.byTemp2Error = 1;
            }
            else
            {
                inputData.wTemp2 = (ushort) ((ushort)(spiOutput[24] << 8) | (spiOutput[23]));
                inputData.wHumid2 = (ushort) ((ushort)(spiOutput[26] << 8) | (spiOutput[25]));
                inputData.byTemp2Error = 0;
            }
            //----------------------------------------------------------------------------------------------------
            //Check Temp3 and Humid3 for value 255, meaning read error
            if (spiOutput[28] == 255 && spiOutput[27] == 255 && spiOutput[30] == 255 && spiOutput[29] == 255)
            {
                inputData.byTemp3Error = 1;
            }
            else
            {
                inputData.wTemp3 = (ushort) ((ushort)(spiOutput[28] << 8) | (spiOutput[27]));
                inputData.wHumid3 = (ushort) ((ushort)(spiOutput[30] << 8) | (spiOutput[29]));
                inputData.byTemp3Error = 0;
            }
            //----------------------------------------------------------------------------------------------------
            //spiOutput[31]; //Reserved
            //spiOutput[32]; //Reserved

            if ((byJumper10V & (0b00000001)) != 0)
            {
                inputData.rAnalogIn0 = (float) (inputData.wAnalogIn0 * (10.0 / 1024));
            }
            else
            {
                inputData.rAnalogIn0 = (float) (inputData.wAnalogIn0 * (5.0 / 1024));
            }
            if ((byJumper10V & (0b00000010)) != 0)
            {
                inputData.rAnalogIn1 = (float) (inputData.wAnalogIn1 * (10.0 / 1024));
            }
            else
            {
                inputData.rAnalogIn1 = (float) (inputData.wAnalogIn1 * (5.0 / 1024));
            }

            //Check if user chose DHT11 or DHT22 sensor at GPIO0, 1 = DHT11 and 0 = DHT22
            if (outputData.byGPIO0Dht11 == 1)
            {
                inputData.rTemp0 = inputData.wTemp0 / 256.0f;
                inputData.rHumid0 = inputData.wHumid0 / 256.0f;
            }
            else
            {
                //For DHT22 sensors check bit 15, if set temperature value is negative
                wTempValue = inputData.wTemp0;
                if (((wTempValue >> 15) & 1) != 0)
                {
                    wTempValue &= 0x1000;
                    inputData.rTemp0 = (float) ((wTempValue / 10.0) * -1.0);
                }
                else
                {
                    inputData.rTemp0 = (float) (inputData.wTemp0 / 10.0);
                }
                inputData.rHumid0 = (float) (inputData.wHumid0 / 10.0);
            }
            //Check if user chose DHT11 or DHT22 sensor at GPIO1, 1 = DHT11 and 0 = DHT22
            if (outputData.byGPIO1Dht11 == 1)
            {
                inputData.rTemp1 = inputData.wTemp1 / 256.0f;
                inputData.rHumid1 = inputData.wHumid1 / 256.0f;
            }
            else
            {
                //For DHT22 sensors check bit 15, if set temperature value is negative
                wTempValue = inputData.wTemp1;
                if (((wTempValue >> 15) & 1) != 0)
                {
                    wTempValue &= 0x1000;
                    inputData.rTemp1 = (float) ((wTempValue / 10.0) * -1.0);
                }
                else
                {
                    inputData.rTemp1 = (float) (inputData.wTemp1 / 10.0);
                }
                inputData.rHumid1 = (float) (inputData.wHumid1 / 10.0);
            }
            //Check if user chose DHT11 or DHT22 sensor at GPIO2, 1 = DHT11 and 0 = DHT22
            if (outputData.byGPIO2Dht11 == 1)
            {
                inputData.rTemp2 = inputData.wTemp2 / 256.0f;
                inputData.rHumid2 = inputData.wHumid2 / 256.0f;
            }
            else
            {
                //For DHT22 sensors check bit 15, if set temperature value is negative
                wTempValue = inputData.wTemp2;
                if (((wTempValue >> 15) & 1) != 0)
                {
                    wTempValue &= 0x1000;
                    inputData.rTemp2 = (float) ((wTempValue / 10.0) * -1.0);
                }
                else
                {
                    inputData.rTemp2 = (float) (inputData.wTemp2 / 10.0);
                }
                inputData.rHumid2 = (float) (inputData.wHumid2 / 10.0);
            }
            //Check if user chose DHT11 or DHT22 sensor at GPIO3, 1 = DHT11 and 0 = DHT22
            if (outputData.byGPIO3Dht11 == 1)
            {
                inputData.rTemp3 = inputData.wTemp3 / 256.0f;
                inputData.rHumid3 = inputData.wHumid3 / 256.0f;
            }
            else
            {
                //For DHT22 sensors check bit 15, if set temperature value is negative
                wTempValue = inputData.wTemp3;
                if (((wTempValue >> 15) & 1) != 0)
                {
                    wTempValue &= 0x1000;
                    inputData.rTemp3 = (float) ((wTempValue / 10.0) * -1.0);
                }
                else
                {
                    inputData.rTemp3 = (float) (inputData.wTemp3 / 10.0);
                }
                inputData.rHumid3 = (float) (inputData.wHumid3 / 10.0);
            }
            //Get Retain data from SPI input          
            for (i = 0; i <= 31; i++)
            {
                inputData.abyRetainDataIn[i] = spiOutput[33 + i];
            }

            return 0;
        }


        public static int SetDout(byte value)
        {
            const int spiOutputLength = 4;
            var spiOutput = new byte[spiOutputLength];
            const int spiDevice = 0;

            spiOutput[0] = 0b10101010; // Handshake - begin
            spiOutput[1] = 0b00000001; // Command
            spiOutput[2] = value;
            spiOutput[3] = 0b10101010;

            return SPI.WiringPiSPIDataRW(spiDevice, spiOutput, spiOutputLength);
        }

        public static byte GetDout()
        {
            const int spiOutputLength = 4;
            var spiOutput = new byte [spiOutputLength];
            const int spiDevice = 0;

            spiOutput[0] = 0b10101010; // Handshake - begin
            spiOutput[1] = 0b00010010; // Command 18
            spiOutput[2] = 0b10101010; // readback command
            spiOutput[3] = 0b10101010; // read value

            SPI.WiringPiSPIDataRW(spiDevice, spiOutput, spiOutputLength);
            Timing.Delay(10);

            return spiOutput[3];
        }

        public static byte GetDin()
        {
            const int spiOutputLength = 4;
            var spiOutput = new byte[spiOutputLength];
            const int spiDevice = 0;

            spiOutput[0] = 0b10101010; // Handshake - begin
            spiOutput[1] = 0b00000010; // Command
            spiOutput[2] = 0b10101010; // readback command
            spiOutput[3] = 0b10101010; // read value

            SPI.WiringPiSPIDataRW(spiDevice, spiOutput, spiOutputLength);
            Timing.Delay(10);

            return spiOutput[3];
        }

        public static ushort GetAin(int index)
        {
            const int spiOutputLength = 5;
            var spiOutput = new byte[spiOutputLength];
            const int spiDevice = 0;

            for (var i = 0; i < 2; i++)
            {
                switch (index)
                {
                    case 0:
                        spiOutput[1] = 0b00000011; // Command;
                        break;
                    case 1:
                        spiOutput[1] = 0b00000100; // Command;
                        break;
                    case 2:
                        spiOutput[1] = 0b00000101; // Command;
                        break;
                    default:
                        spiOutput[1] = 0b00000110; // Command;
                        break;
                }

                spiOutput[0] = 0b10101010; // Handshake - begin
                spiOutput[2] = 0b00000000; // readback command
                spiOutput[3] = 0b00000000; // read value low
                spiOutput[4] = 0b00000000; // read value high

                SPI.WiringPiSPIDataRW(spiDevice, spiOutput, spiOutputLength);
                Timing.Delay(100);
            }

            var high = (ushort)spiOutput[3];
            var low = (ushort)(spiOutput[4] << 8);

            var output = (ushort)(high | low);

            return output;
        }

        public static int SetAout(int channel, ushort value)
        {
            const int spiOutputLength = 2;
            var spiOutput = new byte[spiOutputLength];
            const int spiDevice = 1;

            spiOutput[0] = 0b00010000;

            if (channel != 0 )
            {
                spiOutput[0] = (byte) (spiOutput[0] | 0b10000000);
            }
            if (value > 1023)
            {
                value = 1023;
            }

            var tmp = (ushort)(value & 0b1111000000);
            tmp = (ushort) (tmp >> 6);
            spiOutput[0] = (byte) (spiOutput[0] | tmp);

            tmp = (ushort) (value & 0b0000111111);
            tmp = (ushort) (tmp << 2);
            spiOutput[1] = (byte) tmp;

            return SPI.WiringPiSPIDataRW(spiDevice, spiOutput, spiOutputLength);
        }

        public static int SetRelays(byte values)
        {
            const int spiOutputLength = 4;
            var spiOutput = new byte[spiOutputLength];
            const int spiDevice = 0;

            spiOutput[0] = 0b10101010; // Handshake - begin
            spiOutput[1] = 0b00000111; // Command
            spiOutput[2] = values;
            spiOutput[3] = 0b10101010;

            return SPI.WiringPiSPIDataRW(spiDevice, spiOutput, spiOutputLength);
        }

        public static byte GetRelays()
        {
            const int spiOutputLength = 4;
            var spiOutput = new byte[spiOutputLength];
            const int spiDevice = 0;

            spiOutput[0] = 0b10101010; // Handshake - begin
            spiOutput[1] = 0b00010011; // Command 19
            spiOutput[2] = 0b10101010; // readback command
            spiOutput[3] = 0b10101010; // read value

            SPI.WiringPiSPIDataRW(spiDevice, spiOutput, spiOutputLength);
            Timing.Delay(10);

            return spiOutput[3];
        }

        public static ushort GetTemp(int index)
        {
            const int spiOutputLength = 5;
            var spiOutput = new byte[spiOutputLength];
            const int spiDevice = 0;

            for (var i = 0; i < 2; i++)
            {
                switch (index)
                {
                    case 0:
                        spiOutput[1] = 0b00001010; // Command;
                        break;
                    case 1:
                        spiOutput[1] = 0b00001011; // Command;
                        break;
                    case 2:
                        spiOutput[1] = 0b00001100; // Command;
                        break;
                    default:
                        spiOutput[1] = 0b00001101; // Command;
                        break;
                }

                spiOutput[0] = 0b10101010; // Handshake - begin
                spiOutput[2] = 0b00000000; // readback command
                spiOutput[3] = 0b00000000; // read value low
                spiOutput[4] = 0b00000000; // read value high

                SPI.WiringPiSPIDataRW(spiDevice, spiOutput, spiOutputLength);
                Timing.Delay(100);
            }

            var high = (ushort)spiOutput[3];
            var low = (ushort)(spiOutput[4] << 8);

            var output = (ushort)(high | low);
            return output;
        }

        public static ushort GetHumidity(int index)
        {
            const int spiOutputLength = 5;
            var spiOutput = new byte[spiOutputLength];
            const int spiDevice = 0;

            for (var i = 0; i < 2; i++)
            {
                switch (index)
                {
                    case 0:
                        spiOutput[1] = 0b00001110; // Command;
                        break;
                    case 1:
                        spiOutput[1] = 0b00001111; // Command;
                        break;
                    case 2:
                        spiOutput[1] = 0b00010000; // Command;
                        break;
                    default:
                        spiOutput[1] = 0b00010001; // Command;
                        break;
                }

                spiOutput[0] = 0b10101010; // Handshake - begin
                spiOutput[2] = 0b00000000; // readback command
                spiOutput[3] = 0b00000000; // read value low
                spiOutput[4] = 0b00000000; // read value high

                SPI.WiringPiSPIDataRW(spiDevice, spiOutput, spiOutputLength);
                Timing.Delay(100);
            }

            var high = (ushort)spiOutput[3];
            var low = (ushort)(spiOutput[4] << 8);

            var output = (ushort)(high | low);
            return output;
        }

        public static int SetServo(int channel, byte value)
        {
            const int spiOutputLength = 4;
            var spiOutput = new byte[spiOutputLength];
            const int spiDevice = 0;

            if (channel > 0)
            {
                spiOutput[1] = 0b10000001; // Command
            }
            else
            {
                spiOutput[1] = 0b10000000; // Command
            }

            spiOutput[0] = 0b10101010; // Handshake - begin
            spiOutput[2] = value;
            spiOutput[3] = 0b10101010;

            return SPI.WiringPiSPIDataRW(spiDevice, spiOutput, spiOutputLength);
        }

        public static int SetPwm(int channel, ushort value)
        {
            const int spiOutputLength = 5;
            var spiOutput = new byte[spiOutputLength];
            const int spiDevice = 0;

            if (channel > 0)
            {
                spiOutput[1] = 0b10000011; // Command
            }
            else
            {
                spiOutput[1] = 0b10000010; // Command
            }

            spiOutput[0] = 0b10101010; // Handshake - begin
            spiOutput[2] = (byte) (value & 0b0000000011111111);
            spiOutput[3] = (byte) ((value & 0b1111111100000000) >> 8);
            spiOutput[4] = 0b10101010;

            return SPI.WiringPiSPIDataRW(spiDevice, spiOutput, spiOutputLength);
        }

        public static int SpiSetPwmControl(byte value0, byte value1, byte value2)
        {
            const int spiOutputLength = 6;
            var spiOutput = new byte[spiOutputLength];
            const int spiDevice = 0;

            spiOutput[0] = 0b10101010; // Handshake - begin
            spiOutput[1] = 0b10000100; // Command
            spiOutput[2] = value0;
            spiOutput[3] = value1;
            spiOutput[4] = value2;
            spiOutput[5] = 0b10101010;

            return SPI.WiringPiSPIDataRW(spiDevice, spiOutput, spiOutputLength);
        }

        public static int SetGpioControl(byte value)
        {
            const int spiOutputLength = 4;
            var spiOutput = new byte[spiOutputLength];
            const int spiDevice = 0;

            spiOutput[0] = 0b10101010; // Handshake - begin
            spiOutput[1] = 0b10000101; // Command
            spiOutput[2] = value;
            spiOutput[3] = 0b10101010;

            SPI.WiringPiSPIDataRW(spiDevice, spiOutput, spiOutputLength);

            return 0;
        }

        public static int SetUcControl(byte value)
        {
            const int spiOutputLength = 4;
            var spiOutput = new byte[spiOutputLength];
            const int spiDevice = 0;

            spiOutput[0] = 0b10101010; // Handshake - begin
            spiOutput[1] = 0b10000110; // Command
            spiOutput[2] = value;
            spiOutput[3] = 0b10101010;

            return SPI.WiringPiSPIDataRW(spiDevice, spiOutput, spiOutputLength);
        }

        public static int SetAiControl(byte value0, byte value1)
        {
            const int spiOutputLength = 5;
            var spiOutput = new byte[spiOutputLength];
            const int spiDevice = 0;

            spiOutput[0] = 0b10101010; // Handshake - begin
            spiOutput[1] = 0b10000111; // Command
            spiOutput[2] = value0;
            spiOutput[3] = value1;
            spiOutput[4] = 0b10101010;

            SPI.WiringPiSPIDataRW(spiDevice, spiOutput, spiOutputLength);

            return 0;
        }

        public static int SetRaspStat(byte value)
        {
            const int spiOutputLength = 4;
            var spiOutput = new byte[spiOutputLength];
            const int spiDevice = 0;

            spiOutput[0] = 0b10101010; // Handshake - begin
            spiOutput[1] = 0b10001000; // Command
            spiOutput[2] = value;
            spiOutput[3] = 0b10101010;

            return SPI.WiringPiSPIDataRW(spiDevice, spiOutput, spiOutputLength);
        }

        /// <summary>
        /// Requires WiringPiSetup called first.
        /// </summary>
        /// <param name="spiDevice"></param>
        /// <returns></returns>
        public static int Setup(int spiDevice)
        {
            const int pinSpiEnable = 5;
            const int spiFrequency = 700000;

            GPIO.PinMode(pinSpiEnable, (int)GPIOpinmode.Output);
            GPIO.DigitalWrite(pinSpiEnable, 1);
            SPI.WiringPiSPISetup(spiDevice, spiFrequency);
            return 0;
        }

        public static int UcReset()
        {
            const int pinReset = 4;

            Init.WiringPiSetup();

            GPIO.PinMode(pinReset, (int)GPIOpinmode.Output);
            GPIO.DigitalWrite(pinReset, 1);
            Timing.Delay(1000);
            GPIO.DigitalWrite(pinReset, 0);
            return 0;
        }

        public static int GetUcStatus()
        {
            const int spiOutputLength = 4;
            var spiOutput = new byte[spiOutputLength];
            const int spiDevice = 0;

            spiOutput[0] = 0b10101010; // Handshake - begin
            spiOutput[1] = 0b10001010; // Command
            spiOutput[2] = 0b10101010; // readback command
            spiOutput[3] = 0b00000000; // read value

            SPI.WiringPiSPIDataRW(spiDevice, spiOutput, spiOutputLength);
            Timing.Delay(10);

            return spiOutput[3];
        }

        public static ushort GetUcVersion()
        {
            const int spiOutputLength = 5;
            var spiOutput = new byte[spiOutputLength];
            const int spiDevice = 0;

            spiOutput[0] = 0b10101010; // Handshake - begin
            spiOutput[1] = 0b10001001; // Command;
            spiOutput[2] = 0b00000000; // readback command
            spiOutput[3] = 0b00000000; // read value low
            spiOutput[4] = 0b00000000; // read value high

            SPI.WiringPiSPIDataRW(spiDevice, spiOutput, spiOutputLength);
            Timing.Delay(100);

            var high = (ushort)(spiOutput[4] << 8);
            var low = (ushort)spiOutput[3];

            var version = (ushort)(high | low);
            return version;
        }

        public static int SetGpio(byte values)
        {
            const int spiOutputLength = 4;
            var spiOutput = new byte[spiOutputLength];
            const int spiDevice = 0;

            spiOutput[0] = 0b10101010; // Handshake - begin
            spiOutput[1] = 0b00001000; // Command
            spiOutput[2] = values;
            spiOutput[3] = 0b10101010;

            SPI.WiringPiSPIDataRW(spiDevice, spiOutput, spiOutputLength);

            return 0;
        }

        public static byte GetGpio()
        {
            const int spiOutputLength = 4;
            var spiOutput = new byte[spiOutputLength];
            const int spiDevice = 0;

            spiOutput[0] = 0b10101010; // Handshake - begin
            spiOutput[1] = 0b00001001; // Command
            spiOutput[2] = 0b10101010; // readback command
            spiOutput[3] = 0b10101010; // read value

            SPI.WiringPiSPIDataRW(spiDevice, spiOutput, spiOutputLength);
            Timing.Delay(10);

            return spiOutput[3];
        }



    }
}
