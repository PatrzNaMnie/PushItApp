using PushITapp.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace PushITapp.Services
{
    class HistoricalData
    {
        static Calendar calendar = new GregorianCalendar();

        public static List<int> HistoricalValues(string HashCode)
        {
            List<int> historicalValues = new List<int>();

            var historicalUserValues = HistoricalService.GetHistoricalByUserId(UsersService.GetUser(HashCode).Result).Result;

            var everyDayInYear = EveryDayInYear();

            for (int i = 0; i < calendar.GetDaysInYear(DateTime.Now.Year); i++)
            {

                historicalValues.Insert(i, historicalUserValues.Where(p => p.Date.Date == everyDayInYear[i].Date).Sum(x => x.PushUps));


            }

            return historicalValues;
            
        }

        public static List<DateTime> EveryDayInYear()
        {
            List<DateTime> everyDayInYear = new List<DateTime>();

            DateTime everyDay = new DateTime(DateTime.Now.Year, 1, 1);

            for (int i = 0; i < calendar.GetDaysInYear(DateTime.Now.Year); i++)
            {
               everyDayInYear.Insert(i, everyDay.AddDays(i));

            }

            return everyDayInYear;
        }
    }
}
