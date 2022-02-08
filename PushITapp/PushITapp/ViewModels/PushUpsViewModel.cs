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

        public AsyncCommand<Task> Switch;

        public string _hashCode;

        PushUpsData pushUpsData;

        public string _entryValue;

        public int dailyPushUps;

        private bool proportional;


        private CancellationTokenSource _tokenSource;
        public PushUpsViewModel()
        {

            pushUpsData = new PushUpsData(PushUpsService.GetPushUps(UsersService.GetUser(Services.HashCode.GetHashCode()).Result).Result);

            DailyPushUps = pushUpsData.GetCorrectAmount();

            AddCommand = new AsyncCommand<object>(AddPushUps);

            HashCode = Services.HashCode.GetHashCode();

            Switch = new AsyncCommand<Task>(switchToProportional);

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

                if(proportional == true)
                    DailyPushUps = pushUpsData.Proportional();
                else
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

        public async Task switchToProportional(object sender)
        {

            if (proportional == true)
            {
                proportional = false;

                DailyPushUps = pushUpsData.GetCorrectAmount();
            }

            proportional = true;

            if (proportional == true)
            {
                DailyPushUps = pushUpsData.Proportional();
            }
        }
    }
}