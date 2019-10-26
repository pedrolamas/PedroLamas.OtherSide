using System;
using System.Globalization;

namespace PedroLamas.OtherSide.Model
{
    public class Coordinate
    {
        public double Latitude { get; set; }

        public double Longitude { get; set; }

        public Coordinate(double latitude, double longitude)
        {
            Latitude = latitude;
            Longitude = longitude;
        }

        public Coordinate GetAntipodeCoordinate()
        {
            return new Coordinate(-Latitude, Longitude > 0 ? Longitude - 180 : Longitude + 180);
        }

        public double GetDistanceTo(Coordinate other)
        {
            return GetDistanceTo(other.Latitude, other.Longitude);
        }

        public double GetDistanceTo(double latitude, double longitude)
        {
            var d1 = Math.Pow(Latitude - latitude, 2);
            var d2 = Math.Pow(Longitude - longitude, 2);

            return Math.Sqrt(d1 + d2);
        }

        public override string ToString()
        {
            return string.Format(CultureInfo.InvariantCulture, "{0:0.0000},{1:0.0000}", Latitude, Longitude);
        }

        public static Coordinate Parse(string coordinate)
        {
            var coords = coordinate.Split(',');

            var lat = double.Parse(coords[0], CultureInfo.InvariantCulture);
            var lng = double.Parse(coords[1], CultureInfo.InvariantCulture);

            return new Coordinate(lat, lng);
        }
    }
}