using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BugTrackerV3.Models;

namespace BugTrackerV3.Models
{
    public class ProjectPMViewModel
    {
        public Project Project { get; set; }
        // ProjectManager is just a user who happens to be assigned to manage the project
        public ApplicationUser ProjectManager { get; set; }
    }
}