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
            //get list of user objects,
            var allUsers = db.Users.ToList();
            //for each user object check if displayname is null
            //if displayname is not null, assign the email to the displayname
            foreach (var name in allUsers)
            {
                if (name.DisplayName == null)
                {
                    name.DisplayName = name.Email;
                }

            }
            //save to the database.
            db.SaveChanges();

            return View(users);
        }

        public ActionResult AddUserRole(string IdUser)
        {
            helpers.UserRolesHelper uhelper = new helpers.UserRolesHelper();
            AdminViewModel.AdminUserViewModel myUser = new AdminViewModel.AdminUserViewModel();
            //Generate a list of roles the user is NOT in, using a foreach loop.
            List<string> nonRoles = new List<string>();
            var AllRoles = db.Roles.Select(m => m.Name);
            List<string> myRoles = uhelper.ListUserRoles(IdUser).ToList();
            foreach (var comp in AllRoles)
            {
               if (myRoles.Contains(comp) == false)
                {
                    nonRoles.Add(comp);
                }

            };
            //code to convert the list of strings to a selectlist
            ViewBag.Roles = nonRoles.Select(x =>
                                  new SelectListItem()
                                  {
                                      Text = x
                                  });
            ViewBag.CurrentRoles = myRoles.ToList();
            ViewBag.Users = new MultiSelectList(db.Users.Where(u => u.Id == IdUser), "Id", "DisplayName");
          //  ViewBag.Roles = new MultiSelectList(nonRoles, "Name", "Name");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddUserRole(string Users, string Roles)
        {
            if (!helper.IsUserinRole(Users, Roles))
            {
                helper.AddUserToRole(Users, Roles);
            return RedirectToAction("ManageUserRoles");
            }
           // return View();
            return RedirectToAction("ManageUserRoles", new { idUser = Users });

        }

        //get
        public ActionResult RemoveUserRole(string Users, string Roles)
        {
            if (helper.IsUserinRole(Users, Roles))
            {
                helper.RemoveUserFromRole(Users, Roles);
                return RedirectToAction("ManageUserRoles");
            }
            return RedirectToAction("RemoveUserRole");

            //ViewBag.Users = new SelectList(db.Users, "Id", "DisplayName");
            //ViewBag.Roles = new SelectList(db.Roles, "Name", "Name");
            //return View();

        }

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult RemoveUserRole(string Users, string Roles)
        //{
        //    if (helper.IsUserinRole(Users, Roles))
        //    {
        //        helper.RemoveUserFromRole(Users, Roles);
        //        return RedirectToAction("ManageUserRoles");
        //    }
        //    return RedirectToAction("RemoveUserRole");
        //}

    }
}