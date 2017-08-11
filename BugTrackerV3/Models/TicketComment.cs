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
        public DateTimeOffset Created { get; set; }

        public int TicketId { get; set; }
        public string UserId { get; set; }

        public virtual Ticket Ticket { get; set; }
        public virtual ApplicationUser User { get; set; }

    }
}