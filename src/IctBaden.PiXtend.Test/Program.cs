using System;

namespace IctBaden.PiXtend.Test
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var inputs = new InV2S();
            var outputs = new OutV2S {byModelOut = Interop.Model};

            Console.WriteLine("PiXtend.Test");
            
            while (true)
            {
                var key = Console.ReadKey();
                if (key.KeyChar == 'x') break;

                if (key.KeyChar == '1')
                {
                    outputs.byRelayOut = 0x0F;
                }
                if (key.KeyChar == '0')
                {
                    outputs.byRelayOut = 0x00;
                }

                var result = Interop.Spi_AutoModeV2S(outputs, inputs);
                Console.WriteLine($"Spi_AutoModeV2S = {result}");
            }

            Console.WriteLine("done.");
        }
    }
}
