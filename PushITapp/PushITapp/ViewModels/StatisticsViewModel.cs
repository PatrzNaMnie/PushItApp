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

        // ------------------------------------

        private SeriesData _pushUpsSeriesData;

        private SeriesData _daysSeriesData;

        private List<DateTimeData> _europePopulationData;

        public string _pushUpsCenterLabelPattern;

        public string _daysCenterLabelPattern;


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

            days = HistoricalData.ToTheseDay().ToArray();

            PushUpsCenterLabelPattern = $"Push ups\n{PushUpsService.GetPushUps(UsersService.GetUser(HashCode).Result).Result}";

            DaysCenterLabelPattern = $"Days\n{pushUpsData.GetCompletedDays()}";

            // --------------------------

            PushUpsSeriesData = new SeriesData("Completed push ups", PushUpsService.GetPushUps(UsersService.GetUser(HashCode).Result).Result,
                "Push ups to go", pushUpsData.NumOfPushUps() - PushUpsService.GetPushUps(UsersService.GetUser(HashCode).Result).Result);

            DaysSeriesData = new SeriesData("Completed days", pushUpsData.GetCompletedDays(),
                "Days to go", calendar.GetDaysInYear(DateTime.Now.Year) - pushUpsData.GetCompletedDays());

            var europePopulationData = new List<DateTimeData>();

            for (int i = 0; i < days.Length; i++)
            {
                europePopulationData.Add(new DateTimeData(days[i], pushUpsValues[i]));
            }

            EuropePopulationData = europePopulationData;
            // ---------------------------

            Refresh = new AsyncCommand(RefreshStatistics);
        }

        public string HashCode
        {
            get => _hashCode;
            set => SetProperty(ref _hashCode, value);
        }

        public SeriesData PushUpsSeriesData
        {
            get => _pushUpsSeriesData;
            set => SetProperty(ref _pushUpsSeriesData, value);
        }

        public SeriesData DaysSeriesData
        {
            get => _daysSeriesData;
            set => SetProperty(ref _daysSeriesData, value);
        }

        public List<DateTimeData> EuropePopulationData
        {
            get => _europePopulationData;
            set => SetProperty(ref _europePopulationData, value);
        }

        public string PushUpsCenterLabelPattern
        {
            get => _pushUpsCenterLabelPattern;
            set => SetProperty(ref _pushUpsCenterLabelPattern, value);
        }
        public string DaysCenterLabelPattern
        {
            get => _daysCenterLabelPattern;
            set => SetProperty(ref _daysCenterLabelPattern, value);
        }

        // Refreshing charts values Task
        async Task RefreshStatistics()
        {
            await Task.Run(()=>
            {
                pushUpsValues = HistoricalData.HistoricalValues(HashCode).ToArray();

                PushUpsSeriesData = new SeriesData("Completed push ups", PushUpsService.GetPushUps(UsersService.GetUser(HashCode).Result).Result,
                    "Puish ups to go", pushUpsData.NumOfPushUps() - PushUpsService.GetPushUps(UsersService.GetUser(HashCode).Result).Result);

                DaysSeriesData = new SeriesData("Completed days", pushUpsData.GetCompletedDays(),
                    "Days to go", calendar.GetDaysInYear(DateTime.Now.Year) - pushUpsData.GetCompletedDays());

                _europePopulationData.Clear();

                EuropePopulationData = _europePopulationData;

                var europePopulationData = new List<DateTimeData>();

                for (int i = 0; i < days.Length; i++)
                {
                    europePopulationData.Add(new DateTimeData(days[i], pushUpsValues[i]));
                }

                EuropePopulationData = europePopulationData;

                PushUpsCenterLabelPattern = $"Push ups\n{PushUpsService.GetPushUps(UsersService.GetUser(HashCode).Result).Result}";

                DaysCenterLabelPattern = $"Days\n{pushUpsData.GetCompletedDays()}";

                isRefreshing = false;
            }
            );
        }

        public bool isRefreshing
        {
            get => _isRefreshing;
            set => SetProperty(ref _isRefreshing, value);
        }
    }

}


