using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows.Navigation;
using GalaSoft.MvvmLight.Ioc;
using PedroLamas.OtherSide.Model;
using Cimbalino.Phone.Toolkit.Extensions;

namespace PedroLamas.OtherSide
{
    public class AssociationUriMapper : UriMapperBase
    {
        public override Uri MapUri(Uri uri)
        {
            if (uri.ToString().StartsWith("/ShowPosition?"))
            {
                var queryString = QueryString(uri);

                var mainModel = SimpleIoc.Default.GetInstance<IMainModel>();

                mainModel.Position = Coordinate.Parse(queryString["p"]);
                mainModel.Center = mainModel.Position;
                mainModel.ZoomLevel = double.Parse(queryString["z"], CultureInfo.InvariantCulture);

                return new Uri("/View/MainPage.xaml", UriKind.Relative);
            }

            return uri;
        }

        public static IDictionary<string, string> QueryString(Uri uri)
        {
            var uriString = uri.ToString();
            var queryIndex = uriString.IndexOf("?", StringComparison.InvariantCulture);

            if (queryIndex == -1)
            {
                return new Dictionary<string, string>();
            }

            var query = uriString.Substring(queryIndex + 1);

            return query.Split('&')
                .Where(x => !string.IsNullOrEmpty(x))
                .Select(x => x.Split('='))
                .ToDictionary(x => x[0], x => x.Length == 2 ? System.Net.HttpUtility.UrlDecode(x[1]) : null);
        }
    }
}