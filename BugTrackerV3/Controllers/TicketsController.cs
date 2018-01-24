using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BugTrackerV3.Models;
using Microsoft.AspNet.Identity;
using BugTrackerV3.helpers;


namespace BugTrackerV3.Controllers
{
    [Authorize]
    public class TicketsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Tickets

        public ActionResult Index()
        {
            //find the id of the current user
            var userId = User.Identity.GetUserId();

            TicketIndexViewModel model = new TicketIndexViewModel();
            //use roles helper to check for the roles
            UserRolesHelper helper = new UserRolesHelper();


            if (User.IsInRole("Admin"))
            {

                var tickets = db.Tickets.Include(t => t.AssignedToUser)
                        .Include(t => t.OwnerUser)
                        .Include(t => t.Project)
                        .Include(t => t.TicketPriority)
                        .Include(t => t.TicketStatus)
                        .Include(t => t.TicketType);
                model.AdminTickets = tickets.ToList();
            }
            if (User.IsInRole("Developer"))
            {

                var tickets = db.Tickets.Include(t => t.AssignedToUser)
                        .Include(t => t.OwnerUser)
                        .Include(t => t.Project)
                        .Include(t => t.TicketPriority)
                        .Include(t => t.TicketStatus)
                        .Include(t => t.TicketType);
                model.DevTickets = tickets.ToList();
            }
            if (User.IsInRole("ProjectManager"))
            {

                var tickets = db.Tickets.Include(t => t.AssignedToUser)
                        .Include(t => t.OwnerUser)
                        .Include(t => t.Project)
                        .Include(t => t.TicketPriority)
                        .Include(t => t.TicketStatus)
                        .Include(t => t.TicketType);
                model.PMTickets = tickets.ToList();
            }
            if (User.IsInRole("Submitter"))
            {

                var tickets = db.Tickets.Include(t => t.AssignedToUser)
                        .Include(t => t.OwnerUser)
                        .Include(t => t.Project)
                        .Include(t => t.TicketPriority)
                        .Include(t => t.TicketStatus)
                        .Include(t => t.TicketType);
                model.SubTickets = tickets.ToList();
            }


            return View(model);

        }

        // GET: List all Tickets for all projects for that dev.
        [Authorize(Roles = "Developer")]
        public ActionResult DevProjIndex()
        {
            //find the id of the current user
            string devUserId = User.Identity.GetUserId();

            TicketIndexViewModel model = new TicketIndexViewModel();
            //use roles helper to check for the roles
            ProjectsHelper phelper = new ProjectsHelper();
            //user ProjectsHelper to get a list of projects the dev is on.
           List<Project> devProjects = phelper.ListUserProjects(devUserId).ToList();
            //use linq with selectmany to get all tickets on each project.
            if (devProjects.Count() == 0)
            {
                return RedirectToAction("Index");
            }
            var devProjTickets = devProjects.SelectMany(t => t.Tickets).AsQueryable();
            {

                var tickets = devProjTickets.Include(t => t.AssignedToUser)
                        .Include(t => t.OwnerUser)
                        .Include(t => t.Project)
                        .Include(t => t.TicketPriority)
                        .Include(t => t.TicketStatus)
                        .Include(t => t.TicketType);
                //tickets = tickets.Where(t => t.Project.Id == phelper.ListUserProjects(devUserId).id;

                model.DevTickets = tickets.ToList();

            }


            return View(model);

        }





