using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BugTrackerV3.Models;
using Microsoft.AspNet.Identity;

namespace BugTrackerV3.Controllers
{
    public class TicketAttachmentsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: TicketAttachments
        public ActionResult Index()
        {
            var ticketAttachments = db.TicketAttachments.Include(t => t.Ticket).Include(t => t.User);
            return View(ticketAttachments.ToList());
        }

        // GET: TicketAttachments/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TicketAttachment ticketAttachment = db.TicketAttachments.Find(id);
            if (ticketAttachment == null)
            {
                return HttpNotFound();
            }
            return View(ticketAttachment);
        }

        // GET: TicketAttachments/Create
        public ActionResult Create( string TicketId)
        {
            //made the dropdown show only 
            ViewBag.TicketId = new SelectList(db.Tickets.Where(t => t.Id.ToString() == TicketId), "Id", "Title");
            ViewBag.UserId = new SelectList(db.Users.Where(t => t.Id == User.Identity.GetUserId()), "Id", "FirstName");
            return View();
        }

        // POST: TicketAttachments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,FilePath,Description,Created,UserId,TicketId")] TicketAttachment ticketAttachment, HttpPostedFileBase fileAdded)
        {
            ticketAttachment.Ticket = db.Tickets.Find(ticketAttachment.TicketId);
            if (ticketAttachment.Ticket.AssignedToUserId == User.Identity.GetUserId()
                || ticketAttachment.Ticket.OwnerUserId == User.Identity.GetUserId()
                 || ticketAttachment.Ticket.Project.PMID == User.Identity.GetUserId()
                 || User.IsInRole("Admin")
                 )
            {
                
                    if (ModelState.IsValid)
                {
                    if (fileAdded != null && fileAdded.ContentLength > 0)
                    { 
                        //code to get the url from uploaded file
                        var fileName = Path.GetFileName(fileAdded.FileName);
                    fileAdded.SaveAs(Path.Combine(Server.MapPath("~/Uploads/"), fileName));
                    ticketAttachment.FilePath = "/Uploads/" + fileName;
                    

                    db.TicketAttachments.Add(ticketAttachment);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                    }
                }

                ViewBag.TicketId = new SelectList(db.Tickets.Where(t => t.Id == ticketAttachment.TicketId), "Id", "Title");
                ViewBag.UserId = new SelectList(db.Users.Where(t => t.Id == User.Identity.GetUserId()), "Id", "FirstName");
                return View(ticketAttachment);
            
                }
            return View(ticketAttachment);
        }

        // GET: TicketAttachments/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TicketAttachment ticketAttachment = db.TicketAttachments.Find(id);
            if (ticketAttachment == null)
            {
                return HttpNotFound();
            }
            ViewBag.TicketId = new SelectList(db.Tickets, "Id", "Title", ticketAttachment.TicketId);
            ViewBag.UserId = new SelectList(db.Users, "Id", "FirstName", ticketAttachment.UserId);
            return View(ticketAttachment);
        }

        // POST: TicketAttachments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,FilePath,Description,Created,UserId,TicketId")] TicketAttachment ticketAttachment)
        {
            if (ModelState.IsValid)
            {
                db.Entry(ticketAttachment).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.TicketId = new SelectList(db.Tickets, "Id", "Title", ticketAttachment.TicketId);
            ViewBag.UserId = new SelectList(db.Users, "Id", "FirstName", ticketAttachment.UserId);
            return View(ticketAttachment);
        }

        // GET: TicketAttachments/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TicketAttachment ticketAttachment = db.TicketAttachments.Find(id);
            if (ticketAttachment == null)
            {
                return HttpNotFound();
            }
            return View(ticketAttachment);
        }

        // POST: TicketAttachments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            TicketAttachment ticketAttachment = db.TicketAttachments.Find(id);
            db.TicketAttachments.Remove(ticketAttachment);
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
