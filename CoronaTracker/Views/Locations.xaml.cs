using CoronaTracker.ViewModels;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace CoronaTracker.Views
{
    public partial class Locations : ContentPage
    {

        readonly MapLocationViewModel _viewModel;

        public Locations()
        {
            InitializeComponent();
            BindingContext = _viewModel = new MapLocationViewModel();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            _viewModel.OnAppearing();
        }

        private void SwipeGestureRecognizer_Swiped(object sender, SwipedEventArgs e)
        {
            _viewModel.Locations.RemoveData((Location)e.Parameter);
            _viewModel.Items.Remove((Location)e.Parameter);
            _viewModel.SelectedItem = null;
        }

    }
}
