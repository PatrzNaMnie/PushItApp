using PushITapp.Services;
using PushITapp.ViewModels;
using PushITapp.Views;
using System;
using System.Collections.Generic;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace PushITapp
{
    public partial class AppShell : Xamarin.Forms.Shell
    {
        public AppShell()
        {
            InitializeComponent();

            AddUser();
            AddPushUp();

            //Routing.RegisterRoute(nameof(PushUpsPage), typeof(PushUpsPage));
            //Routing.RegisterRoute(nameof(StatisticsPage), typeof(StatisticsPage));
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
