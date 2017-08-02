namespace BugTrackerV4.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using Models;

    internal sealed class Configuration : DbMigrationsConfiguration<BugTrackerV4.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed (BugTrackerV4.Models.ApplicationDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //
            

            //seeding the roles
            var roleManager = new RoleManager<IdentityRole>(
           new RoleStore<IdentityRole>(context));

            if (!context.Roles.Any(r => r.Name == "Admin"))
            {
                roleManager.Create(new IdentityRole { Name = "Admin" });
            }

            if (!context.Roles.Any(r => r.Name == "ProjectManager"))
            {
                roleManager.Create(new IdentityRole { Name = "ProjectManager" });
            }

            if (!context.Roles.Any(r => r.Name == "Developer"))
            {
                roleManager.Create(new IdentityRole { Name = "Developer" });
            }

            if (!context.Roles.Any(r => r.Name == "Submitter"))
            {
                roleManager.Create(new IdentityRole { Name = "Submitter" });
            }


            //Seeding the ticket types
            if (!context.TicketTypes.Any(r => r.Name == "Bug"))
            {
                context.TicketTypes.Add(new TicketType { Name = "Bug" });
            }

            if (!context.TicketTypes.Any(r => r.Name == "Project Task"))
            {
                context.TicketTypes.Add(new TicketType { Name = "Project Task" });
            }

            if (!context.TicketTypes.Any(r => r.Name == "Software Update"))
            {
                context.TicketTypes.Add(new TicketType { Name = "Software Update" });
            }


            //seeding the ticket priorities
            if (!context.TicketPrioritys.Any(r => r.Name == "Low"))

            {
                context.TicketPrioritys.Add(new TicketPriority { Name = "Low" });
            }

            if (!context.TicketPrioritys.Any(r => r.Name == "Medium"))
            {
                context.TicketPrioritys.Add(new TicketPriority { Name = "Medium" });
            }

            if (!context.TicketPrioritys.Any(r => r.Name == "High"))
            {
                context.TicketPrioritys.Add(new TicketPriority { Name = "High" });
            }

            if (!context.TicketPrioritys.Any(r => r.Name == "ExtremeDanger"))
            {
                context.TicketPrioritys.Add(new TicketPriority { Name = "ExtremeDanger" });
            }


            //seeding the ticket status
            if (!context.TicketStatus.Any(r => r.Name == "Open"))
            {
                context.TicketStatus.Add(new TicketStatus { Name = "Open" });
            }


            if (!context.TicketStatus.Any(r => r.Name == "New"))
            {
                context.TicketStatus.Add(new TicketStatus { Name = "New" });
            }

            if (!context.TicketStatus.Any(r => r.Name == "Closed"))
            {
                context.TicketStatus.Add(new TicketStatus { Name = "Closed" });
            }

            if (!context.TicketStatus.Any(r => r.Name == "WontFix"))
            {
                context.TicketStatus.Add(new TicketStatus { Name = "WontFix" });
            }
            if (!context.TicketStatus.Any(r => r.Name == "UnderReview"))
            {
                context.TicketStatus.Add(new TicketStatus { Name = "UnderReview" });
            }



            //seeding hanifwarren as the admin
            var userManager = new UserManager<ApplicationUser>(
            new UserStore<ApplicationUser>(context));

            if (!context.Users.Any(u => u.Email == "hanifwarren@gmail.com"))
            {
                userManager.Create(new ApplicationUser
                {
                    UserName = "hanifwarren",
                    Email = "hanifwarren@gmail.com",
                    FirstName = "Hanif",
                    LastName = "Warren",
                    DisplayName = "Hanif Warren"
                }, "12345678");
            }


            //adding all roles to hanifwarren
            var userIdhw = userManager.FindByEmail("hanifwarren@gmail.com").Id;
            userManager.AddToRole(userIdhw, "Admin");
            userManager.AddToRole(userIdhw, "ProjectManager");
            userManager.AddToRole(userIdhw, "Submitter");
            userManager.AddToRole(userIdhw, "Developer");

            //seeding dummypm
            if (!context.Users.Any(u => u.Email == "DummyProjectManager@gmail.com"))
            {
                userManager.Create(new ApplicationUser
                {
                    UserName = "DummyProjectManager@gmail.com",
                    Email = "DummyProjectManager@gmail.com",
            
                    FirstName = "DummyPM",
                    LastName = "ProjectManager",
                    DisplayName = "Dummy Project Manager"
                }, "12345678");
            }


            //adding all roles to dummyPM except Admin
            var userIdPM = userManager.FindByEmail("DummyProjectManager@gmail.com").Id;
           
            userManager.AddToRole(userIdPM, "ProjectManager");
            userManager.AddToRole(userIdPM, "Submitter");
            userManager.AddToRole(userIdPM, "Developer");







            //testusers 
            if (!context.Users.Any(u => u.Email == "testuser1@btracker.com"))
            {
                userManager.Create(new ApplicationUser
                {
                    UserName = "testuser1@btracker.com",
                    Email = "testuser1@btracker.com",
                    FirstName = "Test1",
                    LastName = "User1",
                    DisplayName = "Imatest1"
                }, "Abc&123!");
            }

            //adding all roles to dummyPM except Admin
            var userIdt1 = userManager.FindByEmail("testuser1@btracker.com").Id;                
            userManager.AddToRole(userIdt1, "Submitter");
            


            if (!context.Users.Any(u => u.Email == "testuser2@btracker.com"))
            {

                userManager.Create(new ApplicationUser
                {
                    UserName = "testuser2@btracker.com",
                    Email = "testuser2@btracker.com",
                    FirstName = "Test2",
                    LastName = "User2",
                    DisplayName = "Imatest2"
                }, "Abc&123!");
            }
            var userIdt2 = userManager.FindByEmail("testuser2@btracker.com").Id;
            userManager.AddToRole(userIdt2, "Submitter");

            if (!context.Users.Any(u => u.Email == "testuser3@btracker.com"))
            {

                userManager.Create(new ApplicationUser
                {
                    UserName = "testuser3@btracker.com",
                    Email = "testuser3@btracker.com",
                    FirstName = "Test3",
                    LastName = "User3",
                    DisplayName = "Imatest3"
                }, "Abc&123!");
            }
            var userIdt3 = userManager.FindByEmail("testuser3@btracker.com").Id;
            userManager.AddToRole(userIdt3, "Submitter");

            if (!context.Users.Any(u => u.Email == "testuser4@btracker.com"))
            {

                userManager.Create(new ApplicationUser
                {
                    UserName = "testuser4@btracker.com",
                    Email = "testuser4@btracker.com",
                    FirstName = "Test4",
                    LastName = "User4",
                    DisplayName = "Imatest4"
                }, "Abc&123!");
            }
            var userIdt4 = userManager.FindByEmail("testuser4@btracker.com").Id;
            userManager.AddToRole(userIdt4, "Submitter");

            if (!context.Users.Any(u => u.Email == "testuser5@btracker.com"))
            {

                userManager.Create(new ApplicationUser
                {
                    UserName = "testuser5@btracker.com",
                    Email = "testuser5@btracker.com",
                    FirstName = "Test5",
                    LastName = "User5",
                    DisplayName = "Imatest5"
                }, "Abc&123!");
            }
            var userIdt5 = userManager.FindByEmail("testuser5@btracker.com").Id;
            userManager.AddToRole(userIdt5, "Submitter");

            if (!context.Users.Any(u => u.Email == "testuser6@btracker.com"))
            {

                userManager.Create(new ApplicationUser
                {
                    UserName = "testuser6@btracker.com",
                    Email = "testuser6@btracker.com",
                    FirstName = "Test6",
                    LastName = "User6",
                    DisplayName = "Imatest6"
                }, "Abc&123!");
            }
            var userIdt6 = userManager.FindByEmail("testuser6@btracker.com").Id;
            userManager.AddToRole(userIdt6, "Submitter");

            //seeding the devs
            //added multiple with obvious naming scheme
            if (!context.Users.Any(u => u.Email == "testdev1@btracker.com"))
            {
                userManager.Create(new ApplicationUser
                {
                    UserName = "testdev1",
                    Email = "testdev1@btracker.com",
                    FirstName = "Firstdev1",
                    LastName = "Lastdev1",
                    DisplayName = "dev1"
                }, "Abc&123!");
            }
            var userdev1 = userManager.FindByEmail("testdev1@btracker.com").Id;
            userManager.AddToRole(userdev1, "Developer");
            userManager.AddToRole(userdev1, "Submitter");


            if (!context.Users.Any(u => u.Email == "testdev2@btracker.com"))
            {
                userManager.Create(new ApplicationUser
                {
                    UserName = "testdev2",
                    Email = "testdev2@btracker.com",
                    FirstName = "Firstdev2",
                    LastName = "LastDev2",
                    DisplayName = "Dev2"
                }, "Abc&123!");
            }
            var userdev2 = userManager.FindByEmail("testdev2@btracker.com").Id;
            userManager.AddToRole(userdev2, "Developer");
            userManager.AddToRole(userdev2, "Submitter");

            if (!context.Users.Any(u => u.Email == "testdev3@btracker.com"))
            {
                userManager.Create(new ApplicationUser
                {
                    UserName = "testdev3",
                    Email = "testdev3@btracker.com",
                    FirstName = "Firstdev3",
                    LastName = "Lastdev3",
                    DisplayName = "dev3"
                }, "Abc&123!");
            }
            var userdev3 = userManager.FindByEmail("testdev3@btracker.com").Id;
            userManager.AddToRole(userdev3, "Developer");
            userManager.AddToRole(userdev3, "Submitter");

            if (!context.Users.Any(u => u.Email == "testdev4@btracker.com"))
            {
                userManager.Create(new ApplicationUser
                {
                    UserName = "testdev4",
                    Email = "testdev4@btracker.com",
                    FirstName = "Firstdev4",
                    LastName = "Lastdev4",
                    DisplayName = "dev4"
                }, "Abc&123!");
            }
            var userdev4 = userManager.FindByEmail("testdev4@btracker.com").Id;
            userManager.AddToRole(userdev4, "Developer");
            userManager.AddToRole(userdev4, "Submitter");

            //.......................................

            //.............................................




            //seeding the devs
            //added multiple with obvious naming scheme
            if (!context.Users.Any(u => u.Email == "testPMan1@btracker.com"))
            {
                userManager.Create(new ApplicationUser
                {
                    UserName = "testPMan1",
                    Email = "testPMan1@btracker.com",
                    FirstName = "FirstPMan1",
                    LastName = "LastPMan1",
                    DisplayName = "PMan1"
                }, "Abc&123!");
            }
            var userPMan1 = userManager.FindByEmail("testPMan1@btracker.com").Id;
            userManager.AddToRole(userPMan1, "ProjectManager");
            userManager.AddToRole(userPMan1, "Submitter");


            if (!context.Users.Any(u => u.Email == "testPMan2@btracker.com"))
            {
                userManager.Create(new ApplicationUser
                {
                    UserName = "testPMan2",
                    Email = "testPMan2@btracker.com",
                    FirstName = "FirstPMan2",
                    LastName = "LastPMan2",
                    DisplayName = "PMan2"
                }, "Abc&123!");
            }
            var userPMan2 = userManager.FindByEmail("testPMan2@btracker.com").Id;
            userManager.AddToRole(userPMan2, "ProjectManager");
            userManager.AddToRole(userPMan2, "Submitter");

            if (!context.Users.Any(u => u.Email == "testPMan3@btracker.com"))
            {
                userManager.Create(new ApplicationUser
                {
                    UserName = "testPMan3",
                    Email = "testPMan3@btracker.com",
                    FirstName = "FirstPMan3",
                    LastName = "LastPMan3",
                    DisplayName = "PMan3"
                }, "Abc&123!");
            }
            var userPMan3 = userManager.FindByEmail("testPMan3@btracker.com").Id;
            userManager.AddToRole(userPMan3, "ProjectManager");
            userManager.AddToRole(userPMan3, "Submitter");


            if (!context.Users.Any(u => u.Email == "testPMan4@btracker.com"))
            {
                userManager.Create(new ApplicationUser
                {
                    UserName = "testPMan4",
                    Email = "testPMan4@btracker.com",
                    FirstName = "FirstPMan4",
                    LastName = "LastPMan4",
                    DisplayName = "PMan4"
                }, "Abc&123!");
            }
            var userPMan4 = userManager.FindByEmail("testPMan4@btracker.com").Id;
            userManager.AddToRole(userPMan4, "ProjectManager");
            userManager.AddToRole(userPMan4, "Submitter");






        }
    }
}
