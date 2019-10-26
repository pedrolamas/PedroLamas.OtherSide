using System;
using System.Collections.Generic;
using System.Device.Location;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Cimbalino.Phone.Toolkit.Extensions;
using Cimbalino.Phone.Toolkit.Services;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Threading;
using Microsoft.Phone.Maps.Services;
using Microsoft.Phone.Shell;
using PedroLamas.OtherSide.Helpers;
using PedroLamas.OtherSide.Model;
using Windows.Devices.Geolocation;

namespace PedroLamas.OtherSide.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        private const double Epsilon = 0.0001;

        private readonly IMainModel _mainModel;
        private readonly ISettingsModel _settingsModel;
        private readonly INavigationService _navigationService;
        private readonly ILocationService _locationService;
        private readonly IMessageBoxService _messageBoxService;
        private readonly ISystemTrayService _systemTrayService;
        private readonly ISmsComposeService _smsComposeService;
        private readonly IShareLinkService _shareLinkService;
        private readonly IEmailComposeService _emailComposeService;
        private readonly IHereMapsService _hereMapsService;
        private readonly IKnownCitiesService _knownCitiesService;

        #region Properties

        public RelayCommand UseCurrentLocationCommand { get; private set; }

        public RelayCommand UseAntipodeLocationCommand { get; private set; }

        public RelayCommand ShareByEmailCommand { get; private set; }

        public RelayCommand ShareBySmsCommand { get; private set; }

        public RelayCommand ShareOnSocialNetworkCommand { get; private set; }

        public RelayCommand PinToStartCommand { get; private set; }

        public RelayCommand FindNearestLandMassCommand { get; private set; }

        public RelayCommand FindNearestCityCommand { get; private set; }

        public RelayCommand ShowLocationInfoCommand { get; private set; }

        public RelayCommand ShowSettingsCommand { get; private set; }

        public RelayCommand ShowAboutCommand { get; private set; }

        public Coordinate Position
        {
            get
            {
                return _mainModel.Position;
            }
            set
            {
                if (_mainModel.Position != value)
                {
                    _mainModel.Position = value;

                    RaisePropertyChanged(() => Position);
                }
            }
        }

        public Coordinate Center
        {
            get
            {
                return _mainModel.Center;
            }
            set
            {
                if (_mainModel.Center != value)
                {
                    _mainModel.Center = value;

                    RaisePropertyChanged(() => Center);
                }
            }
        }

        public double ZoomLevel
        {
            get
            {
                return _mainModel.ZoomLevel;
            }
            set
            {
                if (Math.Abs(_mainModel.ZoomLevel - value) > Epsilon)
                {
                    _mainModel.ZoomLevel = value;

                    RaisePropertyChanged(() => ZoomLevel);
                }
            }
        }

        private Coordinate CurrentPosition
        {
            get
            {
                return Position ?? Center;
            }
        }

        #endregion

        public MainViewModel(IMainModel mainModel, ISettingsModel settingsModel, IHereMapsService hereMapsService, IKnownCitiesService knownCitiesService, INavigationService navigationService, ILocationService locationService, IMessageBoxService messageBoxService, ISystemTrayService systemTrayService, ISmsComposeService smsComposeService, IShareLinkService shareLinkService, IEmailComposeService emailComposeService)
        {
            _mainModel = mainModel;
            _settingsModel = settingsModel;
            _hereMapsService = hereMapsService;
            _knownCitiesService = knownCitiesService;
            _navigationService = navigationService;
            _locationService = locationService;
            _messageBoxService = messageBoxService;
            _systemTrayService = systemTrayService;
            _smsComposeService = smsComposeService;
            _shareLinkService = shareLinkService;
            _emailComposeService = emailComposeService;

            UseCurrentLocationCommand = new RelayCommand(UseCurrentLocation);

            UseAntipodeLocationCommand = new RelayCommand(UseAntipodeLocation);

            ShareByEmailCommand = new RelayCommand(() =>
            {
                _emailComposeService.Show("The Other Side", CurrentPosition.ToString());
            });

            ShareBySmsCommand = new RelayCommand(() =>
            {
                _smsComposeService.Show(string.Empty, "The Other Side: " + CurrentPosition.ToString());
            });

            ShareOnSocialNetworkCommand = new RelayCommand(() =>
            {
                var coordinate = CurrentPosition;
                var hereMapsUrl = "http://here.com/{0},14".FormatWithInvariantCulture(coordinate);

                _shareLinkService.Show("The Other Side", CurrentPosition.ToString(), hereMapsUrl);
            });

            PinToStartCommand = new RelayCommand(PinToStart);

            FindNearestLandMassCommand = new RelayCommand(FindNearestLandMass);

            FindNearestCityCommand = new RelayCommand(FindNearestCity);

            ShowLocationInfoCommand = new RelayCommand(ShowLocationInfo);

            ShowSettingsCommand = new RelayCommand(() =>
            {
                _navigationService.NavigateTo("/View/SettingsPage.xaml");
            });

            ShowAboutCommand = new RelayCommand(() =>
            {
                _navigationService.NavigateTo("/View/AboutPage.xaml");
            });
        }

        private async void UseCurrentLocation()
        {
            if (!_settingsModel.AllowLocationAccess)
            {
                if (_messageBoxService.Show("Allow The Other Side to get your current location?\n\nYour location information will only be used to retrieve information and will not be saved or shared with anyone.", "Location Service", System.Windows.MessageBoxButton.OKCancel) != System.Windows.MessageBoxResult.OK)
                {
                    return;
                }

                _settingsModel.AllowLocationAccess = true;
                _settingsModel.Save();
            }

            _systemTrayService.Show("Getting your current location...");

            try
            {
                var position = await _locationService.GetPositionAsync(LocationServiceAccuracy.High, TimeSpan.FromMinutes(30), TimeSpan.FromSeconds(20));

                _systemTrayService.Hide();

                Position = new Coordinate(position.Latitude, position.Longitude);
                Center = Position;
                ZoomLevel = 14;
            }
            catch (Exception)
            {
                _systemTrayService.Hide();

                _messageBoxService.Show("An error occurred while trying to get your current location!", "Error");
            }
        }

        private void UseAntipodeLocation()
        {
            Position = CurrentPosition.GetAntipodeCoordinate();
            Center = Position;
        }

        private async void FindNearestLandMass()
        {
            _systemTrayService.Show("Finding nearest land mass...");

            Position = await _knownCitiesService.GetNearestCityToAsync(CurrentPosition);

            _systemTrayService.Hide();
        }

        private async void FindNearestCity()
        {
            _systemTrayService.Show("Finding nearest city...");

            Position = await _knownCitiesService.GetNearestCityToAsync(CurrentPosition);

            _systemTrayService.Hide();
        }

        private void ShowLocationInfo()
        {
            var position = CurrentPosition;

            var query = new ReverseGeocodeQuery()
            {
                GeoCoordinate = new GeoCoordinate(position.Latitude, position.Longitude)
            };

            query.QueryCompleted += OnQueryCompleted;

            _systemTrayService.Show("Retrieving current location info...");

            query.QueryAsync();
        }

        private void OnQueryCompleted(object sender, QueryCompletedEventArgs<IList<MapLocation>> e)
        {
            _systemTrayService.Hide();

            DispatcherHelper.RunAsync(() =>
            {
                if (e.Error != null || e.Result.Count == 0)
                {
                    _messageBoxService.Show("Unable to retrieve information for the selected location", "Error");

                    return;
                }

                var locationInfo = e.Result[0].Information.Address;

                var locationInfoText = string.Empty;

                if (!string.IsNullOrEmpty(locationInfo.City))
                {
                    if (!string.IsNullOrEmpty(locationInfo.District))
                    {
                        locationInfoText += locationInfo.City + ", " + locationInfo.District + "\n";
                    }
                    else
                    {
                        locationInfoText += locationInfo.City + "\n";
                    }
                }
                else if (!string.IsNullOrEmpty(locationInfo.District))
                {
                    locationInfoText += locationInfo.District + "\n";
                }

                if (!string.IsNullOrEmpty(locationInfo.Country))
                {
                    locationInfoText += locationInfo.Country + "\n";
                }

                if (!string.IsNullOrEmpty(locationInfo.Continent))
                {
                    locationInfoText += locationInfo.Continent + "\n";
                }

                if (string.IsNullOrEmpty(locationInfoText))
                {
                    _messageBoxService.Show("Unable to retrieve information for the selected location", "Error");

                    return;
                }

                _messageBoxService.Show(locationInfoText, "Location Info");
            });
        }

        private void PinToStart()
        {
            var coordinate = CurrentPosition;

            var deepLink = string.Format(CultureInfo.InvariantCulture, "/ShowPosition?p={0}&z={1}",
                Uri.EscapeDataString(coordinate.ToString()),
                Uri.EscapeDataString(ZoomLevel.ToStringInvariantCulture()));
            var navigationUri = new Uri(deepLink, UriKind.Relative);

            if (ShellTile.ActiveTiles.Any(x => x.NavigationUri == navigationUri))
            {
                return;
            }

            var zoomLevel = (int)Math.Ceiling(ZoomLevel);

            var liveTile = new FlipTileData
            {
                SmallBackgroundImage = _hereMapsService.BuildTileUri(coordinate.Latitude, coordinate.Longitude, 159, 159, zoomLevel),
                BackgroundImage = _hereMapsService.BuildTileUri(coordinate.Latitude, coordinate.Longitude, 336, 336, zoomLevel),
                WideBackgroundImage = _hereMapsService.BuildTileUri(coordinate.Latitude, coordinate.Longitude, 691, 336, zoomLevel),
                BackContent = coordinate.ToString(),
                BackTitle = "The Other Side"
            };

            ShellTile.Create(navigationUri, liveTile, true);
        }
    }
}