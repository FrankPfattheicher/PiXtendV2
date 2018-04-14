using System;
using IctBaden.PiXtend.PiXtendV2;

namespace IctBaden.PiXtend.Test
{
    internal class Program
    {
        private static void Main()
        {
            Console.WriteLine("PiXtend.Test");

            SPI.SpiControllerReset();
            Console.WriteLine("SpiControllerReset ok");

            //Setup SPI using wiringPi	
            SPI.SpiSetup(0); //use SPI device 0.0 (PiXtend V2 -S-)
            Console.WriteLine("SpiSetup(0) ok");
            SPI.SpiSetup(1); //use SPI device 0.1 (PiXtend V2 -S- DAC)
            Console.WriteLine("SpiSetup(1) ok");



            var inputs = new InputData();
            var outputs = new OutputData();
            SPI.SpiAutoMode(outputs, inputs);

            while (true)
            {
                var key = Console.ReadKey();
                if (key.KeyChar == 'x') break;

                var mask = (byte)(1 << (key.KeyChar - '0'));
                outputs.byRelayOut ^= mask;

                SPI.SpiAutoMode(outputs, inputs);
                Console.WriteLine($"Inputs = {inputs.byDigitalIn:X}");
            }

            Console.WriteLine("done.");
        }
    }
}
