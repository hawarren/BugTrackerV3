using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BugTrackerV3.Models
{
    public class UsersViewModel
    {
        public ApplicationUser User { get; set; }
        public List<string> Roles { get; set; }
        public List<string> ProjectsOn { get; set; }
    }
}