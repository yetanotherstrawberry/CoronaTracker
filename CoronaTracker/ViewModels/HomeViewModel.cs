using CoronaTracker.Services;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace CoronaTracker.ViewModels
{
    public class HomeViewModel : BaseViewModel
    {

        private string text = "";
        private bool service = false;
        public ICommand Start { get; }
        public ICommand Stop { get; }

        public string Text
        {
            get => text;
            set => SetProperty(ref text, value);
        }

        public bool IsServiceEnabled
        {
            get
            {
                SetProperty(ref service, Preferences.Get("backgroundService", false));
                return service;
            }
            set => SetProperty(ref service, value);
        }

        public HomeViewModel()
        {
            Title = AppResources.Title;
            Start = new Command(() => DependencyService.Get<ILocationBackground>().BackgroundLocation());
            Stop = new Command(() => DependencyService.Get<ILocationBackground>().StopLocationBackground());
        }

    }
}
