using System.Device.Gpio;
using System;
using System.Net;
using System.IO;

namespace Sonnette.RaspPi;

public class Sonnette
{

    public static void Main(string[] args)
    {

        GpioController controller = new GpioController(PinNumberingScheme.Board);
        string URL = "http://192.168.43.36:5001/";

        WebRequest wrGETURL = null;

        //Déclaration des PIN
        controller.OpenPin(10, PinMode.Output);
        controller.OpenPin(11, PinMode.Input);


        while (true)
        {
            SwitchLed(controller, wrGETURL, URL);
            Thread.Sleep(100);
        }
    }

    public static void SwitchLed(GpioController controller, WebRequest wrGETURL, string URL)
    {
        if (controller.Read(11) == PinValue.Low)
        {
            controller.Write(10, PinValue.High);
            wrGETURL = WebRequest.Create(URL);
        }
        else
        {
            controller.Write(10, PinValue.Low);
        }
    }

    public static void SendMessage(WebRequest wrGETURL)
    {
    
    
    }


}
