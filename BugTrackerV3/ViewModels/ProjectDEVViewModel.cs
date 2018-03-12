using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BugTrackerV3.Models
{
    public class ProjectDEVViewModel
    {
        public Project Project { get; set; }

        //multiselectlist allows us to send in items that were already selected
        public MultiSelectList DevUsers { get; set; }
        public List<string> SelectedUsers { get; set; }
    }
}