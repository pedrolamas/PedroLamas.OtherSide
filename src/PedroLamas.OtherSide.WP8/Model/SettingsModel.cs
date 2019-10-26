using Cimbalino.Phone.Toolkit.Services;

namespace PedroLamas.OtherSide.Model
{
    public class SettingsModel : ISettingsModel
    {
        private readonly IApplicationSettingsService _applicationSettingsService;

        #region Properties

        public bool AllowLocationAccess
        {
            get
            {
                return _applicationSettingsService.Get<bool>("AllowLocationAccess");
            }
            set
            {
                _applicationSettingsService.Set<bool>("AllowLocationAccess", value);
            }
        }

        #endregion

        public SettingsModel(IApplicationSettingsService applicationSettingsService)
        {
            _applicationSettingsService = applicationSettingsService;
        }

        public void Save()
        {
            if (_applicationSettingsService.IsDirty)
                _applicationSettingsService.Save();
        }
    }
}