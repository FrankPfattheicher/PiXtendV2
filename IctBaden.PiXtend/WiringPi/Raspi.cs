using System.Runtime.InteropServices;

namespace IctBaden.PiXtend.WiringPi
{
    public class Raspi
    {
        [DllImport("libwiringPi.so", EntryPoint = "piBoardRev")]
        public static extern int PiBoardRev();

        [DllImport("libwiringPi.so", EntryPoint = "wpiPinToGpio")]
        public static extern int WpiPinToGpio(int wPiPin);

        [DllImport("libwiringPi.so", EntryPoint = "physPinToGpio")]
        public static extern int PhysPinToGpio(int physPin);

        [DllImport("libwiringPi.so", EntryPoint = "setPadDrive")]
        public static extern int SetPadDrive(int group, int value);
    }
}