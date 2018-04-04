using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BugTrackerV3.Models
{
    using System.ComponentModel.DataAnnotations;

    public class TicketAttachment
    {
        public int Id { get; set; }
        public string FilePath { get; set; }
        public string Description { get; set; }
        [Display(Name = "Date Uploaded")]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yy hh:mmtt}", ApplyFormatInEditMode = true)]
        public DateTimeOffset Created { get; set; }
        //public string FileUrl { get; set; }


        public string UserId { get; set; }
        public int TicketId { get; set; }

        public virtual ApplicationUser User { get; set; }
        public virtual Ticket Ticket { get; set; }

    }
}