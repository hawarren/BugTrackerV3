using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http; 

using System.Web.UI.WebControls;

namespace BugTrackerV3
{
    public static class BugTrackerV3
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
