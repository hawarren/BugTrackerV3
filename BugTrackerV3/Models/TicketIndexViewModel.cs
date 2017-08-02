using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BugTrackerV4.Models
{
    public class TicketIndexViewModel
    {
        //this viewmodel is for displaying all tickets based on a person and their role
        public ApplicationUser User { get; set; }
        public List<string> Roles { get; set; }
        public List<Ticket> AdminTickets { get; set; }
        public List<Ticket> PMTickets { get; set; }
        public List<Ticket> DevTickets { get; set; }
        public List<Ticket> SubTickets { get; set; }
    }
}