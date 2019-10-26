using System;

namespace PedroLamas.OtherSide.Model
{
    public interface IHereMapsService
    {
        Uri BuildTileUri(double latitude, double longitude, int width, int height);

        Uri BuildTileUri(double latitude, double longitude, int width, int height, int zoom);

        Uri BuildTileUri(double latitude, double longitude, int width, int height, int zoom, int pipZoom);
    }
}