using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using BugTrackerV3.Models;
using System.Web.Mvc;

namespace BugTrackerV3.helpers
{
    public class UserRolesHelper
    {
        private UserManager<ApplicationUser> userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
        private ApplicationDbContext db = new ApplicationDbContext();


        public bool IsUserinRole(string userId, string roleName)
        {
            return userManager.IsInRole(userId, roleName);
        }

        public ICollection<string> ListUserRoles(string userId)
        {
            return userManager.GetRoles(userId);
        }




        public bool AddUserToRole(string userId, string roleName)
        {
            var result = userManager.AddToRole(userId, roleName);
            return result.Succeeded;
        }

        public bool RemoveUserFromRole(string userId, string roleName)
        {
            var result = userManager.RemoveFromRole(userId, roleName);
            return result.Succeeded;
        }

        public ICollection<ApplicationUser> UsersInRole(string roleName)
        {
            var resultList = new List<ApplicationUser>();
            var List = userManager.Users.ToList();
            foreach (var user in List)
            {
                if (IsUserinRole(user.Id, roleName))
                    resultList.Add(user);
            }

            return resultList;
        }

        public ICollection<ApplicationUser> UsersNotInRole(string roleName)
        {
            var resultList = new List<ApplicationUser>();
            var List = userManager.Users.ToList();
            foreach (var user in List)
            {
                if (!IsUserinRole(user.Id, roleName))
                    resultList.Add(user);
            }
            return resultList;
        }
    }
}

/*
 * Once the class is defined, you must set up a UserManager object through which you must obtain all user-role information.
 *
 * Within the UserManager object, methods exist to help you a) determine whether a user is in a particular role
 * b) add a user to a role, and
 * c) remove a user from a role
 * While these methods already exist in the UserManager, some developers prefer to include wrappers for these methods in their own helper classes.
 * It is also possible to construct custom methods to obtain information that is not directly accessible from the UserManager, such as a list of users not currently in a particular role, or a list of users assigned to a given role.
 *
 * The processes described herein can be repeated to create any number of Helper classes to perform addiioanl repetitive operations. For example, in your BugTracker application, you must create and manage a Projects table in your database. Just as you must assign and unassign users to and from Roles,
 * you must also assign and unassign users to and from Projects. The structure of these relationships is virtually identical, and therefore the processes are likewise very similar.
 * The only difference is that there is no mechanism in the UserManager class to deal with Projects in the BugTracker. You must build that yourself.
 *
 * Consider your solution in step 5 above. Use the same thought processes you did here to devise a wya to assign and unassign users to and from Projects in the BugTracker.
 */