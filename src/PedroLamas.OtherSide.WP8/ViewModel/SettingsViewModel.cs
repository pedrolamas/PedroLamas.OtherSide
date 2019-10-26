using Cimbalino.Phone.Toolkit.Services;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using PedroLamas.OtherSide.Model;

namespace PedroLamas.OtherSide.ViewModel
{
    public class SettingsViewModel : ViewModelBase
    {
        private readonly ISettingsModel _settingsModel;
        private readonly INavigationService _navigationService;

        private bool _allowLocationAccess;

        #region Properties

        public bool AllowLocationAccess
        {
            get
            {
                return _settingsModel.AllowLocationAccess;
            }
            set
            {
                _allowLocationAccess = value;
            }
        }

        public RelayCommand SaveCommand { get; private set; }

        #endregion

        public SettingsViewModel(ISettingsModel settingsModel, INavigationService navigationService)
        {
            _settingsModel = settingsModel;
            _navigationService = navigationService;

            _allowLocationAccess = _settingsModel.AllowLocationAccess;

            SaveCommand = new RelayCommand(() =>
            {
                _settingsModel.AllowLocationAccess = _allowLocationAccess;
                _settingsModel.Save();

                _navigationService.GoBack();
            });
        }
    }
}