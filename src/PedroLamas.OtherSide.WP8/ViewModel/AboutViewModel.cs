using Cimbalino.Phone.Toolkit.Services;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;

namespace PedroLamas.OtherSide.ViewModel
{
    public class AboutViewModel : ViewModelBase
    {
        private readonly IWebBrowserService _webBrowserService;
        private readonly IMarketplaceReviewService _marketplaceReviewService;
        private readonly IMarketplaceSearchService _marketplaceSearchService;
        private readonly IShareLinkService _shareLinkService;

        #region Properties

        public RelayCommand OpenHomepageCommand { get; private set; }

        public RelayCommand OpenTwitterCommand { get; private set; }

        public RelayCommand RateApplicationCommand { get; private set; }

        public RelayCommand ShareApplicationCommand { get; private set; }

        public RelayCommand MarketplaceSearchCommand { get; private set; }

        #endregion

        public AboutViewModel(IWebBrowserService webBrowserService, IMarketplaceReviewService marketplaceReviewService, IMarketplaceSearchService marketplaceSearchService, IShareLinkService shareLinkService)
        {
            _webBrowserService = webBrowserService;
            _marketplaceReviewService = marketplaceReviewService;
            _marketplaceSearchService = marketplaceSearchService;
            _shareLinkService = shareLinkService;

            OpenHomepageCommand = new RelayCommand(OnOpenHomepageCommand);

            OpenTwitterCommand = new RelayCommand(OnOpenTwitterCommand);

            RateApplicationCommand = new RelayCommand(OnRateApplicationCommand);

            ShareApplicationCommand = new RelayCommand(OnShareApplicationCommand);

            MarketplaceSearchCommand = new RelayCommand(OnMarketplaceSearchCommand);
        }

        private void OnOpenHomepageCommand()
        {
            _webBrowserService.Show("http://www.pedrolamas.com");
        }

        private void OnOpenTwitterCommand()
        {
            _webBrowserService.Show("http://twitter.com/pedrolamas");
        }

        private void OnRateApplicationCommand()
        {
            _marketplaceReviewService.Show();
        }

        private void OnShareApplicationCommand()
        {
            _shareLinkService.Show("The Other Side", "The Other Side: view your antipode! via @pedrolamas", "http://www.windowsphone.com/s?appid=9e4d2183-9f54-494a-944f-39faae093622");
        }

        private void OnMarketplaceSearchCommand()
        {
            _marketplaceSearchService.Show("Pedro Lamas");
        }
    }
}