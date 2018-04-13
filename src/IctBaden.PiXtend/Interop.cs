using System.Runtime.InteropServices;

namespace IctBaden.PiXtend
{

    public static class Interop
    {
        public const byte Model = 83;

        [DllImport("libpixtend.so", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern int Spi_AutoModeV2S([MarshalAs(UnmanagedType.Struct)] OutV2S outputData, [MarshalAs(UnmanagedType.Struct)] InV2S inputData);
    }
}
