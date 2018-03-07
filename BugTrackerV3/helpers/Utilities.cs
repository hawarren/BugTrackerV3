using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Web.Configuration;
using System.Web.Mvc.Html;

namespace BugTrackerV3.helpers
{
    using System.Security.Policy;
    using System.Web.Mvc;

    using BugTrackerV3.Models;

    public class Utilities
    {
        private static ApplicationDbContext db = new ApplicationDbContext();

        public static List<string> BuildPropertyHistoryList()
        {
            var propertyList = new List<string>();
            var properties = WebConfigurationManager.AppSettings["propertylist"];
            foreach (var property in properties.Split(','))
            {
                propertyList.Add(property);
            }
            return propertyList;
        }

        public static string GetPriorityById(int id)
        {
            return db.TicketPrioritys.Find(id).Name;
        }

        public static string GetTypeById(int id)
        {
            return db.TicketTypes.Find(id).Name;
        }

        public static string GetStatusById(int id)
        {
            return db.TicketStatus.Find(id).Name;
        }
        // Builds messages and concatenates to string for delivery in an email or other notification
        public static string BuildNotificationMessage(string msgType, int ticketId, string recipientId)
        {
            var ticket = db.Tickets.Find(ticketId);

            var message = new StringBuilder();

            message.AppendFormat("Dear {0},", db.Users.Find(recipientId).FirstName);
            message.AppendLine(System.Environment.NewLine);
            //alter message based on whether dev has been assigned or unassigned from a ticket
            if (msgType == "SameAssigned")
            {
                message.AppendFormat("Please be advised that the ticket referenced herein has been changed. You are still the developer on this ticket, and here are the following details \n");
            }
            else
            {
            message.AppendFormat(
                "You have been {0} a Ticket. Please review the following details \n",
                msgType == "Assigned" ? " have been assigned to a " : " have been unassigned from ");

            }

            message.AppendLine(System.Environment.NewLine);

            message.AppendFormat("Assignment Date: {0}", DateTime.Now);
            message.AppendLine(System.Environment.NewLine);

            message.AppendFormat("Ticket Id: {0}", ticket.Id);
            message.AppendLine(System.Environment.NewLine);

            message.AppendFormat("Ticket Title: {0}", ticket.Title);
            message.AppendLine(System.Environment.NewLine);

            message.AppendFormat("Project Name: {0}", ticket.Project.Name);
            message.AppendLine(System.Environment.NewLine);
            return  message.ToString();
        }



        public static async Task SendEmailNotification(string recipientId, string subject, string message)
        {

            var from = WebConfigurationManager.AppSettings["emailFrom"];
            //temporarily have all notifications sent to my email.
            var to = "hwarrendev@gmail.com";
            //var to = db.Users.Find(recipientId).Email;
            var email = new MailMessage(from, to)
                            {
                                Subject = subject,
                                //"Notification from BugTracker Regarding Your Ticket",
                                Body = message,
                                IsBodyHtml = false
                            };
            var svc = new PersonalEmail();
            await svc.SendAsync(email);
        }
    }
}