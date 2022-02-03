using PushITapp.Services;
using PushITapp.ViewModels;
using PushITapp.Views;
using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace PushITapp
{
    public partial class AppShell : Xamarin.Forms.Shell
    {
        public AppShell()
        {
            InitializeComponent();


            //AddUser();
            //AddPushUp();
            //PushUpsService.AddUPushUps();
            Routing.RegisterRoute(nameof(ItemDetailPage), typeof(ItemDetailPage));
            Routing.RegisterRoute(nameof(NewItemPage), typeof(NewItemPage));
        }

        public void AddUser()
        {
            var res = UsersService.GetUser(HashCode.GetHashCode());
            if (UsersService.GetUser(HashCode.GetHashCode()) == null)
                UsersService.AddUser();


        }

        public void AddPushUp()
        {
            var test = PushUpsService.GetPushUps().Result.Exists(p => p.UserId == UsersService.GetUser(HashCode.GetHashCode()).Result);
            if (PushUpsService.GetPushUps().Result.Exists(p=> p.UserId == UsersService.GetUser(HashCode.GetHashCode()).Result) == false)
                PushUpsService.AddUPushUps();


        }

    }
}
