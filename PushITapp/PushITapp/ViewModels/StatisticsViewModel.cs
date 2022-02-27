using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using Microcharts;
using ProgressRingControl.Forms.Plugin;
using PushITapp.Data;
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


        // ------------------------------------

        readonly SeriesData pushUpsSeriesData;

        readonly SeriesData daysSeriesData;

        public SeriesData PushUpsSeriesData => pushUpsSeriesData;
        public string PushUpsCenterLabelPattern => $"Push ups\n{PushUpsService.GetPushUps(UsersService.GetUser(HashCode).Result).Result}";

        public SeriesData DaysSeriesData => daysSeriesData;
        public string DaysCenterLabelPattern => $"Days\n{pushUpsData.GetCompletedDays()}";

        public List<DateTimeData> EuropePopulationData { get; }

        // ------------------------------------

        public StatisticsViewModel()
        {
            Calendar calendar = new GregorianCalendar();

            HashCode =  Services.HashCode.GetHashCode();

            pushUpsData = new PushUpsData(PushUpsService.GetPushUps(UsersService.GetUser(HashCode).Result).Result);

            var pushUpsValues = HistoricalData.HistoricalValues(HashCode).ToArray();

            var days = HistoricalData.EveryDayInYear().ToArray();

            var PushUpsChartEntries = new List<ChartEntry>();

            var DaysChartEntries = new List<ChartEntry>();


            // --------------------------

            pushUpsSeriesData = new SeriesData("Completed push ups", PushUpsService.GetPushUps(UsersService.GetUser(HashCode).Result).Result,
                "Puish ups to go", pushUpsData.NumOfPushUps() - PushUpsService.GetPushUps(UsersService.GetUser(HashCode).Result).Result);

            daysSeriesData = new SeriesData("Completed days", pushUpsData.GetCompletedDays(),
                "Days to go", calendar.GetDaysInYear(DateTime.Now.Year) - pushUpsData.GetCompletedDays());

            EuropePopulationData = new List<DateTimeData>();

            for (int i = 0; i < days.Length; i++)
            {
                EuropePopulationData.Add(new DateTimeData(days[i], pushUpsValues[i]));
            }

            // ---------------------------

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


