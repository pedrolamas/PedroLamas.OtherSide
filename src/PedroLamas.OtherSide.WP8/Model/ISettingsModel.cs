namespace PedroLamas.OtherSide.Model
{
    public interface ISettingsModel
    {
        bool AllowLocationAccess { get; set; }

        void Save();
    }
}