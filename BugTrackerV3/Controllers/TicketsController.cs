﻿using System;
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
    using System.Threading.Tasks;

    // TODO: refactor to rely on helpers for most controller functionality, rather than mucking up controllers with code
    [RequireHttps]
    [Authorize]
    public class TicketsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private TicketsHelper tixHelper = new TicketsHelper();
        private UserRolesHelper rolesHelper = new UserRolesHelper();

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
        public ActionResult Create([Bind(Include = "Id,Title,Description,Created,Updated,ProjectId,TicketStatusId,TicketPriorityId,TicketTypeId,OwnerUserId,AssignedToUserId")]int projectId)
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
            ViewBag.ProjectDescription = this.db.Projects.Find(ticket.ProjectId).ProjectDescription;
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
                //add user to project when they create a ticket
                ProjectsHelper phelper = new ProjectsHelper();
                if (!phelper.IsUserOnProject(User.Identity.GetUserId(), ticket.ProjectId))
                {
                    phelper.AddUserToProject(User.Identity.GetUserId(), ticket.ProjectId);
                }

                ticket.OwnerUserId = User.Identity.GetUserId();
                ticket.TicketPriorityId = db.TicketPrioritys.FirstOrDefault(n => n.Name == "Low").Id;
                //Give ticket initial status of "new"
                ticket.TicketStatusId = 2;
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
            //check if this user is either an admin, the project manager, or owner on this ticket
            if (ticket.Project.PMID != User.Identity.GetUserId()
                && ticket.OwnerUserId != User.Identity.GetUserId()
                && !User.IsInRole("Admin")
                )
            {

                return RedirectToAction("Details", "Tickets", new { id = ticket.Id });
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
       //public ActionResult Edit([Bind(Include = "Id,Title,Description,Created,Updated,ProjectId,TicketStatusId,TicketPriorityId,TicketTypeId,OwnerUserId,AssignedToUserId")] Ticket ticket)
       public async Task<ActionResult> Edit([Bind(Include = "Id,Title,Description,Created,Updated,ProjectId,TicketStatusId,TicketPriorityId,TicketTypeId,OwnerUserId,AssignedToUserId")] Ticket ticket)
        {
            //retrieve original ticket from database, but do not cache it in this dbcontext. This will be the "oldTicket"
            var oldTicket = this.db.Tickets.AsNoTracking().FirstOrDefault(t => t.Id == ticket.Id);


            if (ModelState.IsValid)
            {

                //Sets ticket status according to whether it's been assigned or not.
                // rewrite to use switch statement
                if (ticket.AssignedToUserId != null && ticket.TicketStatusId == 2)
                {
                    ticket.TicketStatusId = 1;
                }
                if (ticket.AssignedToUserId == null && ticket.TicketStatusId == 1)
                {
                    ticket.TicketStatusId = 2;
                    if (User.IsInRole("Admin"))
                    {
                        ticket.AssignedToUserId = User.Identity.GetUserId();
                    }
                    else
                    {
                        ticket.AssignedToUserId = ticket.Project.PMID;
                    }
                }

                if (ticket.AssignedToUserId == null && ticket.TicketStatusId == 3)
                {
                    //ticket.AssignedToUserId = User.Identity.GetUserId();
                    ticket.AssignedToUserId = ticket.Project.PMID;
                    //this.db.Tickets.AsNoTracking().FirstOrDefault(t => t.Id == ticket.Id);
                }
                if (ticket.AssignedToUserId != null && ticket.OwnerUserId != null)
                {
                //Use ProjectsHelper to
                //add developer to project once they are assigned to a ticket
                ProjectsHelper phelper = new ProjectsHelper();
                phelper.AddUserToProject(ticket.AssignedToUserId, ticket.ProjectId);
                phelper.AddUserToProject(ticket.OwnerUserId, ticket.ProjectId);

                }
                if (ticket.OwnerUserId == null)
                {
                    ticket.OwnerUserId = User.Identity.GetUserId();

                }

                ticket.Updated = DateTimeOffset.Now;





                    db.Entry(ticket).State = EntityState.Modified;
                db.SaveChanges();


                //ticketshelper to create the ticket history
                if (TicketsHelper.HasTicketChanged(oldTicket, ticket))
                {
                    TicketsHelper.AddTicketHistory(oldTicket, ticket);
                }

                //Send the relevant data to create notifications
                await this.tixHelper.GenerateNotifications(oldTicket, ticket);

                //return to ticket details so user can see updated changes.
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
