using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BugTrackerV4.Models
{
    public class UsersViewModel
    {
        public ApplicationUser User { get; set; }
        public List<string> Roles { get; set; }
    }
}