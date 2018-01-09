using System.Web.Mvc;
using System.Web.Routing;

namespace CloudStaff.FirePath.Api
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
        }
    }
}
