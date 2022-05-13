using System.Device.Gpio;
using System;
using System.Net;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using Sonnette.Raspi.Models;

namespace Sonnette.RaspPi;

public class Sonnette
{
    private static readonly HttpClient client = new HttpClient();
    private static PinValue previousState = PinValue.High;

    public static void Main(string[] args)
    {
        Console.WriteLine("Hello");

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
        if (previousState != controller.Read(11)) {
            previousState = controller.Read(11);
            //Si bouton 
            if (controller.Read(11) == PinValue.Low)
            {
                SendMessage(controller);
            }
        }
    }

    public static async void SendMessage(GpioController controller)
    {

        //client.Timeout = TimeSpan.FromSeconds(30);

        Notification notif = new Notification
        {
            idNotif = 1,
            dateNotif = DateTime.Now,
            typeNotif = 4
        };

        var json = JsonSerializer.Serialize(notif);
        var data = new StringContent(json, Encoding.UTF8, "application/json");

        var url = "http://192.168.43.36:5001/Api";

        try
        {
            var response = await client.PostAsync(url, data);
            string result = response.Content.ReadAsStringAsync().Result;

            if (response.IsSuccessStatusCode)
            {
                //Allumer la led quand bouton Low
                controller.Write(10, PinValue.High);
                Thread.Sleep(1000);
                controller.Write(10, PinValue.Low);
            }
            else
            {
                for (int i = 0; i < 5; i++)
                {
                    controller.Write(10, PinValue.High);
                    Thread.Sleep(200);
                    controller.Write(10, PinValue.Low);
                    Thread.Sleep(200);
                }
            }
        } catch (HttpRequestException ex)
        {
            System.Diagnostics.Debug.WriteLine(ex.Message);
            Console.WriteLine(ex.Message);
            for (int i = 0; i < 5; i++)
            {
                controller.Write(10, PinValue.High);
                Thread.Sleep(200);
                controller.Write(10, PinValue.Low);
                Thread.Sleep(200);
            }
        }

        
    }


}
