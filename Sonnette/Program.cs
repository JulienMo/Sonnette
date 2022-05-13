using System.Device.Gpio;
using System.Text;
using System.Text.Json;
using Sonnette.Raspi.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;


namespace Sonnette.Raspi;

public class Sonnette
{
    private static readonly HttpClient client = new HttpClient();
    private static PinValue previousState = PinValue.High;
    private static Settings mySettings = new Settings();


    public static void Main(string[] args)
    {
        //Charger appsettings dans config
        var config = new ConfigurationBuilder().AddJsonFile("appsettings.json", false, true).Build();

        //Mettre les valeurs de AppSettings dans mySettings
        config.GetSection("AppSettings").Bind(mySettings);

        Console.WriteLine("Hello");

        GpioController controller = new GpioController(PinNumberingScheme.Board);

        //Déclaration des PIN
        controller.OpenPin(mySettings.outputPin, PinMode.Output);
        controller.OpenPin(mySettings.inputPin, PinMode.Input);


        while (true)
        {
            SwitchLed(controller);
            Thread.Sleep(100);
        }
    }

    public static void SwitchLed(GpioController controller)
    {
        if (previousState != controller.Read(mySettings.inputPin)) {
            previousState = controller.Read(mySettings.inputPin);
            //Si bouton 
            if (controller.Read(mySettings.inputPin) == PinValue.Low)
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
            idNotif = mySettings.id,
            dateNotif = DateTime.Now,
            typeNotif = 4
        };

        var json = JsonSerializer.Serialize(notif);
        var data = new StringContent(json, Encoding.UTF8, "application/json");

        var url = mySettings.url;


        DateTime dt = notif.dateNotif;
        int heureNotif = Convert.ToInt32(dt.ToString("HH"));

        if ((heureNotif < mySettings.debutNPD) || (heureNotif > mySettings.finNPD))
        {
            Console.WriteLine(dt.ToString("HH"));
            Console.WriteLine(heureNotif);
            Console.WriteLine(mySettings.debutNPD);
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
                    controller.Write(mySettings.outputPin, PinValue.High);
                    Thread.Sleep(200);
                    controller.Write(mySettings.outputPin, PinValue.Low);
                    Thread.Sleep(200);
                }
            }

        }

    }


}
