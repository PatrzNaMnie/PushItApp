using DevExpress.XamarinForms.Charts;
using MvvmHelpers;
using PushITapp.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace PushITapp.Data
{
    public class SeriesData : BaseViewModel, IPieSeriesData
    {
        List<KeyValuePair<string, double>> data;


        public SeriesData(string firstValueLabel, double firstValue, string secondValueLabel, double secondValue)
        {
            data = new List<KeyValuePair<string, double>>() {
                new KeyValuePair<string, double>(firstValueLabel, firstValue),
                new KeyValuePair<string, double>(secondValueLabel, secondValue)
            };
        }

        public int GetDataCount() => data.Count;
        public string GetLabel(int index) => data[index].Key;
        public double GetValue(int index) => data[index].Value;
        public object GetKey(int index) => null;


    }

}
