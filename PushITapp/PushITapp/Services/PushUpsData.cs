using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Threading.Tasks;

namespace PushITapp.Services
{
    public class PushUpsData
    {

        private int pushUpsCompleted;
        public PushUpsData(int pushUpsCompleted)
        {
            this.pushUpsCompleted = pushUpsCompleted;
        }

        // Total numer of push ups
        public int NumOfPushUps()
        {

            Calendar calendar = new GregorianCalendar();
            int pushUps = 0;

            for (int i = 1; i <= calendar.GetDaysInYear(DateTime.Now.Year); i++)
            {
                pushUps = pushUps + i;
            }

            return pushUps;
        }

        // Difference betweend completed push ups and total push ups 
        public int PushUpsToEnd()
        {
            return NumOfPushUps() - pushUpsCompleted;
        }

        // Count proportional num of push ups for same value on each day
        public int Proportional()
        {
            Calendar calendar = new GregorianCalendar();
            calendar.GetDayOfYear(DateTime.Now);

            return PushUpsToEnd() / (calendar.GetDaysInYear(DateTime.Now.Year) - calendar.GetDayOfYear(DateTime.Now));
        }

        // Get day of year - number of push ups to do
        public int DailyPushUps()
        {
            Calendar calendar = new GregorianCalendar();
            return calendar.GetDayOfYear(DateTime.Now);
        }

        // Show difference between number of pushups to do and completed pushups 
        // It helps to see how much push ups you need to do to complete day task
        public int GetCorrectAmount()
        {
            Calendar calendar = new GregorianCalendar();
            int pushUps = 0;

            for (int i = 1; i <= calendar.GetDayOfYear(DateTime.Now); i++)
            {
                pushUps = pushUps + i;
            }

            if (pushUps - pushUpsCompleted < 0)
                return 0;
            else
                return pushUps - pushUpsCompleted;
        }

        // Add push ups
        public void AddPushUps(int pushUps, string hashCode)
        {
            PushUpsService.PutPushUps(UsersService.GetUser(hashCode).Result, pushUps);
            HistoricalService.AddHistorical(pushUps, DateTime.Now);
            pushUpsCompleted = PushUpsService.GetPushUps(UsersService.GetUser(hashCode).Result).Result;
        }


        // Percentage completed push ups in total num of push ups 
        public float GetPrcOfPushUps()
        {
            return pushUpsCompleted * 100 / NumOfPushUps();
        }

        // Count how many days you complete by checking number of completed push ups
        public int GetCompletedDays()
        {

            Calendar calendar = new GregorianCalendar();
            int pushUps = 0;
            int days = 0;

            for (days = 1; days <= calendar.GetDaysInYear(DateTime.Now.Year); days++)
            {
                pushUps = pushUps + days;

                if(pushUps > pushUpsCompleted)
                {
                    break;
                    
                }
            }

            return days - 1;

        }



        //public static int MinPushToCatchUp(int pushUpsCompleted)
        //{
        //    GetCorrectAmount(pushUpsCompleted)
        //}

        //public static int DaysToCatchUp(int pushUpsCompleted)
        //{
        //    GetCorrectAmount(pushUpsCompleted)
        //}
    }
}
