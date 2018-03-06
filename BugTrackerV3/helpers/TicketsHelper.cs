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
using System.Threading.Tasks;

namespace BugTrackerV3.helpers
{
    using System.Drawing;
    using System.Web.Configuration;
    using System.Web.Mvc;

    using BugTrackerV3.Models;

    using Microsoft.Owin.Infrastructure;

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

        public List<Ticket> GetProjectTickets(int projectId)
        {
            var project = db.Projects.Find(projectId);
            return project.Tickets.ToList();
        }

        public List<Ticket> GetPMTickets()
        {
            var pmTickets = new List<Ticket>();
            var userId = HttpContext.Current.User.Identity.GetUserId();
            var pmProjects = this.projHelper.ListUserProjects(userId);

            foreach (var project in pmProjects)
            {
                foreach (var ticket in project.Tickets)
                {
                    pmTickets.Add(ticket);
                }
            }
            return pmTickets;
        }










        #region TicketHistory Helper(s)
        //Abstracting away the work necessary to record a TicketHistory

        public static bool HasTicketChanged(Ticket oldTicket, Ticket newTicket)
        {
            return !oldTicket.Equals(newTicket);
        }

        public static void AddTicketHistory(Ticket oldTicket, Ticket newTicket)
        {
            //Iterate over the properties of a Ticket Object
            foreach (var property in oldTicket.GetType().GetProperties())
            {
                //why are watchedProperties in the web.config file?
                var watchedProperties = WebConfigurationManager.AppSettings["watchedProperties"].Split(',');
                var currentProp = property.ToString().Split(' ')[1];


                if (watchedProperties.Contains(currentProp))
                {
                    if (property.GetValue(oldTicket, null) != null)
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
        }


        #endregion

        #region TicketNotification Helper(s)
        //Abstracting away the work necessary to record a TicketNotification

            //Generates a notification that is then saved in the database, separately from any emails that go out.
        public void AddTicketNotification(int ticketId, string recipientId, string message)
        {
            var notification = new TicketNotification
                                   {
                                       TicketId = ticketId,
                                       SenderId = HttpContext.Current.User.Identity.GetUserId(),
                                       RecipientId = recipientId,
                                       Body = message,
                                       DateNotified = DateTime.Now
                                   };
            db.TicketNotifications.Add(notification);
            db.SaveChanges();
        }


        public async Task GenerateNotifications(Ticket oldTicket, Ticket ticket)
        {
            //tailor message according to whether it's reassigned/assigned/unassigned etc.
            var ticketState = "";
            string subject = "";
            if (oldTicket.AssignedToUserId == null)
            {
                if (ticket.AssignedToUserId == null) ticketState = "NotAssigned";
                else ticketState = "Assigned";
            }
            else
            {
                if (ticket.AssignedToUserId == null)
                    ticketState = "Unassigned";
                else if (oldTicket.AssignedToUserId != ticket.AssignedToUserId)
                    ticketState = "Reassigned";
                //generate notification even if dev doesn't change
                else if (oldTicket.AssignedToUserId == ticket.AssignedToUserId)
                {
                    if (HasTicketChanged(oldTicket, ticket)) ticketState = "SameAssigned";
                }



            }

            switch (ticketState)
            {
                case "Assigned":
                    subject = "You have been assigned a BugTracker ticket: : \" " + ticket.Title + "\"";
                    AddTicketNotification(ticket.Id, ticket.AssignedToUserId, Utilities.BuildNotificationMessage("Assigned", ticket.Id, ticket.AssignedToUserId));
                    await Utilities.SendEmailNotification(
                        ticket.AssignedToUserId, subject,
                        Utilities.BuildNotificationMessage("Assigned", ticket.Id, ticket.AssignedToUserId));
                    break;
                case "Unassigned":
                    AddTicketNotification(
                        ticket.Id,
                        oldTicket.AssignedToUserId,
                        Utilities.BuildNotificationMessage("UnAssigned", oldTicket.Id, oldTicket.AssignedToUserId));
                    break;

                case "Reassigned":
                    subject = "You have been assigned a BugTracker ticket: \" " + ticket.Title + "\"";
                    AddTicketNotification(
                        ticket.Id,
                        ticket.AssignedToUserId,
                        Utilities.BuildNotificationMessage("Assigned", ticket.Id, ticket.AssignedToUserId));
                    await Utilities.SendEmailNotification(
                        ticket.AssignedToUserId, subject,
                        Utilities.BuildNotificationMessage("Assigned", ticket.Id, ticket.AssignedToUserId));
                    break;
                case "NotAssigned":
                    break;
                    //if dev is the same but other changes to ticket
                case "SameAssigned":
                     subject = "Update on BugTracker ticket: \" " + ticket.Title + "\"";
                    AddTicketNotification(
                        ticket.Id,
                        ticket.AssignedToUserId,
                        Utilities.BuildNotificationMessage("SameAssigned", ticket.Id,
                        ticket.AssignedToUserId));
                    await Utilities.SendEmailNotification(
                        ticket.AssignedToUserId, subject,
                        Utilities.BuildNotificationMessage("SameAssigned", ticket.Id, ticket.AssignedToUserId));
                    break;
            }
          }
        #endregion


    }
}