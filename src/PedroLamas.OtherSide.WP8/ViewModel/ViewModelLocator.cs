using System;
using Cimbalino.Phone.Toolkit.Services;
using GalaSoft.MvvmLight.Ioc;
using Microsoft.Practices.ServiceLocation;
using PedroLamas.OtherSide.Model;

namespace PedroLamas.OtherSide.ViewModel
{
    public class ViewModelLocator
    {
        public ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            Register<INavigationService, NavigationService>();
            Register<IApplicationSettingsService, ApplicationSettingsService>();
            Register<ILocationService, LocationService>();
            Register<IMessageBoxService, MessageBoxService>();
            Register<ISystemTrayService, SystemTrayService>();
            Register<ISmsComposeService, SmsComposeService>();
            Register<IEmailComposeService, EmailComposeService>();
            Register<IShareStatusService, ShareStatusService>();
            Register<IShareLinkService, ShareLinkService>();
            Register<IWebBrowserService, WebBrowserService>();
            Register<IMarketplaceReviewService, MarketplaceReviewService>();
            Register<IMarketplaceSearchService, MarketplaceSearchService>();

            Register<IKnownCitiesService, KnownCitiesService>();
            Register<ISettingsModel, SettingsModel>();
            Register<IMainModel, MainModel>();
            Register<IHereMapsService>(() => new HereMapsService("RoYAnh3cTxiIymlBXVkM", "aaxyvNaru07Yr7rW2e2_7w"));

            Register<MainViewModel>();
            Register<SettingsViewModel>();
            Register<AboutViewModel>();
        }

        public MainViewModel Main
        {
            get
            {
                return ServiceLocator.Current.GetInstance<MainViewModel>();
            }
        }

        public SettingsViewModel Settings
        {
            get
            {
                return ServiceLocator.Current.GetInstance<SettingsViewModel>();
            }
        }

        public AboutViewModel About
        {
            get
            {
                return ServiceLocator.Current.GetInstance<AboutViewModel>();
            }
        }
        
        #region Helpers

        private void Register<TInterface, TClass>()
            where TInterface : class
            where TClass : class
        {
            if (!SimpleIoc.Default.IsRegistered<TInterface>())
            {
                SimpleIoc.Default.Register<TInterface, TClass>();
            }
        }

        private void Register<TClass>() where TClass : class
        {
            if (!SimpleIoc.Default.IsRegistered<TClass>())
            {
                SimpleIoc.Default.Register<TClass>();
            }
        }

        private void Register<TClass>(Func<TClass> factory) where TClass : class
        {
            if (!SimpleIoc.Default.IsRegistered<TClass>())
            {
                SimpleIoc.Default.Register(factory);
            }
        }

        #endregion
    }
}