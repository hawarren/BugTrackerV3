using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BugTrackerV4.Models
{
    public class AssignDevViewModel
    {
        public SelectList Developers { get; set; }
        public Ticket Ticket { get; set; }
        public string SelectedUser { get; set; }
    }
}