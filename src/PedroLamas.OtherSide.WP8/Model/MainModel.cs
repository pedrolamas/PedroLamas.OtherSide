namespace PedroLamas.OtherSide.Model
{
    public class MainModel : IMainModel
    {
        private Coordinate _center = new Coordinate(0, 0);

        #region Properties

        public Coordinate Position { get; set; }

        public Coordinate Center
        {
            get
            {
                return _center;
            }
            set
            {
                _center = value;
            }
        }

        public double ZoomLevel { get; set; }

        #endregion

    }
}