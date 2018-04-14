using System.Runtime.InteropServices;

namespace IctBaden.PiXtend.WiringPi
{
    /// <summary>
    /// Used to configure a GPIO pin's direction and provide read & write functions to a GPIO pin
    /// </summary>
    // ReSharper disable once InconsistentNaming
    public class GPIO
    {
        [DllImport("libwiringPi.so", EntryPoint = "pinMode")]           //Uses Gpio pin numbers
        public static extern void PinMode(int pin, int mode);

        [DllImport("libwiringPi.so", EntryPoint = "digitalWrite")]      //Uses Gpio pin numbers
        public static extern void DigitalWrite(int pin, int value);

        [DllImport("libwiringPi.so", EntryPoint = "digitalWriteByte")]      //Uses Gpio pin numbers
        public static extern void DigitalWriteByte(int value);

        [DllImport("libwiringPi.so", EntryPoint = "digitalRead")]           //Uses Gpio pin numbers
        public static extern int DigitalRead(int pin);

        [DllImport("libwiringPi.so", EntryPoint = "pullUpDnControl")]         //Uses Gpio pin numbers  
        public static extern void PullUpDnControl(int pin, int pud);

        //This pwm mode cannot be used when using GpioSys mode!!
        [DllImport("libwiringPi.so", EntryPoint = "pwmWrite")]              //Uses Gpio pin numbers
        public static extern void PwmWrite(int pin, int value);

        [DllImport("libwiringPi.so", EntryPoint = "pwmSetMode")]             //Uses Gpio pin numbers
        public static extern void PwmSetMode(int mode);

        [DllImport("libwiringPi.so", EntryPoint = "pwmSetRange")]             //Uses Gpio pin numbers
        public static extern void PwmSetRange(uint range);

        [DllImport("libwiringPi.so", EntryPoint = "pwmSetClock")]             //Uses Gpio pin numbers
        public static extern void PwmSetClock(int divisor);

        [DllImport("libwiringPi.so", EntryPoint = "gpioClockSet")]              //Uses Gpio pin numbers
        public static extern void ClockSetGpio(int pin, int freq);

    }
}
