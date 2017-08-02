using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http; 

//using System.Web.UI.WebControls;

namespace BugTrackerV4
{
    public static class BugTrackerV4
    {
        public static void Register(HttpConfiguration config)
        {
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
