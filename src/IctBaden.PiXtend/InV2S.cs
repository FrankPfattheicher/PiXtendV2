
// ReSharper disable InconsistentNaming

using System.Runtime.InteropServices;

namespace IctBaden.PiXtend
{
    [StructLayout(LayoutKind.Sequential)]
    public struct InV2S
    {
        public byte byFirmware;
        public byte byHardware;
        public byte byModelIn;
        public byte byUCState;
        public byte byUCWarnings;
        public byte byDigitalIn;
        public ushort wAnalogIn0;
        public ushort wAnalogIn1;
        public byte byGPIOIn;
        public ushort wTemp0;
        public byte byTemp0Error;
        public ushort wTemp1;
        public byte byTemp1Error;
        public ushort wTemp2;
        public byte byTemp2Error;
        public ushort wTemp3;
        public byte byTemp3Error;
        public ushort wHumid0;
        public ushort wHumid1;
        public ushort wHumid2;
        public ushort wHumid3;
        public float rAnalogIn0;
        public float rAnalogIn1;
        public float rTemp0;
        public float rTemp1;
        public float rTemp2;
        public float rTemp3;
        public float rHumid0;
        public float rHumid1;
        public float rHumid2;
        public float rHumid3;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
        public byte[] abyRetainDataIn;
    }
}
