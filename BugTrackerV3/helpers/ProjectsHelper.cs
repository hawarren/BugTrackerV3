

/* With the ProjectsHelper class, methods exist to help you
 * Assign/Unassign users to projects
 * Determine whether a user is assigned to a particular project
 * Get a list of users on a certain project
 * List projects to which a user is assigned
 */
namespace BugTrackerV3.helpers
{
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using BugTrackerV3.Models;
using System.Data.Entity.Validation;
using System.Web.Mvc;

using Microsoft.AspNet.Identity;

    public class ProjectsHelper
    {
        ApplicationDbContext db = new ApplicationDbContext();

        public List<ApplicationUser> ProjectUsersByRole(int projectId, string role)
        {
            UserRolesHelper uHelper = new UserRolesHelper();
            var users = uHelper.UsersInRole(role);
            return users.Where(m => m.Projects.Select(p => p.Id).Contains(projectId)).ToList();
        }

        public bool IsUserOnProject(string userId, int projectId)
        {
            var project = db.Projects.Find(projectId);
            var flag = project.Users.Any(u => u.Id == userId);
           // if (flag != null)
            //{
                return (flag);
            //}
            //else
            //{
            //    flag = false;
            //    return (flag);
            //}

        }

        public ICollection<Project> ListUserProjects(string userId)
        {
            ApplicationUser user = db.Users.Find(userId);

            var projects = user.Projects.ToList();
            return (projects);

        }
        public void AddUserToProject(string userId, int projectId)
        {
            if (!IsUserOnProject(userId, projectId))
            {
                Project proj = db.Projects.Find(projectId);
                var newUser = db.Users.Find(userId);

                proj.Users.Add(newUser);
                db.SaveChanges();
            }
        }

        public void RemoveUserFromProject(string userId, int projectId)
        {
            if(IsUserOnProject(userId, projectId))
                {
                Project proj = db.Projects.Find(projectId);
                var delUser = db.Users.Find(userId);

                proj.Users.Remove(delUser);
                db.Entry(proj).State = EntityState.Modified; //just saves this obj instance
                db.SaveChanges();
                }
        }

        public ICollection<ApplicationUser> ListUsersOnProject(int projectId)
        {
            return db.Projects.Find(projectId).Users;
        }

        public ICollection<ApplicationUser> ListUsersNotOnProject(int projectId)
        {
            return db.Users.Where(u => u.Projects.All(p => p.Id != projectId)).ToList();
        }


        //method to generate  tickethistory
        public void AddTicketHistory(Ticket oldTicket, Ticket newTicket)
        {
            //Each of these properties can trigger a history if they change
            var propList = new List<string>
                               {
                                   "Title",
                                   "Description",
                                   "Created",
                                   //"Updated",
                                   //"TicketTypeId",
                                   //"TicketStatusId",
                                   //"TicketPriorityId",
                                   //"AssignTouserId",
                                   //"AssignedToUserId",
                                   "ProjectId"
                               };

            //Write a for a loop that loops through the properties of a Ticket
            foreach (var property in propList)
            {
                //Having an issue with null property values...AssignToUserId
                var newValue = newTicket.GetType().GetProperty(property) == null ? "" : newTicket.GetType().GetProperty(property).GetValue(newTicket).ToString();
                var oldValue = oldTicket.GetType().GetProperty(property) == null ? "" : oldTicket.GetType().GetProperty(property).GetValue(oldTicket).ToString();

                if (newValue != oldValue)
                {
                    //Add TicketHistory
                    var newTicketHistory = new TicketHistory();
                    newTicketHistory.UserId = HttpContext.Current.User.Identity.GetUserId();
                    newTicketHistory.Changed = DateTime.Now;
                    newTicketHistory.TicketId = newTicket.Id;

                    //Record Property name and values
                    newTicketHistory.Property = property;
                    newTicketHistory.OldValue = oldValue;
                    newTicketHistory.NewValue = newValue;

                    this.db.TicketHistorys.Add(newTicketHistory);
                    db.SaveChanges();


                }
            }
        }
    }
}