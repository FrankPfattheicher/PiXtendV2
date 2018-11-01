using System.Runtime.InteropServices;

namespace IctBaden.PiXtend.WiringPi
{
    public class SoftPwm
    {
        [DllImport("libwiringPi.so", EntryPoint = "softPwmCreate")]
        public static extern int Create(int pin, int initialValue, int pwmRange);

        [DllImport("libwiringPi.so", EntryPoint = "softPwmWrite")]
        public static extern void Write(int pin, int value);

        [DllImport("libwiringPi.so", EntryPoint = "softPwmStop")]
        public static extern void Stop(int pin);
    }
}