using System.Web.Mvc;
using System.Web.Routing;

namespace CameraMap.Configs
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                "DownloadURL", // ルート名
                "{controller}/{action}/download/{FileID}/{FileName}", // パラメーター付きの URL
                new { controller = "ImageMng", action = "GetDeadFishFile" } // パラメーターの既定値
            );

        }
    }
}