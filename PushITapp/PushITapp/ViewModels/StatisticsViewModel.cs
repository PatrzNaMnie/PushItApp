using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using Microcharts;
using ProgressRingControl.Forms.Plugin;
using PushITapp.Services;
using SkiaSharp;
using Xamarin.Forms;

namespace PushITapp.ViewModels
{
    public class StatisticsViewModel : ViewModelBase
    {
        PushUpsData pushUpsData;

        private string _hashCode;

        public RadialGaugeChart ChartOfPushUps;

        public RadialGaugeChart ChartOfDays;


        private SKColor ChartOfPushUpsColor = SKColor.Parse("#FF5959");
        private SKColor ChartOfDaysColor = SKColor.Parse("#A659FF");




        public StatisticsViewModel()
        {
            Calendar calendar = new GregorianCalendar();

            HashCode =  Services.HashCode.GetHashCode();

            pushUpsData = new PushUpsData(PushUpsService.GetPushUps(UsersService.GetUser(HashCode).Result).Result);

            var PushUpsChartEntries = new List<ChartEntry>();

            var DaysChartEntries = new List<ChartEntry>();

            PushUpsChartEntries.Add(new ChartEntry(PushUpsService.GetPushUps(UsersService.GetUser(HashCode).Result).Result)
            {
                Color = ChartOfPushUpsColor,
                ValueLabel = $"{pushUpsData.GetPrcOfPushUps()} %",
                Label = "PushUps",
                ValueLabelColor = ChartOfPushUpsColor
            });

            DaysChartEntries.Add(new ChartEntry(pushUpsData.GetCompletedDays())
            {
                Color = ChartOfDaysColor,
                ValueLabel = $"{pushUpsData.GetCompletedDays()}",
                Label = "Days",
                ValueLabelColor = ChartOfDaysColor
            });


            ChartOfPushUps = new RadialGaugeChart { Entries = PushUpsChartEntries, LabelTextSize = 30f, MaxValue = pushUpsData.NumOfPushUps() };

            ChartOfDays = new RadialGaugeChart { Entries = DaysChartEntries, LabelTextSize = 30f, MaxValue = calendar.GetDaysInYear(DateTime.Now.Year) };
        }

        public string HashCode
        {
            get => _hashCode;
            set => SetProperty(ref _hashCode, value);
        }

        public RadialGaugeChart ChartofPushUps
        {
            get => ChartOfPushUps;
            set => SetProperty(ref ChartOfPushUps, value);
        }

        public RadialGaugeChart ChartofDays
        {

            get => ChartOfDays;
            set => SetProperty(ref ChartOfDays, value);
        }


    }

    }


