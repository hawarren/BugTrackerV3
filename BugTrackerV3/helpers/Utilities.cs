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

        public static string BuildNotificationMessage(string type, int ticketId, string recipientId)
        {
            var ticket = db.Tickets.Find(ticketId);

            var message = new StringBuilder();
            message.AppendFormat("Dear {0},", db.Users.Find(recipientId).FirstName);
            message.AppendLine(System.Environment.NewLine);
            //add remainder of BuildNotifcationMessage here

            message.AppendFormat(
                "You have been {0} a Ticket. Please review the following details",
                type == "Assigned" ? " assigned to " : " unassigned from ");
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

        public static async Task SendEmailNotification(string recipientId, string message)
        {
            var from = WebConfigurationManager.AppSettings["emailFrom"];
            var to = db.Users.Find(recipientId).Email;
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