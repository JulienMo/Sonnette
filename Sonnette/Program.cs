using System.Device.Gpio;

namespace Sonnette.RaspPi;

public class Sonnette
{
    public static void Main(string[] args)
    {
        // See https://aka.ms/new-console-template for more information
        Console.WriteLine("Hello, World!");

        GpioController controller = new GpioController(PinNumberingScheme.Board);

        controller.OpenPin(10, PinMode.Output);

        while (true)
        {
            controller.Write(10, PinValue.Low);
            Thread.Sleep(1000);

            controller.Write(10, PinValue.High);
            Thread.Sleep(1000);
        }


    }
}
