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

        public static string BuildNotificationMessage(string type, int ticketId, string recipientId)
        {
            var ticket = db.Tickets.Find(ticketId);

            var message = new StringBuilder();
            message.AppendFormat("Dear {0},", db.Users.Find(recipientId).FirstName);
            message.AppendLine(System.Environment.NewLine);
            //add remainder of BuildNotifcationMessage here

        }

    }
}