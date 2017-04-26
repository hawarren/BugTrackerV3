using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BugTrackerV3.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace BugTrackerV3.Controllers
{
    [Authorize(Roles ="Admin")]
    public class AdminController : Controller
    {

        ApplicationDbContext db = new ApplicationDbContext();
        helpers.UserRolesHelper helper = new helpers.UserRolesHelper();


        // GET: Admin
        public ActionResult Index()
        {
            List<UsersViewModel> users = new List<UsersViewModel>();
            var dbUsers = db.Users.ToList();

            foreach (var usr in dbUsers)
            {
                UsersViewModel vm = new UsersViewModel();
                vm.User = usr;
                vm.Roles = helper.ListUserRoles(usr.Id).ToList();
                users.Add(vm);
            }

            return View(users);
        }
        // GET: Admin
        public ActionResult ManageUserRoles()
        {
            List<UsersViewModel> users = new List<UsersViewModel>();
            var dbUsers = db.Users.ToList();

            foreach (var usr in dbUsers)
            {
                UsersViewModel vm = new UsersViewModel();
                vm.User = usr;
                vm.Roles = helper.ListUserRoles(usr.Id).ToList();
                users.Add(vm);
            }

            return View(users);
        }

        public ActionResult AddUserRole()
        {
            ViewBag.Users = new SelectList(db.Users, "Id", "DisplayName");
            ViewBag.Roles = new SelectList(db.Roles, "Name", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddUserRole(string Users, string Roles)
        {
            helper.AddUserToRole(Users, Roles);
            return RedirectToAction("ManageUserRoles");

        }

        public ActionResult RemoveUserRole(ApplicationUser IdUser)
        {

            // ViewBag.Users = new SelectList(db.Users, "Id", "DisplayName");
            ViewBag.User = IdUser;
            ViewBag.Roles = new SelectList(db.Roles, "Name", "Name");
            return View();
                        
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RemoveUserRole(string Users, string Roles)
        {
            if (helper.IsUserinRole(Users, Roles))
            {
                helper.RemoveUserFromRole(Users, Roles);
                return RedirectToAction("ManageUserRoles");
            }
            return RedirectToAction("RemoveUserRole");
        }

    }
}