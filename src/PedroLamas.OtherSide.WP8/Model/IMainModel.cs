namespace PedroLamas.OtherSide.Model
{
    public interface IMainModel
    {
        Coordinate Position { get; set; }

        Coordinate Center { get; set; }

        double ZoomLevel { get; set; }
    }
}