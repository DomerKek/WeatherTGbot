using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using Telegram.Bot.Types.Enums;

namespace WeatherTGbot
{
    class Program
    {
        private static string token { get; set; } = "2001343818:AAEonAeE2dC4WcdMDPzOd3SLQ3p6J_9_gUg";

        private static TelegramBotClient client;

        static string nameCity;
        static float tempOfCity;
        static string nameOfCity;

        static string answerOnWeather;

        [Obsolete]
        static void Main(string[] args)
        {
            client = new TelegramBotClient(token);
            client.StartReceiving();
            client.OnMessage += OnMessageHandler;
            Console.ReadLine();
            client.StopReceiving();
            
        }

        [Obsolete]
        private static async void OnMessageHandler(object sender, MessageEventArgs e)
        {
            var msg = e.Message;
            if (msg.Type == MessageType.Text)
            {
                switch (msg.Text)
                {
                    case "Киев":
                        var kiev = await client.SendTextMessageAsync(msg.Chat.Id,
                            $"{answerOnWeather} \n\nТемпература в {nameOfCity}: {Math.Round(tempOfCity)} °C");
                        break;
                    case "Львов":
                        var lviv = await client.SendTextMessageAsync(msg.Chat.Id,
                            $"{answerOnWeather} \n\nТемпература в {nameOfCity}: {Math.Round(tempOfCity)} °C");
                        break;
                    case "Харьков":
                        var kharkiv = await client.SendTextMessageAsync(msg.Chat.Id,
                            $"{answerOnWeather} \n\nТемпература в {nameOfCity}: {Math.Round(tempOfCity)} °C");
                        break;
                    case "Черкасы":
                        var cherkasy = await client.SendTextMessageAsync(msg.Chat.Id,
                            $"{answerOnWeather} \n\nТемпература в {nameOfCity}: {Math.Round(tempOfCity)} °C");
                        break;
                    default:
                        await client.SendTextMessageAsync(msg.Chat.Id, "Выберите команду: ", replyMarkup: GetButtons());
                        break;
                }
                nameCity = msg.Text;
                Weather(nameCity);
                Celsius(tempOfCity);

                Console.WriteLine(msg.Text); //вывод в консоль
            }
                
            //await client.SendTextMessageAsync(msg.Chat.Id, msg.Text, replyMarkup: GetButtons());
        }

        private static IReplyMarkup GetButtons()
        {
            var list = new List<KeyboardButton>() { 
                new KeyboardButton("Киев"),
                new KeyboardButton("Львов") ,
                new KeyboardButton("Харьков") };
            var markup = new ReplyKeyboardMarkup(list, resizeKeyboard: true);
            

            return markup;
            

        }
        public static void Weather(string cityName)
        {
            try
            {
                string url = "https://api.openweathermap.org/data/2.5/weather?q=" + cityName + "&appid=507c4eba27ca88ab50ac88e4e69c7981";
                HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
                HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest?.GetResponse();

                string response;

                using (StreamReader streamReader = new StreamReader(httpWebResponse.GetResponseStream()))
                {
                    response = streamReader.ReadToEnd();
                }
                WeatherResponse weatherResponse = JsonConvert.DeserializeObject<WeatherResponse> (response);

                nameOfCity = weatherResponse.Name;
                tempOfCity = weatherResponse.Main.Temp - 273;
            }
            catch (WebException)
            {
                Console.WriteLine("Возникло исключение");
                return;
            }
        }
        public static void Celsius(float celsius)
        {
            if (celsius <= 10 && celsius > 0)
            {
                answerOnWeather = "Оденься тепло!";
            }
            else if (celsius <= 0)
            {
                answerOnWeather = "На улице мороз!";
            }
            else if (celsius > 10 && celsius < 16)
            {
                answerOnWeather = "Погода благоприятная";
            }
            else
            {
                answerOnWeather = "На улице тепло!";
            }
        }
    }
}
