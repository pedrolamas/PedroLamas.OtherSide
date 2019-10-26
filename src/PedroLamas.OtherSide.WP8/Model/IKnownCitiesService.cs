using System.Threading.Tasks;

namespace PedroLamas.OtherSide.Model
{
    public interface IKnownCitiesService
    {
        Task<Coordinate> GetNearestCityToAsync(Coordinate coordinate);

        Coordinate GetNearestCityTo(Coordinate coordinate);
    }
}