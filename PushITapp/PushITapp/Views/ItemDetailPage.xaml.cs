using PushITapp.ViewModels;
using System.ComponentModel;
using Xamarin.Forms;

namespace PushITapp.Views
{
    public partial class ItemDetailPage : ContentPage
    {
        public ItemDetailPage()
        {
            InitializeComponent();
            BindingContext = new ItemDetailViewModel();
        }
    }
}