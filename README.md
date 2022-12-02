# CoronaTracker
Xamarin.Forms mobile app and .NET 6 WebAPI for tracking spreading infections.

![Program image](/Screenshots/Main.png)


## Functionality
### Location tracking
Application will start a service that will track your location from time to time.
You can start and stop the service from the main page of the app at any time.

### Location history
You can check where you have been in the past by tapping the "Location history" tab.

![History](/Screenshots/UserHistory.png)

### Map
Tap on any location coordinates you see in the app to show a map.

![History](/Screenshots/Map.png)

### Reporting an infection
Tap report to send your past locations to the server, so other users will be warned about contact with you.

![History](/Screenshots/Reported.png)

### Notifications
Get a notification after an exposure has been detected.

![History](/Screenshots/Exposed.png)

### Infections map
Tap "infections" tab to show latest places exposed to diesease.

![History](/Screenshots/Infections.png)

## Background service
The app uses a background service that will work even after closing the app.
The service will show a constant notification. Stop it any time you want from the app.
Exposure notification will be sent even if the app is closed, but the service has to be running.
