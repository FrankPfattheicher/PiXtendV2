using System.Runtime.InteropServices;

namespace IctBaden.PiXtend.WiringPi
{
    /// <summary>
    /// Provides use of the Timing functions such as delays
    /// </summary>
    public class Timing
    {
        [DllImport("libwiringPi.so", EntryPoint = "millis")]
        public static extern uint Millis();

        [DllImport("libwiringPi.so", EntryPoint = "micros")]
        public static extern uint Micros();

        [DllImport("libwiringPi.so", EntryPoint = "delay")]
        public static extern void Delay(uint howLong);

        [DllImport("libwiringPi.so", EntryPoint = "delayMicroseconds")]
        public static extern void DelayMicroseconds(uint howLong);
    }
}