using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BugTrackerV3.helpers;
using BugTrackerV3.Models;
using BugTrackerV3.ViewModels;
using Microsoft.AspNet.Identity;


namespace BugTrackerV3.Controllers
{
    [RequireHttps]
    [Authorize]
    public class DashboardVMController : Controller
    {

        private ApplicationDbContext db = new ApplicationDbContext();
        private ProjectsHelper projHelper = new ProjectsHelper();
        private TicketsHelper tixHelper = new TicketsHelper();
        private UserRolesHelper roleHelper = new UserRolesHelper();

       [Authorize]
        public ActionResult BasicDashboard()
        {
            var dashboardData = new DashboardVM();
            var userId = User.Identity.GetUserId();
            var myRole = "Guest";
            if (userId != null)
                myRole = this.roleHelper.ListUserRoles(User.Identity.GetUserId()).FirstOrDefault();

            switch (myRole)
            {
                case "Admin":
                    dashboardData.Projects = this.db.Projects.ToList();
                    dashboardData.Tickets = this.db.Tickets.ToList();
                    break;

                case "ProjectManager":
                    dashboardData.Projects = this.projHelper.ListUserProjects(userId).ToList();
                    dashboardData.Tickets = this.tixHelper.GetPMTickets();
                    break;

                case "Developer":
                    dashboardData.Projects = this.projHelper.ListUserProjects(userId).ToList();
                    dashboardData.Tickets = this.db.Tickets.Where(t => t.AssignedToUserId == userId).ToList();
                    break;
                case "Submitter": dashboardData.Tickets = this.db.Tickets.Where(t => t.OwnerUserId == userId).ToList();
                    break;
                default: ViewBag.Message = "You will not be able to see any data until you are assigned a role.";
                    break;
            }
            return View(dashboardData);
        }


        #region DefaultControllerActions



        //    // GET: DashboardVM
        //    public ActionResult Index()
        //    {
        //        return View();
        //    }

        //    // GET: DashboardVM/Details/5
        //    public ActionResult Details(int id)
        //    {
        //        return View();
        //    }

        //    // GET: DashboardVM/Create
        //    public ActionResult Create()
        //    {
        //        return View();
        //    }

        //    // POST: DashboardVM/Create
        //    [HttpPost]
        //    public ActionResult Create(FormCollection collection)
        //    {
        //        try
        //        {
        //            // TODO: Add insert logic here

        //            return RedirectToAction("Index");
        //        }
        //        catch
        //        {
        //            return View();
        //        }
        //    }

        //    // GET: DashboardVM/Edit/5
        //    public ActionResult Edit(int id)
        //    {
        //        return View();
        //    }

        //    // POST: DashboardVM/Edit/5
        //    [HttpPost]
        //    public ActionResult Edit(int id, FormCollection collection)
        //    {
        //        try
        //        {
        //            // TODO: Add update logic here

        //            return RedirectToAction("Index");
        //        }
        //        catch
        //        {
        //            return View();
        //        }
        //    }

        //    // GET: DashboardVM/Delete/5
        //    public ActionResult Delete(int id)
        //    {
        //        return View();
        //    }

        //    // POST: DashboardVM/Delete/5
        //    [HttpPost]
        //    public ActionResult Delete(int id, FormCollection collection)
        //    {
        //        try
        //        {
        //            // TODO: Add delete logic here

        //            return RedirectToAction("Index");
        //        }
        //        catch
        //        {
        //            return View();
        //        }
        //
    // }
        #endregion
    }
}
