using System;
using Cimbalino.Phone.Toolkit.Extensions;

namespace PedroLamas.OtherSide.Model
{
    public class HereMapsService : IHereMapsService
    {
        private readonly string _appId;
        private readonly string _appCode;

        public HereMapsService(string appId, string appCode)
        {
            _appId = appId;
            _appCode = appCode;
        }

        public Uri BuildTileUri(double latitude, double longitude, int width, int height)
        {
            var url = "http://m.nok.it/?appid={0}&appcode={1}&c={2},{3}&w={4}&h={5}&nord".FormatWithInvariantCulture(
                _appId,
                _appCode,
                latitude,
                longitude,
                width,
                height);

            return new Uri(url);
        }

        public Uri BuildTileUri(double latitude, double longitude, int width, int height, int zoom)
        {
            var url = "http://m.nok.it/?appid={0}&appcode={1}&c={2},{3}&w={4}&h={5}&z={6}&nord".FormatWithInvariantCulture(
                _appId,
                _appCode,
                latitude,
                longitude,
                width,
                height,
                zoom);

            return new Uri(url);
        }

        public Uri BuildTileUri(double latitude, double longitude, int width, int height, int zoom, int pipZoom)
        {
            var url = "http://m.nok.it/?appid={0}&appcode={1}&c={2},{3}&w={4}&h={5}&z={6}&pip={7}&nord".FormatWithInvariantCulture(
                _appId,
                _appCode,
                latitude,
                longitude,
                width,
                height,
                zoom,
                pipZoom);

            return new Uri(url);
        }
    }
}