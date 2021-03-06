﻿



using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BugTrackerV3.Models
{
    public class Project
    {
        public Project()
        {
            this.Tickets = new HashSet<Ticket>();
            this.Users = new HashSet<ApplicationUser>();
        }

        public int Id { get; set; }
        [Required]
        [Display(Name = "Name of Project")]
        public string Name { get; set; }

        //Project Description
        [Display(Name = "Project Description")]
        public string ProjectDescription { get; set; }

        //this is the project manager ID, not a primary key
        public string PMID { get; set; }

        public virtual ICollection<Ticket> Tickets { get; set; }
        public virtual ICollection<ApplicationUser> Users { get; set; }
    }

}