        // GET: Tickets for that user only
        [Authorize]
        public ActionResult UserTicketsIndex()
        {
            //find the id of the current user
            var userId = User.Identity.GetUserId();

            TicketIndexViewModel model = new TicketIndexViewModel();
            //use roles helper to check for the roles
            UserRolesHelper helper = new UserRolesHelper();


            if (User.IsInRole("Admin"))
            {

                var tickets = db.Tickets.Include(t => t.AssignedToUser)
                        .Include(t => t.OwnerUser)
                        .Include(t => t.Project)
                        .Include(t => t.TicketPriority)
                        .Include(t => t.TicketStatus)
                        .Include(t => t.TicketType);
                model.AdminTickets = tickets.ToList();

            }
            if (User.IsInRole("Developer"))
            {

                var tickets = db.Tickets.Include(t => t.AssignedToUser)
                        .Include(t => t.OwnerUser)
                        .Include(t => t.Project)
                        .Include(t => t.TicketPriority)
                        .Include(t => t.TicketStatus)
                        .Include(t => t.TicketType)
                        //.Include(t => t.ProjectId)
                        ;
                tickets = tickets.Where(s => s.AssignedToUserId == userId);
                model.DevTickets = tickets.ToList();






            }
            if (User.IsInRole("ProjectManager"))
            {

                var tickets = db.Tickets.Include(t => t.AssignedToUser)
                        .Include(t => t.OwnerUser)
                        .Include(t => t.Project)
                        .Include(t => t.TicketPriority)
                        .Include(t => t.TicketStatus)
                        .Include(t => t.TicketType);
                tickets = tickets.Where(s => s.Project.PMID == userId);
                model.PMTickets = tickets.ToList();
            }
            if (User.IsInRole("Submitter"))
            {

                var tickets = db.Tickets.Include(t => t.AssignedToUser)
                        .Include(t => t.OwnerUser)
                        .Include(t => t.Project)
                        .Include(t => t.TicketPriority)
                        .Include(t => t.TicketStatus)
                        .Include(t => t.TicketType);
                tickets = tickets.Where(s => s.OwnerUserId == userId);
                model.SubTickets = tickets.ToList();
            }


            return View(model);

        }




