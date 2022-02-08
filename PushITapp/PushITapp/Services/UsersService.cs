using Newtonsoft.Json;
using PushITapp.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace PushITapp.Services
{
    static class UsersService
    {

        //static string Baseurl = DeviceInfo.Platform == DevicePlatform.Android ?
        //                                    "http://10.0.2.2:5000" : "http://localhost:5000";

        static string BaseUrl = "http://192.168.0.157/";

        static HttpClient client;

        static UsersService()
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

        public static async Task<IEnumerable<Users>> GetUsers()
        {
            var json = client.GetStringAsync("api/users").Result;
            var users = JsonConvert.DeserializeObject<IEnumerable<Users>>(json);
            return users;

        }


        public static async Task<int> GetUser(string hashCode)
        {
            try
            {
                var user = client.GetStringAsync($"api/users/{hashCode}").Result;
                var result = JsonConvert.DeserializeObject<int>(user);
                return result;
            }
            catch (Exception)
            {

                return 0;
            }
            
        }

        public static async Task AddUser()
        {
            var json = client.GetStringAsync("api/users").Result;
            var users = JsonConvert.DeserializeObject<List<Users>>(json);

            var user = new Users
            {
                Id = users.FindLast(p => p.Id == p.Id).Id + 1,
                HashCode = HashCode.GetHashCode()
            };

            var jsonUs = JsonConvert.SerializeObject(user);
            var content =
                new StringContent(jsonUs, Encoding.UTF8, "application/json");

            var response = await client.PostAsync("api/users", content);

            if (!response.IsSuccessStatusCode)
            {

            }
        }

    }
}
