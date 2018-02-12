using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace BugTrackerV3.Models
{
    public class TicketHistoryController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: TicketHistory
        public ActionResult Index()
        {
            var TicketHistorys = db.TicketHistorys.Include(t => t.Ticket).Include(t => t.User);
            return View(TicketHistorys.ToList());
        }

        // GET: TicketHistory/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TicketHistory ticketHistory = db.TicketHistorys.Find(id);
            if (ticketHistory == null)
            {
                return HttpNotFound();
            }
            return View(ticketHistory);
        }

        // GET: TicketHistory/Create
        public ActionResult Create()
        {
            ViewBag.TicketId = new SelectList(db.Tickets, "Id", "Title");
            ViewBag.UserId = new SelectList(db.Users, "Id", "FirstName");
            return View();
        }

        // POST: TicketHistory/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,TicketId,Property,EditId,OldValue,NewValue,Changed,UserId")] TicketHistory ticketHistory)
        {
            if (ModelState.IsValid)
            {
                db.TicketHistorys.Add(ticketHistory);
                db.SaveChanges();
                //return RedirectToAction("Index");
                //go back to ticket details
                return RedirectToAction("Details", "Tickets", new { id = ticketHistory.TicketId });
            }

            ViewBag.TicketId = new SelectList(db.Tickets, "Id", "Title", ticketHistory.TicketId);
            ViewBag.UserId = new SelectList(db.Users, "Id", "FirstName", ticketHistory.UserId);
            return View(ticketHistory);
        }

        // GET: TicketHistory/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TicketHistory ticketHistory = db.TicketHistorys.Find(id);
            if (ticketHistory == null)
            {
                return HttpNotFound();
            }
            ViewBag.TicketId = new SelectList(db.Tickets, "Id", "Title", ticketHistory.TicketId);
            ViewBag.UserId = new SelectList(db.Users, "Id", "FirstName", ticketHistory.UserId);
            return View(ticketHistory);
        }

        // POST: TicketHistory/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,TicketId,Property,EditId,OldValue,NewValue,Changed,UserId")] TicketHistory ticketHistory)
        {
            if (ModelState.IsValid)
            {
                db.Entry(ticketHistory).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.TicketId = new SelectList(db.Tickets, "Id", "Title", ticketHistory.TicketId);
            ViewBag.UserId = new SelectList(db.Users, "Id", "FirstName", ticketHistory.UserId);
            return View(ticketHistory);
        }

        // GET: TicketHistory/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TicketHistory ticketHistory = db.TicketHistorys.Find(id);
            if (ticketHistory == null)
            {
                return HttpNotFound();
            }
            return View(ticketHistory);
        }

        // POST: TicketHistory/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            TicketHistory ticketHistory = db.TicketHistorys.Find(id);
            db.TicketHistorys.Remove(ticketHistory);
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
