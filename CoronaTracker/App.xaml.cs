using CoronaTracker.Services;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace CoronaTracker
{
    public partial class App : Application
    {

        private void ServiceStartup()
        {
            if (Preferences.Get("backgroundService", false))
                DependencyService.Get<ILocationBackground>().BackgroundLocation();
        }

        public App()
        {
            InitializeComponent();

            DependencyService.Register<UserLocationService>();
            DependencyService.Register<InfectionService>();
            MainPage = new AppShell();
        }

        protected override void OnResume()
        {
            base.OnResume();
            ServiceStartup();
        }

        protected override void OnStart()
        {
            base.OnStart();
            ServiceStartup();
        }

    }
}
