using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BugTrackerV3.helpers;
using Microsoft.AspNet.Identity;
using System.Configuration;
using System.Net.Mail;

namespace BugTrackerV3.Controllers
{
    using System.Text;
    using System.Threading.Tasks;

    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            var CurrentUser = User.Identity.GetUserId();
            //pass the current user if there is one
            if (CurrentUser != null)
            {
                UserRolesHelper helper = new UserRolesHelper();
                if (helper.IsUserinRole(CurrentUser, "Admin"))
                {
                    return RedirectToAction("ManageUserRoles", "Admin");
                }
            }


            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        [Authorize]
        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Contact(EmailModel model)
        {

            var emailBody = new StringBuilder();
            emailBody.AppendLine(
                "This is a message from your Bugtracker web application. The name and the email of the contacting person is above.");
            emailBody.AppendLine("<br />");
            emailBody.AppendLine(model.Body);

            if (ModelState.IsValid)
            {
                try
                {
                    var body = "<p>Email From: <bold>{0}</bold> ({1})</p><p> Message:</p><p>{2}</p>";
                    var from = "BugTracker<hanif.warren@gmail.com>";
                    model.Body = emailBody.ToString();

                    var email =
                        new MailMessage(from, ConfigurationManager.AppSettings["emailto"])
                        {
                            Subject =
                                    "BugTrackerV3 Contact Email",
                            Body = string.Format(
                                    body,
                                    model.FromName,
                                    model.FromEmail,
                                    model.Body),
                            IsBodyHtml = true
                        };
                    var svc = new PersonalEmail();
                    await svc.SendAsync(email);
                    return View("Sent");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    await Task.FromResult(0);
                }
            }
                return View(model);


        }



    }
}
