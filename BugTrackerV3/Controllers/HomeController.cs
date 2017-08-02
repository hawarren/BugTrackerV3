using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BugTrackerV4.helpers;
using Microsoft.AspNet.Identity;

namespace BugTrackerV4.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            var CurrentUser = User.Identity.GetUserId();
            //pass the current user if there is one
            if (CurrentUser != null)
            {
                UserRolesHelper helper = new UserRolesHelper();
                if (helper.IsUserinRole(CurrentUser, "Admin"))
                {
                    return RedirectToAction("ManageUserRoles", "Admin");
                }
            }


            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}