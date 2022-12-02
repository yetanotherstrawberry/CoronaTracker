using CoronaTracker.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace CoronaTracker.ViewModels
{
    public class MapLocationViewModel : BaseViewModel
    {

        private Location _selectedItem;

        public ObservableCollection<Location> Items { get; }
        public Command LoadItemsCommand { get; }
        public Command AddItemCommand { get; }
        public Command<Location> ItemTapped { get; }

        public MapLocationViewModel()
        {
            Title = AppResources.History;
            Items = new ObservableCollection<Location>();
            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());
            ItemTapped = new Command<Location>(OnItemSelected);
        }

        private async Task ExecuteLoadItemsCommand()
        {
            IsBusy = true;

            try
            {
                Items.Clear();
                ICollection<Location> items = await Locations.LocationList();
                foreach (Location item in items)
                    Items.Add(item);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }

        public void OnAppearing()
        {
            IsBusy = true;
            SelectedItem = null;
        }

        public Location SelectedItem
        {
            get => _selectedItem;
            set
            {
                SetProperty(ref _selectedItem, value);
                OnItemSelected(value);
            }
        }

        private async void OnItemSelected(Location item)
        {
            if (item == null)
                return;

            await Shell.Current.GoToAsync($"{nameof(MapLocation)}?{nameof(LocationViewModel.Location)}={item.Latitude.ToString() + @"%2C" + item.Longitude.ToString()}");
        }

    }
}
