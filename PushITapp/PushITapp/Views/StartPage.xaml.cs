using PushITapp.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PushITapp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class StartPage : ContentPage
    {
        public StartPage()
        {
            InitializeComponent();

            Load();

        }

        async void Load()
        {

            await Task.Run(() =>
            {
                AddUser();
                AddPushUp();
                Thread.Sleep(5000);
                goToPushUpPage();
            });


        }

        void goToPushUpPage()
        {
            Shell.Current.GoToAsync("//PushUpsPage");
        }

        /// <summary>
        /// Add new user when doesn't exists in database
        /// </summary>
        public void AddUser()
        {
            var user = UsersService.GetUser(HashCode.GetHashCode()).Result;
            if (user == 0)
                UsersService.AddUser();


        }

        /// <summary>
        /// Add new push ups when doesn't exists in database
        /// </summary>
        public void AddPushUp()
        {
            var pushUp = PushUpsService.GetPushUps().Result.Exists(p => p.UserId == UsersService.GetUser(HashCode.GetHashCode()).Result);
            if (pushUp == false)
                PushUpsService.AddUPushUps();


        }
    }
}