using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Web.Configuration;

namespace BugTrackerV3.helpers
{
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
            message.AppendFormat(
                "You have been {0} a Ticket. Please review the following details",
                msgType == "Assigned" ? " assigned to " : " unassigned from ");
            message.AppendLine(System.Environment.NewLine);

            message.AppendFormat("Assignment Date: {0}", DateTime.Now);
            message.AppendLine(System.Environment.NewLine);

            message.AppendFormat("Ticket Id: {0}", ticket.Id);
            message.AppendLine(System.Environment.NewLine);

            message.AppendFormat("Ticket Title: {0}", ticket.Project.Name);
            message.AppendLine(System.Environment.NewLine);

            message.AppendFormat("Project Name: {0}", ticket.Project.Name);
            message.AppendLine(System.Environment.NewLine);

            return message.ToString();
        }

        public static string BuildNotificationMessage2(TicketHistory changelog, int ticketId, string recipientId)
        {
            var ticket = db.Tickets.Find(ticketId);
            //this unnecessary but didnt' want to change the conditional involving it
            var msgType = "Assigned";
            var message = new StringBuilder();

            message.AppendFormat("Dear {0},", db.Users.Find(recipientId).FirstName);
            message.AppendLine(System.Environment.NewLine);
            //alter message based on whether dev has been assigned or unassigned from a ticket
            message.AppendFormat(
                "You have been {0} a Ticket. Please review the following details",
                msgType == "Assigned" ? " assigned to " : " unassigned from ");
            message.AppendLine(System.Environment.NewLine);

            message.AppendFormat("Assignment Date: {0}", DateTime.Now);
            message.AppendLine(System.Environment.NewLine);

            message.AppendFormat("Ticket Id: {0}", ticket.Id);
            message.AppendLine(System.Environment.NewLine);

            message.AppendFormat("Ticket Title: {0}", ticket.Project.Name);
            message.AppendLine(System.Environment.NewLine);

            message.AppendFormat("Project Name: {0}", ticket.Project.Name);
            message.AppendLine(System.Environment.NewLine);

            message.AppendFormat("Project Changes: {0}", changelog.ToString());
            message.AppendLine(System.Environment.NewLine);

            return message.ToString();
        }

        public static async Task SendEmailNotification(string recipientId, string message)
        {

            var from = WebConfigurationManager.AppSettings["emailFrom"];
            //temporarily have all notifications sent to my email.
            var to = "hwarrendev@gmail.com";
            //var to = db.Users.Find(recipientId).Email;
            var email = new MailMessage(from, to)
                            {
                                Subject = "You have been assigned to a Ticket",
                                Body = message,
                                IsBodyHtml = true
                            };
            var svc = new PersonalEmail();
            await svc.SendAsync(email);
        }
    }
}