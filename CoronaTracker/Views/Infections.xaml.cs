using CoronaTracker.ViewModels;
using Xamarin.Forms;

namespace CoronaTracker.Views
{
    public partial class Infections : ContentPage
    {

        readonly InfectionsViewMoel _viewModel;

        public Infections()
        {
            InitializeComponent();
            BindingContext = _viewModel = new InfectionsViewMoel();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            _viewModel.OnAppearing();
        }

    }
}
