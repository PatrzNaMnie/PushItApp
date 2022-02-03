using MvvmHelpers.Commands;
using PushITapp.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms;


namespace PushITapp.ViewModels
{
    public class PushUpsViewModel : ViewModelBase
    {
        public AsyncCommand<object> AddCommand { get; }

        public AsyncCommand Clicked;

        public string _hashCode;

        PushUpsData pushUpsData;
        //HashCode hashCode = new HashCode();

        public string _entryValue;

        public int dailyPushUps;

        public int _test;

        private CancellationTokenSource _tokenSource;
        public PushUpsViewModel()
        {

            //hashCode = new HashCode();

            //_hashCode = hashCode.GetHashCode();
            //PushUpsService.GetPushUps(1).Result
            pushUpsData = new PushUpsData(PushUpsService.GetPushUps(UsersService.GetUser(Services.HashCode.GetHashCode()).Result).Result);

            dailyPushUps = pushUpsData.GetCorrectAmount();

            AddCommand = new AsyncCommand<object>(AddPushUps);

            Clicked = new AsyncCommand(Click);

            HashCode = UsersService.GetUsers().Result.ToList().LastOrDefault().HashCode;

        }

        public int DailyPushUps
        {
            get => dailyPushUps;
            set => SetProperty(ref dailyPushUps, value);
        }

        public string HashCode
        {
            get => _hashCode;
            set => SetProperty(ref _hashCode, value);
        }

        public string EntryValue
        {
            get => _entryValue;
            set => SetProperty(ref _entryValue, value);
        }

        async Task AddPushUps(object entryValue)
        {
            var _entryValue = (string)entryValue;
            if (_entryValue != "")
            {
                if (_tokenSource != null)
                {
                    _tokenSource.Cancel();
                }
                _tokenSource = new CancellationTokenSource();

                await Task.Delay(1000);

                var value = Int32.Parse(_entryValue);
                pushUpsData.AddPushUps(value, Services.HashCode.GetHashCode());
                DailyPushUps = pushUpsData.GetCorrectAmount();
                EntryValue = "";
            }


        }

        public void Entry_TextChanged(object sender, TextChangedEventArgs e)
        {
            var value = (int)sender;
            pushUpsData.AddPushUps(value, Services.HashCode.GetHashCode());
            dailyPushUps = pushUpsData.GetCorrectAmount();
            
        }



        async Task Click()
        {
            Test = await PushUpsService.GetPushUps(1);
        }

        public int Test
        {
            get => _test;
            set => SetProperty(ref _test, value);
        }
    }
}