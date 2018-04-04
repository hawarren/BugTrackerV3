using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BugTrackerV3.Models
{
    public class TicketHistory
    {
        public int Id { get; set; }
        public int TicketId { get; set; }
        [Required]
        public string Property { get; set; }
        public string EditId { get; set; }
        public string OldValue { get; set; }
        public string NewValue { get; set; }
        [Display(Name = "Date Changed")]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yy hh:mmtt}", ApplyFormatInEditMode = true)]
        public DateTimeOffset Changed { get; set; }
        public string UserId { get; set; }

        public virtual Ticket Ticket { get; set; }
        public virtual ApplicationUser User { get; set; }

    }
}