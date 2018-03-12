using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BugTrackerV3.ViewModels
{
    using System.Security.Policy;

    using BugTrackerV3.Controllers;
    using BugTrackerV3.Models;

    public class DashboardVM
    {
        public List<Project> Projects { get; set; }
        public List<Ticket> Tickets { get; set; }

        public DashboardVM()
        {

            Projects = new List<Project>();
            Tickets = new List<Ticket>();
        }
    }
}