using CoronaTracker.Models;
using CoronaTracker.ViewModels;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace CoronaTracker.Views
{
    public partial class MainPage : ContentPage
    {

        public MainPage() => InitializeComponent();

        private async void ButtonCommand()
        {
            try
            {
                InfectionButton.IsVisible = false;

                var infection = Preferences.Get("infectionPreference", "");

                if (infection.Length > 0)
                {
                    InfectionButton.IsVisible = true;
                    InfectionButton.Command = new Command(async () => await Shell.Current.GoToAsync($"{nameof(MapLocation)}?{nameof(LocationViewModel.Location)}={infection}"));
                }
            }
            catch (Exception exc)
            {
                await DisplayAlert(AppResources.Error, exc.Message, AppResources.OK);
            }
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            ButtonCommand();
        }

        private async void Report(object sender, EventArgs e)
        {
            try
            {
                bool response = await DisplayAlert(AppResources.DoYouWantToReport, AppResources.HistorySent, AppResources.Yes, AppResources.No);

                if (response)
                {
                    var locations = await ((HomeViewModel)BindingContext).Locations.LocationList();
                    var jsonObject = JsonConvert.SerializeObject(locations.Select(x => new Infection { Latitude = x.Latitude, Longitude = x.Longitude, Timestamp = x.Timestamp }));
                    var content = new StringContent(jsonObject, Encoding.UTF8, "application/json");
                    var resp = await new HttpClient(){
                        Timeout = TimeSpan.FromSeconds(5),
                    }.PostAsync(string.Format(AppResources._API_POST_ENDPOINT, AppResources._API_ADDRESS), content);
                    var code = resp.StatusCode;
                    string strona = await resp.Content.ReadAsStringAsync();

                    if (code != HttpStatusCode.OK)
                        throw new Exception(code.ToString());
                    else
                        await DisplayAlert(AppResources.Done, AppResources.ServerOK, AppResources.OK);
                }
            }
            catch (Exception exc)
            {
                await DisplayAlert(AppResources.Error, exc.Message, AppResources.OK);
            }
        }

    }
}
