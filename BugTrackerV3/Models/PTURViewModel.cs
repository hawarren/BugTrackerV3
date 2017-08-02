using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BugTrackerV4.Models
{
    /*this view model is for a page which list all projects, tickets, users, and roles
     * not sure which order, I just want admin to be able to have master view of everything
     * 
         */
    public class PTURViewModel
    {
        public Project Project { get; set; }
        public Ticket Ticket { get; set; }
        public ApplicationUser User { get; set; }
        public List<string> Roles { get; set; }
    }
}