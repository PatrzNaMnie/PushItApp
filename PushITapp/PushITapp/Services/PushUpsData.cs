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

        public int PushUpsToEnd()
        {
            return NumOfPushUps() - pushUpsCompleted;
        }

        public int Proportional()
        {
            Calendar calendar = new GregorianCalendar();
            calendar.GetDayOfYear(DateTime.Now);

            return PushUpsToEnd() / (calendar.GetDaysInYear(DateTime.Now.Year) - calendar.GetDayOfYear(DateTime.Now));
        }

        public int DailyPushUps()
        {
            Calendar calendar = new GregorianCalendar();
            return calendar.GetDayOfYear(DateTime.Now);
        }

        public int GetCorrectAmount()
        {
            Calendar calendar = new GregorianCalendar();
            int pushUps = 0;

            for (int i = 1; i <= calendar.GetDayOfYear(DateTime.Now); i++)
            {
                pushUps = pushUps + i;
            }

            return pushUps - pushUpsCompleted;
        }

        public void AddPushUps(int pushUps, string hashCode)
        {
            PushUpsService.PutPushUps(UsersService.GetUser(hashCode).Result, pushUps);
            pushUpsCompleted = PushUpsService.GetPushUps(UsersService.GetUser(hashCode).Result).Result;
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
