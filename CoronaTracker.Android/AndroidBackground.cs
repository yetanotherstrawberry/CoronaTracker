using Android.App;
using Android.Content;
using Android.OS;
using CoronaTracker.Droid;
using CoronaTracker.Services;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using System.Linq;

[assembly: Dependency(typeof(AndroidBackground))]
namespace CoronaTracker.Droid
{
    public class AndroidBackground : ILocationBackground
    {

        public void BackgroundLocation() {
            var context = Android.App.Application.Context;
            var intent = new Intent(context, typeof(BackgroundService));

            if (Build.VERSION.SdkInt >= BuildVersionCodes.O)
                context.StartForegroundService(intent);
            else
                context.StartService(intent);
        }

        public void StopLocationBackground()
        {
            var contect = Android.App.Application.Context;
            var intent = new Intent(contect, typeof(BackgroundService));
            contect.StopService(intent);
        }

    }

    [Service]
    public class BackgroundService : Service
    {
        private NotificationManager manager;
        private int msg_id = 1338;
        private string channelId;

        public override IBinder OnBind(Intent intent) => null;

        public override StartCommandResult OnStartCommand(Intent intent, StartCommandFlags flags, int startId)
        {
            channelId = "CoronaTrackerInfection";
            var nonImportantChannelName = "CoronaTrackerNonInfection";
            var channelName = "CoronaTracker";

            var channel = new NotificationChannel(nonImportantChannelName, channelName, NotificationImportance.None)
            {
                LockscreenVisibility = NotificationVisibility.Private
            };

            var warnings = new NotificationChannel(channelId, channelName, NotificationImportance.High)
            {
                LockscreenVisibility = NotificationVisibility.Private
            };

            manager = (NotificationManager)GetSystemService(NotificationService);
            manager.CreateNotificationChannel(channel);
            manager.CreateNotificationChannel(warnings);

            var notification = new Notification.Builder(this, nonImportantChannelName)
                .SetContentTitle(AppResources.Title)
                .SetContentText(AppResources.Tracking)
                .SetSmallIcon(Resource.Drawable.notification_bg_low_pressed)
                .SetOngoing(true)
                .Build();

            StartForeground(1337, notification);

            var timer = new Timer(async (e) =>
            {
                await DependencyService.Get<ILocationService>().CheckLocation(forceRefresh: true); // true will force the app to use GPS over and over.
                await DependencyService.Get<IInfectionService>().Download();
                await CheckForInfection();
            }, null, TimeSpan.Zero, TimeSpan.FromMinutes(1));

            Preferences.Set("backgroundService", true);

            return StartCommandResult.Sticky;
        }

        private async Task CheckForInfection()
        {
            var locations = await DependencyService.Get<ILocationService>().LocationList();
            var infections = await DependencyService.Get<IInfectionService>().List();

            var infection_location = infections.FirstOrDefault(
                x => locations.Any(
                    y => Location.CalculateDistance(x.Latitude, x.Longitude, y.Latitude, y.Longitude, DistanceUnits.Kilometers) < 0.5));

            if (infection_location != null)
            {
                manager.Notify(msg_id++, new Notification.Builder(this, channelId)
                    .SetContentTitle(AppResources.Title)
                    .SetContentText(AppResources.Exposure)
                    .SetSmallIcon(Resource.Drawable.notification_action_background)
                    .SetOngoing(false)
                    .Build());

                Preferences.Set("infectionPreference", infection_location.Latitude.ToString() + "," + infection_location.Longitude.ToString());

                if (msg_id > 200000)
                    msg_id = 1338;
            }
            else Preferences.Remove("infectionPreference");
        }

        public override void OnDestroy()
        {
            Preferences.Set("backgroundService", false);
            base.OnDestroy();
        }

    }
}
