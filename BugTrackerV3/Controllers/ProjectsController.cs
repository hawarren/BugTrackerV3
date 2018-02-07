using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BugTrackerV3.Models;
using BugTrackerV3.helpers;
using Microsoft.AspNet.Identity;

namespace BugTrackerV3.Controllers
{
    [Authorize]
    public class ProjectsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: All Projects
        [Authorize]
        public ActionResult Index()
        {
            //pass in userId, check their role, and then show them relevant projects.
            //admin and PM see all projects
            var projs = db.Projects.ToList();
            List<ProjectPMViewModel> model = new List<ProjectPMViewModel>();

            foreach (var p in projs)
            {
                ProjectPMViewModel vm = new ProjectPMViewModel();
                vm.Project = p;
                //this works because the PMID is just the User.Id, which is the primary key of Users
                vm.ProjectManager = p.PMID != null ? db.Users.Find(p.PMID) : null;
                model.Add(vm);
            }


            return View(model);
        }

        // GET: Projects for a User
        public ActionResult UserProjectsOnly()
        {
            var userID = User.Identity.GetUserId();
            //pass in userId, check their role, and then show them relevant projects.
            //admin and PM see all projects
            var projs = db.Projects.ToList();
            List<ProjectPMViewModel> model = new List<ProjectPMViewModel>();

            foreach (var p in projs)
            {
                ProjectsHelper Phelper = new ProjectsHelper();
                if (Phelper.IsUserOnProject(userID, p.Id))
                {

                ProjectPMViewModel vm = new ProjectPMViewModel();
                vm.Project = p;
                //this works because the PMID is just the User.Id, which is the primary key of Users
                //this code adds the user
                vm.ProjectManager = p.PMID != null ? db.Users.Find(p.PMID) : null;
                model.Add(vm);
                }
            }


            return View(model);
        }

        // GET: Projects/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Project project = db.Projects.Find(id);
            if (project == null)
            {
                return HttpNotFound();
            }
            return View(project);
        }

        [Authorize(Roles ="Admin")]
        //GET: Assign Project Manager
        public ActionResult AssignPM(int Id)
        {
            AdminProjectViewModel vm = new AdminProjectViewModel();
            UserRolesHelper helper = new UserRolesHelper();
            var pms = helper.UsersInRole("ProjectManager");

            vm.PMUsers = new SelectList(pms, "Id", "FirstName");
            vm.Project = db.Projects.Find(Id);

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AssignPM(AdminProjectViewModel adminVm)
        {
            if (ModelState.IsValid)
            {
                ProjectsHelper helper = new ProjectsHelper();
                var prj = db.Projects.Find(adminVm.Project.Id);
                prj.PMID = adminVm.SelectedUser;

                helper.AddUserToProject(prj.PMID, prj.Id);

                db.SaveChanges();
                return RedirectToAction("Index");
            }
            // return View(adminVm.Project.Id);
            return View(adminVm);
        }
        //Get
        [Authorize(Roles="Admin, ProjectManager")]
        public ActionResult AssignDEV(int id)
        {
            UserRolesHelper URHelper = new UserRolesHelper();
            string PM = User.Identity.GetUserId();
            if (PM != db.Projects.Find(id).PMID
            && URHelper.IsUserinRole(PM, "admin") != true)
            {
                return RedirectToAction("Index");
            }
            ProjectDEVViewModel vm = new ProjectDEVViewModel();
                UserRolesHelper helper = new UserRolesHelper();
                ProjectsHelper phelper = new ProjectsHelper();

                var dev = helper.UsersInRole("Developer");
                var projdev = phelper.ProjectUsersByRole(id, "Developer").Select(u => u.Id).ToArray();
                vm.DevUsers = new MultiSelectList(dev, "Id", "DisplayName", projdev);
                vm.Project = db.Projects.Find(id);

                return View(vm);


        }

        //POST
        [Authorize(Roles = "Admin, ProjectManager")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AssignDEV(ProjectDEVViewModel model)
        {
            ProjectsHelper helper = new ProjectsHelper();
            if (ModelState.IsValid)
            {
                var prj = db.Projects.Find(model.Project.Id);
                //this code removes all users currently on the project
               // foreach (var usr in prj.Users)
                //{
                 //   helper.RemoveUserFromProject(usr.Id, prj.Id);
                //}
                //this code add all the selected users to the project
                foreach (var dev in model.SelectedUsers)
                {
                    helper.AddUserToProject(dev, model.Project.Id);
                }


                //the helper already saves the changes to the db
                return RedirectToAction("Details", new { id = model.Project.Id });
            }
            return View(model);
        }




        // GET: Projects/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Projects/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,PMID")] Project project)
        {
            if (ModelState.IsValid)
            {
                db.Projects.Add(project);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(project);
        }

        // GET: Projects/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Project project = db.Projects.Find(id);
            if (project == null)
            {
                return HttpNotFound();
            }
            return View(project);
        }

        // POST: Projects/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,PMID,ProjectDescription")] Project project)
        {
            if (ModelState.IsValid)
            {
                db.Entry(project).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(project);

        }

        // GET: Projects/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Project project = db.Projects.Find(id);
            if (project == null)
            {
                return HttpNotFound();
            }
            return View(project);
        }

        // POST: Projects/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Project project = db.Projects.Find(id);
            db.Projects.Remove(project);
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
