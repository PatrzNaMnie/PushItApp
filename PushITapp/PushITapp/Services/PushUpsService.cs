using Newtonsoft.Json;
using PushITapp.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace PushITapp.Services
{
    static class PushUpsService
    {

        //static string Baseurl = DeviceInfo.Platform == DevicePlatform.Android ?
        //                                    "http://10.0.2.2:5000" : "http://localhost:5000";

        static string BaseUrl = "http://192.168.0.157/";

        static HttpClient client;

        static PushUpsService()
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

        public static async Task<List<PushUp>> GetPushUps()
        {
            var json = client.GetStringAsync("api/users").Result;
            var pushUps = JsonConvert.DeserializeObject<List<PushUp>>(json);
            return pushUps;

        }

        public static async Task<int> GetPushUps(int id)
        {
            var pushUp =  client.GetStringAsync($"api/pushups/{id}").Result;
            var result = JsonConvert.DeserializeObject<int>(pushUp);
             
            return result;
        }

        public static async Task PutPushUps(int userId, int pushUps)
        {

            var json = client.GetStringAsync("api/pushups").Result;
            var _pushUps = JsonConvert.DeserializeObject<List<PushUp>>(json);
            var pushup = _pushUps.Find(p => p.UserId == userId);

            pushup.CompletedPushUps += pushUps;

            var jsonPu = JsonConvert.SerializeObject(pushup);
            var content = new StringContent(jsonPu, Encoding.UTF8, "application/json");

            var pushUp = await client.PutAsync($"api/pushups/{pushup.Id}", content);

        }

        public static async Task AddUPushUps()
        {
            var json = client.GetStringAsync("api/pushups").Result;
            var _pushUps = JsonConvert.DeserializeObject<List<PushUp>>(json);
            
            var pushUp = new PushUp
            {
                Id = _pushUps.FindLast(p => p.Id == p.Id).Id + 1,
                UserId = UsersService.GetUser(HashCode.GetHashCode()).Result,
                CompletedPushUps = 0
            };

            var jsonPu = JsonConvert.SerializeObject(pushUp);
            var content =
                new StringContent(jsonPu, Encoding.UTF8, "application/json");

            var response = await client.PostAsync("api/pushups", content);

            if (!response.IsSuccessStatusCode)
            {

            }
        }

    }
}
