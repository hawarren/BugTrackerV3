using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BugTrackerV4.Models;

namespace BugTrackerV4.Models
{
    public class ProjectPMViewModel
    {
        public Project Project { get; set; }
        // ProjectManager is just a user who happens to be assigned to manage the project
        public ApplicationUser ProjectManager { get; set; }
    }
}