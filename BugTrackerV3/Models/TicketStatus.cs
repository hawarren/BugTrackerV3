using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BugTrackerV4.Models
{
    public class TicketStatus
    {
        public int Id { get; set; }
        [Required]
        [Display(Name = "Ticket Status")]
        public string Name { get; set; }
    }
}