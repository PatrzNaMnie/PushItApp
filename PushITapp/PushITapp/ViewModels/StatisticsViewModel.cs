using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microcharts;
using MvvmHelpers.Commands;
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

        private SKColor ChartOfPushUpsColor = SKColor.Parse("#FF5959");
        private SKColor ChartOfDaysColor = SKColor.Parse("#A659FF");


        // ------------------------------------

        private SeriesData pushUpsSeriesData;

        private SeriesData daysSeriesData;

        private List<DateTimeData> europePopulationData;

        public SeriesData PushUpsSeriesData => pushUpsSeriesData;
        public string PushUpsCenterLabelPattern => $"Push ups\n{PushUpsService.GetPushUps(UsersService.GetUser(HashCode).Result).Result}";

        public SeriesData DaysSeriesData => daysSeriesData;
        public string DaysCenterLabelPattern => $"Days\n{pushUpsData.GetCompletedDays()}";

        public List<DateTimeData> EuropePopulationData => europePopulationData;

        // ------------------------------------

        public AsyncCommand Refresh { get; }

        public bool _isRefreshing;

        private Calendar calendar;

        private DateTime[] days;

        private int[] pushUpsValues;
        public StatisticsViewModel()
        {
            calendar = new GregorianCalendar();

            HashCode =  Services.HashCode.GetHashCode();

            pushUpsData = new PushUpsData(PushUpsService.GetPushUps(UsersService.GetUser(HashCode).Result).Result);

            pushUpsValues = HistoricalData.HistoricalValues(HashCode).ToArray();

            days = HistoricalData.EveryDayInYear().ToArray();



            // --------------------------

            pushUpsSeriesData = new SeriesData("Completed push ups", PushUpsService.GetPushUps(UsersService.GetUser(HashCode).Result).Result,
                "Push ups to go", pushUpsData.NumOfPushUps() - PushUpsService.GetPushUps(UsersService.GetUser(HashCode).Result).Result);

            daysSeriesData = new SeriesData("Completed days", pushUpsData.GetCompletedDays(),
                "Days to go", calendar.GetDaysInYear(DateTime.Now.Year) - pushUpsData.GetCompletedDays());

            europePopulationData = new List<DateTimeData>();

            for (int i = 0; i < days.Length; i++)
            {
                europePopulationData.Add(new DateTimeData(days[i], pushUpsValues[i]));
            }

            // ---------------------------

            Refresh = new AsyncCommand(RefreshStatistics);
        }

        public string HashCode
        {
            get => _hashCode;
            set => SetProperty(ref _hashCode, value);
        }

        async Task RefreshStatistics()
        {

            pushUpsValues = HistoricalData.HistoricalValues(HashCode).ToArray();

            pushUpsSeriesData = new SeriesData("Completed push ups", PushUpsService.GetPushUps(UsersService.GetUser(HashCode).Result).Result,
                "Puish ups to go", pushUpsData.NumOfPushUps() - PushUpsService.GetPushUps(UsersService.GetUser(HashCode).Result).Result);

            daysSeriesData = new SeriesData("Completed days", pushUpsData.GetCompletedDays(),
                "Days to go", calendar.GetDaysInYear(DateTime.Now.Year) - pushUpsData.GetCompletedDays());

            europePopulationData.Clear();


            for (int i = 0; i < days.Length; i++)
            {
                europePopulationData.Add(new DateTimeData(days[i], pushUpsValues[i]));
            }

            isRefreshing = false;
        }

        public bool isRefreshing
        {
            get => _isRefreshing;
            set => SetProperty(ref _isRefreshing, value);
        }
    }

}


