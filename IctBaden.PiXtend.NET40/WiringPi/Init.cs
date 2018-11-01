using System.Runtime.InteropServices;

namespace IctBaden.PiXtend.WiringPi
{
    /// <summary>
    /// Used to initialise Gordon's library, there's 4 different ways to initialise and we're going to support all 4
    /// </summary>
    public class Init
    {
        [DllImport("libwiringPi.so", EntryPoint = "wiringPiSetup")]     //This is an example of how to call a method / function in a c library from c#
        public static extern int WiringPiSetup();

        [DllImport("libwiringPi.so", EntryPoint = "wiringPiSetupGpio")]
        public static extern int WiringPiSetupGpio();

        [DllImport("libwiringPi.so", EntryPoint = "wiringPiSetupSys")]
        public static extern int WiringPiSetupSys();

        [DllImport("libwiringPi.so", EntryPoint = "wiringPiSetupPhys")]
        public static extern int WiringPiSetupPhys();
    }
}