using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BugTrackerV4.Models
{
    public class AdminProjectViewModel
    {
        public Project Project { get; set; }
        //SelectList is a type that allows dropdown
        public SelectList PMUsers { get; set; }
        public string SelectedUser { get; set; }
    }
}