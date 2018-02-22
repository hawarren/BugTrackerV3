using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BugTrackerV3.Models
{
    public class TicketNotification
    {
    public int Id { get; set; }
    public string Body { get; set; }
    public DateTimeOffset DateNotified { get; set; }
    public int TicketId { get; set; }
    //public string UserId { get; set; }
    public string SenderId { get; set; }
    public string RecipientId { get; set; }

    public virtual Ticket Ticket { get; set; }
    public virtual ApplicationUser User { get; set; }
    public virtual  ApplicationUser Sender { get; set; }
    }
}