        // GET: Tickets/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ticket ticket = db.Tickets.Find(id);
            if (ticket == null)
            {
                return HttpNotFound();
            }
            return View(ticket);
        }

        // GET: Tickets/Create
        [Authorize(Roles = "Submitter")]
        public ActionResult Create(int projectId)
        {
            Ticket ticket = new Ticket();
            ticket.ProjectId = projectId;


            //SelectList is the defined type, the list of items have to be a List of SelectList
            //ViewBag.AssignedToUserId = new SelectList(db.Users, "Id", "FirstName");
            //ViewBag.OwnerUserId = new SelectList(db.Users, "Id", "FirstName");
            //ViewBag.ProjectId = new SelectList(db.Projects, "Id", "Name");
            //ViewBag.TicketPriorityId = new SelectList(db.TicketPrioritys, "Id", "Name");
            //ViewBag.TicketStatusId = new SelectList(db.TicketStatus, "Id", "Name");
            ViewBag.TicketTypeId = new SelectList(db.TicketTypes, "Id", "Name");

            ViewBag.ProjectName = db.Projects.Find(ticket.ProjectId).Name;

            return View(ticket);
        }

        // POST: Tickets/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Submitter")]
        public ActionResult Create([Bind(Include = "Id,Title,Description,Created,Updated,ProjectId,TicketStatusId,TicketPriorityId,TicketTypeId,OwnerUserId,AssignedToUserId")] Ticket ticket)
        {

            if (ModelState.IsValid)
            {
                ticket.OwnerUserId = User.Identity.GetUserId();
                ticket.TicketPriorityId = db.TicketPrioritys.FirstOrDefault(n => n.Name == "Low").Id;
                ticket.TicketStatusId = 1;
                ticket.Created = DateTimeOffset.Now;
                db.Tickets.Add(ticket);
                db.SaveChanges();
                return RedirectToAction("Details", "Projects", new { id = ticket.ProjectId });
            }
            //Identity framework chooses the name of the database, which is "Users", but the actual class name is "ApplicationUser"
            ViewBag.AssignedToUserId = new SelectList(db.Users, "Id", "FirstName", ticket.AssignedToUserId);
            ViewBag.OwnerUserId = new SelectList(db.Users, "Id", "FirstName", ticket.OwnerUserId);
            ViewBag.ProjectId = new SelectList(db.Projects, "Id", "Name", ticket.ProjectId);
            ViewBag.TicketPriorityId = new SelectList(db.TicketPrioritys, "Id", "Name", ticket.TicketPriorityId);
            ViewBag.TicketStatusId = new SelectList(db.TicketStatus, "Id", "Name", ticket.TicketStatusId);
            ViewBag.TicketTypeId = new SelectList(db.TicketTypes, "Id", "Name", ticket.TicketTypeId);
            return View(ticket);

        }

        // GET: Tickets/Edit/5
        public ActionResult Edit(int? id)
        {


            /////////////////wip code, clean up once I get it working

            //creates helper object to check for developer role
            UserRolesHelper helper = new UserRolesHelper();
            //UsersInRole helper gets a list of users in a role
            //  signature is      public ICollection<ApplicationUser> UsersInRole(string roleName)
            var dev = helper.UsersInRole("Developer");


            ///////////////// end of wip code
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ticket ticket = db.Tickets.Find(id);
            if (ticket == null)
            {
                return HttpNotFound();
            }
            //ViewBag.AssignedToUserId = new SelectList(db.Users, "Id", "FirstName", ticket.AssignedToUserId);
            ViewBag.AssignedToUserId = new SelectList(dev, "Id", "FirstName", ticket.AssignedToUserId);
            ViewBag.OwnerUserId = new SelectList(db.Users, "Id", "FirstName", ticket.OwnerUserId);
            ViewBag.ProjectId = new SelectList(db.Projects, "Id", "Name", ticket.ProjectId);
            ViewBag.TicketPriorityId = new SelectList(db.TicketPrioritys, "Id", "Name", ticket.TicketPriorityId);
            ViewBag.TicketStatusId = new SelectList(db.TicketStatus, "Id", "Name", ticket.TicketStatusId);
            ViewBag.TicketTypeId = new SelectList(db.TicketTypes, "Id", "Name", ticket.TicketTypeId);
            return View(ticket);
        }

        // POST: Tickets/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Title,Description,Created,Updated,ProjectId,TicketStatusId,TicketPriorityId,TicketTypeId,OwnerUserId,AssignedToUserId")] Ticket ticket)
        {
            if (ModelState.IsValid)
            {
                //Use ProjectsHelper to
                //add developer to project once they are assigned to a ticket
                ProjectsHelper phelper = new ProjectsHelper();
                phelper.AddUserToProject(ticket.AssignedToUserId, ticket.ProjectId);
                phelper.AddUserToProject(ticket.OwnerUserId, ticket.ProjectId);
                // ticket.AssignedToUserId
                ticket.Updated = DateTimeOffset.Now;
                db.Entry(ticket).State = EntityState.Modified;
                db.SaveChanges();
                // return RedirectToAction("Index");
                return RedirectToAction("Details", "Tickets", new { id = ticket.Id });
            }
            ViewBag.AssignedToUserId = new SelectList(db.Users, "Id", "FirstName", ticket.AssignedToUserId);
            ViewBag.OwnerUserId = new SelectList(db.Users, "Id", "FirstName", ticket.OwnerUserId);
            ViewBag.ProjectId = new SelectList(db.Projects, "Id", "Name", ticket.ProjectId);
            ViewBag.TicketPriorityId = new SelectList(db.TicketPrioritys, "Id", "Name", ticket.TicketPriorityId);
            ViewBag.TicketStatusId = new SelectList(db.TicketStatus, "Id", "Name", ticket.TicketStatusId);
            ViewBag.TicketTypeId = new SelectList(db.TicketTypes, "Id", "Name", ticket.TicketTypeId);
            return View(ticket);
        }

        // GET: Tickets/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ticket ticket = db.Tickets.Find(id);
            if (ticket == null)
            {
                return HttpNotFound();
            }
            return View(ticket);
        }

        // POST: Tickets/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Ticket ticket = db.Tickets.Find(id);
            db.Tickets.Remove(ticket);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
