using System;
using System.Collections.Generic;
using System.Text;

namespace PushITapp.Data
{
    public class DateTimeData
    {

        public DateTime Year { get; }
        public double Population { get; }

        public DateTimeData(DateTime year, double population)
        {
            this.Year = year;
            this.Population = population;
        }
    }
}
