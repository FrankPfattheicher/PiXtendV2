
// ReSharper disable InconsistentNaming

using System.Runtime.InteropServices;

namespace IctBaden.PiXtend.PiXtendV2
{
    [StructLayout(LayoutKind.Sequential)]
    public class OutputData
    {
        private const int SizeRetainDataOut = 32;

        public byte byModelOut;
        public byte byUCMode;
        public byte byUCCtrl0;
        public byte byUCCtrl1;
        public byte byDigitalInDebounce01;
        public byte byDigitalInDebounce23;
        public byte byDigitalInDebounce45;
        public byte byDigitalInDebounce67;
        public byte byDigitalOut;
        public byte byRelayOut;
        public byte byGPIOCtrl;
        public byte byGPIOOut;
        public byte byGPIODebounce01;
        public byte byGPIODebounce23;
        public byte byPWM0Ctrl0;
        public ushort wPWM0Ctrl1;
        public ushort wPWM0A;
        public ushort wPWM0B;
        public byte byPWM1Ctrl0;
        public byte byPWM1Ctrl1;
        public byte byPWM1A;
        public byte byPWM1B;
        public byte byJumper10V;
        public byte byGPIO0Dht11;
        public byte byGPIO1Dht11;
        public byte byGPIO2Dht11;
        public byte byGPIO3Dht11;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = SizeRetainDataOut)]
        public byte[] abyRetainDataOut;

        public OutputData()
        {
        byModelOut = SPI.ModelIdV2;
            abyRetainDataOut = new byte[SizeRetainDataOut];
        }
    }
}
