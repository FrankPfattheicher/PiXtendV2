using System.Runtime.InteropServices;

namespace IctBaden.PiXtend.WiringPi
{
    /// <summary>
    /// Provides access to the I2C port
    /// </summary>
    public class I2C
    {
        [DllImport("libwiringPi.so", EntryPoint = "wiringPiI2CSetup")]
        public static extern int WiringPiI2CSetup(int devId);

        [DllImport("libwiringPi.so", EntryPoint = "wiringPiI2CRead")]
        public static extern int WiringPiI2CRead(int fd);

        [DllImport("libwiringPi.so", EntryPoint = "wiringPiI2CWrite")]
        public static extern int WiringPiI2CWrite(int fd, int data);

        [DllImport("libwiringPi.so", EntryPoint = "wiringPiI2CWriteReg8")]
        public static extern int WiringPiI2CWriteReg8(int fd, int reg, int data);

        [DllImport("libwiringPi.so", EntryPoint = "wiringPiI2CWriteReg16")]
        public static extern int WiringPiI2CWriteReg16(int fd, int reg, int data);

        [DllImport("libwiringPi.so", EntryPoint = "wiringPiI2CReadReg8")]
        public static extern int WiringPiI2CReadReg8(int fd, int reg);

        [DllImport("libwiringPi.so", EntryPoint = "wiringPiI2CReadReg16")]
        public static extern int WiringPiI2CReadReg16(int fd, int reg);
    }
}