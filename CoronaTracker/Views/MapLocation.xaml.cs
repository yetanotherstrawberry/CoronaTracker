using CoronaTracker.ViewModels;
using Xamarin.Forms;

namespace CoronaTracker.Views
{
    public partial class MapLocation : ContentPage
    {

        public MapLocation()
        {
            InitializeComponent();
            BindingContext = new LocationViewModel();
        }

    }
}
