using System;
using System.Globalization;
using System.IO;
using System.Threading.Tasks;
using System.Windows;

namespace PedroLamas.OtherSide.Model
{
    public class KnownCitiesService : IKnownCitiesService
    {
        public Task<Coordinate> GetNearestCityToAsync(Coordinate coordinate)
        {
            return Task.Factory.StartNew(() =>
            {
                return GetNearestCityTo(coordinate);
            });
        }

        public Coordinate GetNearestCityTo(Coordinate coordinate)
        {
            Coordinate nearestCoordinate = null;
            var nearestDistance = double.MaxValue;

            var streamInfo = Application.GetResourceStream(new Uri("cities.txt", UriKind.Relative));

            using (streamInfo.Stream)
            {
                using (var streamReader = new StreamReader(streamInfo.Stream))
                {
                    while (!streamReader.EndOfStream)
                    {
                        var line = streamReader.ReadLine();

                        if (line.Length < 3)
                        {
                            continue;
                        }

                        var coordinateInfo = line.Split('\t');
                        var latitude = double.Parse(coordinateInfo[0], CultureInfo.InvariantCulture);
                        var longitude = double.Parse(coordinateInfo[1], CultureInfo.InvariantCulture);
                        var distance = coordinate.GetDistanceTo(latitude, longitude);

                        if (nearestDistance > distance)
                        {
                            nearestDistance = distance;

                            nearestCoordinate = new Coordinate(latitude, longitude);
                        }
                    }
                }
            }

            return nearestCoordinate;
        }
    }
}