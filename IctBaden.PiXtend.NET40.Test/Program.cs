using System;
using System.Threading.Tasks;
using IctBaden.PiXtend.PiXtendV2;

namespace IctBaden.PiXtend.Test
{
    internal class Program
    {
        private static bool _running = true;
        private static readonly InputData Inputs = new InputData();
        private static readonly OutputData Outputs = new OutputData();

        private static void Main()
        {
            Console.WriteLine("PiXtend.Test");

            PiXtendSPI.UcReset();
            Console.WriteLine("UcReset ok");

            //Setup SPI using wiringPi	
            PiXtendSPI.Setup(0); //use SPI device 0.0 (PiXtend V2 -S-)
            PiXtendSPI.Setup(1); //use SPI device 0.1 (PiXtend V2 -S- DAC)
            PiXtendSPI.SetGpioControl(0);
            PiXtendSPI.AutoMode(Outputs, Inputs);

            var poll = Task.Run((Action) Poll);

            while (_running)
            {
                var key = Console.ReadKey();
                if (key.KeyChar == 'x') break;

                var mask = (byte)(1 << (key.KeyChar - '0'));
                Outputs.byRelayOut ^= mask;

                Console.WriteLine($"\r\nOutputs = {Outputs.byDigitalOut:X}");
            }

            _running = false;
            poll.Wait();
            Console.WriteLine("done.");
        }

        private static void Poll()
        {
            var oldInputs = PiXtendSPI.GetDin();
            var oldGpio = PiXtendSPI.GetGpio();

            while (_running)
            {
                Task.Delay(50).Wait();
                PiXtendSPI.AutoMode(Outputs, Inputs);

                if (Inputs.byGPIOIn != oldGpio)
                {
                    oldGpio = Inputs.byGPIOIn;
                    Console.WriteLine($"GPIO = {Inputs.byGPIOIn:X}");
                }

                if (Inputs.byDigitalIn != oldInputs)
                {
                    oldInputs = Inputs.byDigitalIn;
                    Console.WriteLine($"Inputs= {Inputs.byDigitalIn:X}");
                }
            }
        }
    }
}
