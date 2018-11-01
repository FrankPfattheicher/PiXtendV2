using System.Runtime.InteropServices;

namespace IctBaden.PiXtend.WiringPi
{
    /// <summary>
    /// Provides SPI port functionality
    /// </summary>
    // ReSharper disable once InconsistentNaming
    public class SPI
    {
        /// <summary>
        /// Configures the SPI channel specified on the Raspberry Pi
        /// </summary>
        /// <param name="channel">Selects either Channel 0 or 1 for use</param>
        /// <param name="speed">Selects speed, 500,000 to 32,000,000</param>
        /// <returns>-1 for an error, or the linux file descriptor the channel uses</returns>
        [DllImport("libwiringPi.so", EntryPoint = "wiringPiSPISetup")]
        public static extern int WiringPiSPISetup(int channel, int speed);

        /// <summary>
        /// Read and Write data over the SPI bus, don't forget to configure it first
        /// </summary>
        /// <param name="channel">Selects Channel 0 or Channel 1 for this operation</param>
        /// <param name="data">signed byte array pointer which holds the data to send and will then hold the received data</param>
        /// <param name="len">How many bytes to write and read</param>
        /// <returns>-1 for an error, or the linux file descriptor the channel uses</returns>
        [DllImport("libwiringPi.so", EntryPoint = "wiringPiSPIDataRW")]
        public static extern int WiringPiSPIDataRW(int channel, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.U1, SizeParamIndex = 2)] byte[] data, int len);  //char is a signed byte
    }
}
