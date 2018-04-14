using System.Runtime.InteropServices;

namespace IctBaden.PiXtend.WiringPi
{
    /// <summary>
    /// Provides access to the Thread priority and interrupts for IO
    /// </summary>
    public class PiThreadInterrupts
    {
        [DllImport("libwiringPi.so", EntryPoint = "piHiPri")]
        public static extern int PiHiPri(int priority);

        [DllImport("libwiringPi.so", EntryPoint = "waitForInterrupt")]
        public static extern int WaitForInterrupt(int pin, int timeout);

        //This is the C# equivelant to "void (*function)(void))" required by wiringPi to define a callback method
        public delegate void IsrCallback();

        [DllImport("libwiringPi.so", EntryPoint = "wiringPiISR")]
        public static extern int WiringPiISR(int pin, int mode, IsrCallback method);

        public enum InterruptLevels
        {
            IntEdgeSetup = 0,
            IntEdgeFalling = 1,
            IntEdgeRising = 2,
            IntEdgeBoth = 3
        }

        //static extern int piThreadCreate(string name);
    }
}