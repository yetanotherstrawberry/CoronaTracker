using CoronaTracker.Models;
using CoronaTracker.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace CoronaTracker.ViewModels
{
    public class InfectionsViewMoel : BaseViewModel
    {

        private Infection _selectedItem;

        public ObservableCollection<Infection> Items { get; }
        public Command LoadItemsCommand { get; }
        public Command AddItemCommand { get; }
        public Command<Infection> ItemTapped { get; }

        public InfectionsViewMoel()
        {
            Title = AppResources.Infections;
            Items = new ObservableCollection<Infection>();
            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());
            ItemTapped = new Command<Infection>(OnItemSelected);
        }

        private async Task ExecuteLoadItemsCommand()
        {
            IsBusy = true;

            try
            {
                Items.Clear();
                foreach (var item in await Infections.List())
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

        public Infection SelectedItem
        {
            get => _selectedItem;
            set
            {
                SetProperty(ref _selectedItem, value);
                OnItemSelected(value);
            }
        }

        private async void OnItemSelected(Infection item)
        {
            if (item == null)
            {
                return;
            }
            await Shell.Current.GoToAsync($"{nameof(MapLocation)}?{nameof(LocationViewModel.Location)}={item.Latitude.ToString() + @"%2C" + item.Longitude.ToString()}");
        }

    }
}
