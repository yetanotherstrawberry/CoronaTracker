using Xamarin.Forms;

namespace CoronaTracker.ViewModels
{
    [QueryProperty(nameof(Location), nameof(Location))]
    public class LocationViewModel : BaseViewModel
    {

        public LocationViewModel() => Title = AppResources.Map;

        private string loc;

        public string Location
        {
            get => loc;
            set => SetProperty(ref loc, value);
        }

    }
}
