using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using BugTrackerV3.Models;
using System.Reflection;
using  System.Web.Configuration;

namespace BugTrackerV3.helpers
{
    using System.Web.Configuration;

    using BugTrackerV3.Models;

    public class TicketsHelper
    {
        private static ApplicationDbContext db = new ApplicationDbContext();
        private UserRolesHelper roleHelper = new UserRolesHelper();
        private ProjectsHelper projHelper = new ProjectsHelper();

        public ICollection<Ticket> GetMyTickets()
        {
            var userId = HttpContext.Current.User.Identity.GetUserId();
            var myRole = this.roleHelper.ListUserRoles(userId).FirstOrDefault();
            var myTickets = new List<Ticket>();

            switch (myRole)
            {
                case "Admin":
                case "Developer":
                    myTickets = db.Tickets.Where(t => t.AssignedToUserId == userId).ToList();
                    break;
                case "ProjectManager":
                    foreach (var project in this.projHelper.ListUserProjects(userId))
                    {

                            foreach (var ticket in db.Tickets.Where(t => t.Project.Id == project.Id))
                            {
                                myTickets.Add(ticket);
                            }
                    }
                    break;
                case "Submitter": myTickets = db.Tickets.Where(t => t.OwnerUserId == userId).ToList();
                    break;
                default: break;
            }
            return myTickets;
        }
        #region TicketHistory Helper(s)
        //Abstracting away the work necessary to record a TicketHistory

        public static bool HasTicketChanged(Ticket oldTicket, Ticket newTicket)
        {
            return !oldTicket.Equals(newTicket);
        }

        public static void GenerateTicketHistories(Ticket oldTicket, Ticket newTicket)
        {
            //Iterate over the properties of a Ticket Object
            foreach (var property in oldTicket.GetType().GetProperties())
            {
                var watchedProperties = WebConfigurationManager.AppSettings["watchedProperties"].Split(',');
                var currentProp = property.ToString().Split(' ')[1];
                if (watchedProperties.Contains(currentProp))
                {
                    var message = "Name: " + property.Name + ", Value: " + property.GetValue(oldTicket, null).ToString();
                    //Set values for old & new property values
                    var oldValue = property.GetValue(oldTicket, null).ToString();
                    var newValue = property.GetValue(newTicket, null).ToString();

                    //If the values differ it may be time to generate a TicketHistory record
                    if (oldValue != newValue)
                    {
                        var newTicketHistory = new TicketHistory();
                        newTicketHistory.Property = currentProp;
                        newTicketHistory.OldValue = oldValue.ToString();
                        newTicketHistory.NewValue = newValue.ToString();
                        newTicketHistory.Changed = DateTime.Now;
                        newTicketHistory.TicketId = oldTicket.Id;
                        newTicketHistory.UserId = HttpContext.Current.User.Identity.GetUserId();

                        db.TicketHistorys.Add(newTicketHistory);
                        db.SaveChanges();
                    }
                }
            }
        }


        #endregion

        #region TicketNotification Helper(s)
        //Abstracting away the work necessary to record a TicketNotification

        public void GenerateTicketNotification(Ticket oldTicket, Ticket newTicket)
        {

        }



        #endregion


    }
}