using CoronaTracker.Views;
using Xamarin.Forms;

namespace CoronaTracker
{
    public partial class AppShell : Shell
    {

        public AppShell()
        {
            InitializeComponent();

            Routing.RegisterRoute(nameof(MapLocation), typeof(MapLocation));
            Routing.RegisterRoute(nameof(MainPage), typeof(MainPage));
            Routing.RegisterRoute(nameof(Locations), typeof(Locations));
            Routing.RegisterRoute(nameof(Infections), typeof(Infections));
        }

    }
}
