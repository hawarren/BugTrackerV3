using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Homer_MVC.Controllers
{
    public class TablesController : Controller
    {
        
        public ActionResult TablesDesign()
        {
            return View();
        }

        public ActionResult DataTables()
        {
            return View();
        }

        public ActionResult FooTable()
        {
            return View();
        }

        public JsonResult tableJsonData()
        {

            // Creat basic JSON object with data for table
            var tableData = new[] 
            {
                new [] { "Tiger Nixon", "System Architect", "Edinburgh", "61", "2011/04/25", "$320,800" },
                new [] { "Garrett Winters", "Accountant", "Tokyo", "63", "2011/07/25", "$170,750" },
                new [] { "Ashton Cox", "Junior Technical Author", "San Francisco", "66", "2009/01/12", "$86,000" },
                new [] { "Cedric Kelly", "Senior Javascript Developer", "Edinburgh", "22", "2012/03/29", "$433,060" },
                new [] { "Airi Satou", "Accountant", "Tokyo", "33", "2008/11/28", "$162,700" },
                new [] { "Brielle Williamson", "Integration Specialist", "New York", "61", "2012/12/02", "$372,000" },
                new [] { "Herrod Chandler", "Sales Assistant", "San Francisco", "59", "2012/08/06", "$137,500" },
                new [] { "Rhona Davidson", "Integration Specialist", "Tokyo", "55", "2010/10/14", "$327,900" },
                new [] { "Colleen Hurst", "Javascript Developer", "San Francisco", "39", "2009/09/15", "$205,500" },
                new [] { "Sonya Frost", "Software Engineer", "Edinburgh", "23", "2008/12/13", "$103,600" },
                new [] { "Jena Gaines", "Office Manager", "London", "30", "2008/12/19", "$90,560" },
                new [] { "Quinn Flynn", "Support Lead", "Edinburgh", "22", "2013/03/03", "$342,000" }
            };

            var dataTable = 
                new {data = tableData} 
            ;

            // Return JSON object
            return Json(dataTable, JsonRequestBehavior.AllowGet);

        }

    }
}
