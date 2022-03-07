using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;

namespace PushITapp.ViewModels
{
    public class BodyViewModel : ViewModelBase
    {
        public BodyViewModel()
        {
            
            // Width (in pixels)
            var width = Xamarin.Essentials.DeviceDisplay.MainDisplayInfo.Width;

            // Height (in pixels)
            var height = Xamarin.Essentials.DeviceDisplay.MainDisplayInfo.Height;
        }


    }
}