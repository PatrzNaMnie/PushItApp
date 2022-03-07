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

        public AsyncCommand<object> Switch { get; }

        public string _hashCode;

        PushUpsData pushUpsData;

        public string _entryValue;

        public int dailyPushUps;

        private bool proportional;

        public int correctAmount;

        public int proportionalAmount;

        private CancellationTokenSource _tokenSource;

        public string countingType;
        public PushUpsViewModel()
        {

            CountingType = "Day by day";

            AddCommand = new AsyncCommand<object>(AddPushUps);

            HashCode = Services.HashCode.GetHashCode();

            pushUpsData = new PushUpsData(PushUpsService.GetPushUps(UsersService.GetUser(HashCode).Result).Result);

            CorrectAmout = pushUpsData.GetCorrectAmount();

            Switch = new AsyncCommand<object>(switchToProportional);

            DailyPushUps = pushUpsData.DailyPushUps();

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

        public int CorrectAmout
        {
            get => correctAmount;
            set => SetProperty(ref correctAmount, value);
        }

        public string CountingType
        {
            get => countingType;
            set => SetProperty(ref countingType, value);
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
                pushUpsData.AddPushUps(value, HashCode);

                if(proportional == true)
                    CorrectAmout = pushUpsData.GetProportionalAmount(HashCode);
                else
                    CorrectAmout = pushUpsData.GetCorrectAmount();

                EntryValue = "";
            }


        }


        // Mode switch Proportional/DayByDay
        public async Task switchToProportional(object sender)
        {

            if (proportional == true)
            {
                proportional = false;

                CountingType = "Day by day";

                CorrectAmout = pushUpsData.GetCorrectAmount();
                DailyPushUps = pushUpsData.DailyPushUps();
            }
            else
            {
                proportional = true;

                CountingType = "Proportional";

                if (proportional == true)
                {
                    CorrectAmout = pushUpsData.GetProportionalAmount(HashCode);
                    DailyPushUps = pushUpsData.Proportional();
                }

            }
            
        }
    }
}