using IctBaden.PiXtend.WiringPi;

namespace IctBaden.PiXtend.PiXtendV2
{
    // ReSharper disable once InconsistentNaming
    public static class PiXtendGPIO
    {
        public static int ChangeGpioMode(int pin, GPIOpinmode mode)
        {
            Init.WiringPiSetup();
            GPIO.PinMode(pin, (int)mode);
            return 0;
        }

    }
}
