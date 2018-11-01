using System.Runtime.InteropServices;

namespace IctBaden.PiXtend.WiringPi
{
    /// <summary>
    ///  Provides the ability to use the Software Tone functions in WiringPi
    /// </summary>
    public class Tone
    {
        [DllImport("libwiringPi.so", EntryPoint = "softToneCreate")]
        public static extern int SoftToneCreate(int pin);

        [DllImport("libwiringPi.so", EntryPoint = "softToneWrite")]
        public static extern void SoftToneWrite(int pin, int freq);
    }
}
