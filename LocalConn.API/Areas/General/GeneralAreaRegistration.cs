using System.Web.Mvc;

namespace LocalConn.API.Areas.General
{
    public class GeneralAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "General";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "General_defaults",
                "General/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}