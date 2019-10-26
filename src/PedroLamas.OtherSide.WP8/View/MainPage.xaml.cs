using System;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Maps.Controls;
using Microsoft.Phone.Shell;
using PedroLamas.OtherSide.ViewModel;

namespace PedroLamas.OtherSide.View
{
    public partial class MainPage : PhoneApplicationPage
    {
        private const string MapsApplicationId = "9e4d2183-9f54-494a-944f-39faae093622";
        private const string MapsAuthenticationToken = "2p_jwiO3XLdRXdozcJYGBw";

        private bool _seenInstructions;

        public MainPage()
        {
            InitializeComponent();

            LocationTypeListPicker.ItemsSource = new[] { "", "Direct Antipode", "Nearest Land Mass", "Nearest City" };

            MapModeListPicker.ItemsSource = typeof(MapCartographicMode).GetFields(BindingFlags.Public | BindingFlags.Static)
                .Select(x => x.Name);
            MapModeListPicker.SelectedIndex = 0;

            ShareTypeListPicker.ItemsSource = new[] { "", "Email Message", "SMS Message", "Social Networks" };
        }

        private void MainMap_OnLoaded(object sender, RoutedEventArgs e)
        {
            Microsoft.Phone.Maps.MapsSettings.ApplicationContext.ApplicationId = MapsApplicationId;
            Microsoft.Phone.Maps.MapsSettings.ApplicationContext.AuthenticationToken = MapsAuthenticationToken;

            while (NavigationService.RemoveBackEntry() != null)
            {
            }

            if (!_seenInstructions)
            {
                _seenInstructions = true;

                MessageBox.Show("- Pinch to Zoom\n- Tap and Move to Pan\n- Tap and Hold to set custom location", "Instructions", MessageBoxButton.OK);
            }
        }

        private void ViewLocationApplicationBarItemBase_OnClick(object sender, EventArgs e)
        {
            LocationTypeListPicker.Open();
        }

        private void ShowMapModesApplicationBarIconButton_OnClick(object sender, EventArgs e)
        {
            MapModeListPicker.Open();
        }

        private void ShareApplicationBarIconButton_OnClick(object sender, EventArgs e)
        {
            ShareTypeListPicker.SelectedIndex = 0;
            ShareTypeListPicker.Open();
        }

        private void MapModeListPicker_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            MainMap.CartographicMode = (MapCartographicMode)Enum.Parse(typeof(MapCartographicMode), (string)MapModeListPicker.SelectedItem);
        }

        private void ShareTypeListPicker_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var vm = (MainViewModel)DataContext;

            switch (ShareTypeListPicker.SelectedIndex)
            {
                case 1:
                    vm.ShareByEmailCommand.Execute(null);
                    break;
                case 2:
                    vm.ShareBySmsCommand.Execute(null);
                    break;
                case 3:
                    vm.ShareOnSocialNetworkCommand.Execute(null);
                    break;
            }

            ShareTypeListPicker.SelectedIndex = 0;
        }

        private void LocationTypeListPicker_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var vm = (MainViewModel)DataContext;

            switch (LocationTypeListPicker.SelectedIndex)
            {
                case 1:
                    vm.UseAntipodeLocationCommand.Execute(null);
                    break;
                case 2:
                    vm.FindNearestLandMassCommand.Execute(null);
                    break;
                case 3:
                    vm.FindNearestCityCommand.Execute(null);
                    break;
            }

            LocationTypeListPicker.SelectedIndex = 0;
        }
    }
}