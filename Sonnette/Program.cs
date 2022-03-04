using System.Device.Gpio;

namespace Sonnette.RaspPi;

public class Sonnette
{

    public static void Main(string[] args)
    {
        // See https://aka.ms/new-console-template for more information

        GpioController controller = new GpioController(PinNumberingScheme.Board);

        //Déclaration des PIN
        controller.OpenPin(10, PinMode.Output);
        controller.OpenPin(11, PinMode.Input);


        while (true)
        {
            SwitchLed(controller);
            Thread.Sleep(100);
        }
    }

    public static void SwitchLed(GpioController controller)
    {
        if (controller.Read(11) == PinValue.Low)
        {
            controller.Write(10, PinValue.High);
        }
        else
        {
            controller.Write(10, PinValue.Low);
        }
    }
}
