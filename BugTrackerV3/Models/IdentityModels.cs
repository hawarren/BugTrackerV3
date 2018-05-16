using System.Collections.Generic;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace BugTrackerV3.Models
{
    using System.Security.Policy;

    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public ApplicationUser()
        {
            this.Projects = new HashSet<Project>();
            this.TicketNotifications = new HashSet<TicketNotification>();
            this.TicketHistories = new HashSet<TicketHistory>();
            this.TicketComments = new HashSet<TicketComment>();
            this.TicketAttachments = new HashSet<TicketAttachment>();
            this.Tickets = new HashSet<Ticket>();
        }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string DisplayName { get; set; }
        public string Gender { get; set; }
        public string MediaUrl { get; set; }

        public virtual ICollection<Project> Projects { get; set; }
        public virtual ICollection<TicketNotification> TicketNotifications { get; set; }
        public virtual ICollection<TicketHistory> TicketHistories { get; set; }
        public virtual ICollection<TicketComment> TicketComments { get; set; }
        public virtual ICollection<TicketAttachment> TicketAttachments { get; set; }
        public virtual ICollection<Ticket> Tickets { get; set; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()             : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();

        }



        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<TicketAttachment> TicketAttachments      { get; set; }
        public DbSet<TicketComment> TicketComments            { get; set; }
        public DbSet<TicketHistory> TicketHistorys           { get; set; }
        public DbSet<TicketNotification> TicketNotifications  { get; set; }
        public DbSet<TicketPriority> TicketPrioritys       { get; set; }
        public DbSet<TicketStatus> TicketStatus          { get; set; }
        public DbSet<TicketType> TicketTypes { get; set; }
    }
}