using Cimbalino.Phone.Toolkit.Services;

namespace PedroLamas.OtherSide.Helpers
{
    public static class ISystemTrayServiceExtensions
    {
        public static void Show(this ISystemTrayService systemTrayService, string text)
        {
            systemTrayService.Text = text;
            systemTrayService.IsIndeterminate = true;
            systemTrayService.IsVisible = true;
        }

        public static void Hide(this ISystemTrayService systemTrayService)
        {
            systemTrayService.IsVisible = false;
            systemTrayService.IsIndeterminate = false;
            systemTrayService.Text = null;
        }
    }
}