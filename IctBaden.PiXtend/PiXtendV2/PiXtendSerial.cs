using IctBaden.PiXtend.WiringPi;

namespace IctBaden.PiXtend.PiXtendV2
{
    public static class PiXtendSerial
    {
        public static int ChangeSerialMode(int mode)
        {
            const int pinSerial = 1; //Pin 1 ^= GPIO18

            Init.WiringPiSetup();
            GPIO.PinMode(pinSerial, (int)GPIOpinmode.Output);

            GPIO.DigitalWrite(pinSerial, mode == 1 ? 1 : 0);
            return 0;
        }

    }
}
