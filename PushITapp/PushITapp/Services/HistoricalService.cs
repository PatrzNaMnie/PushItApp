using Newtonsoft.Json;
using PushITapp.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace PushITapp.Services
{
    class HistoricalService
    {
        //static string Baseurl = DeviceInfo.Platform == DevicePlatform.Android ?
        //                                    "http://10.0.2.2:5000" : "http://localhost:5000";

        static string BaseUrl = "http://192.168.0.157/";

        static HttpClient client;

        static HistoricalService()
        {
            try
            {
                client = new HttpClient
                {
                    BaseAddress = new Uri(BaseUrl)
                };
            }
            catch
            {

            }
        }

        public static async Task<IEnumerable<Historical>> GetHistorical()
        {
            var json = client.GetStringAsync("api/historical").Result;
            var historical = JsonConvert.DeserializeObject<IEnumerable<Historical>>(json);
            return historical;

        }


        public static async Task<List<Historical>> GetHistoricalByUserId(int UserId)
        {
            var historical = client.GetStringAsync($"api/historical/{UserId}").Result;
            var result = JsonConvert.DeserializeObject<List<Historical>>(historical);

            return result;
        }

        public static async Task<List<Historical>> GetHistoricalByUserIdAndDate(int UserId, string Date)
        {
            var historical = client.GetStringAsync($"api/historical/{UserId}/{Date}").Result;
            var result = JsonConvert.DeserializeObject<List<Historical>>(historical);


            return result;
        }


        public static async Task AddHistorical(int pushUps, DateTime date)
        {

            var historical = new Historical
            {
                UserId = UsersService.GetUser(HashCode.GetHashCode()).Result,
                PushUps = pushUps,
                Date = date
            };

            var jsonPu = JsonConvert.SerializeObject(historical);
            var content =
                new StringContent(jsonPu, Encoding.UTF8, "application/json");

            var response = await client.PostAsync("api/historical", content);

            if (!response.IsSuccessStatusCode)
            {

            }
        }
    }
}
