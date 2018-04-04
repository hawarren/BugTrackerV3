using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BugTrackerV3.Models
{
    public class TicketComment
    {
        public int Id { get; set; }
        [Required]
        public string Comment { get; set; }

        [Display(Name = "Date Created")]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yy hh:mmtt}", ApplyFormatInEditMode = true)]
        public DateTimeOffset Created { get; set; }

        public int TicketId { get; set; }
        public string UserId { get; set; }

        public virtual Ticket Ticket { get; set; }
        public virtual ApplicationUser User { get; set; }

    }
}