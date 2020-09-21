using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TestAssignment.Models;
using TestAssignment.Models.ViewModels;
using TestAssignment.Logs;
using System.Diagnostics;
//for datetime 
using System.Globalization;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;


namespace TestAssignment.Controllers
{
    public class BuildingController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        //view building
        public ActionResult List(string searchkey, int pagenum = 0)
        {
          
            if (LoggedIn.isLoggedIn())
            {
                //for checking if the logged in user admin 
                //bool isAdmin = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(User.Identity.GetUserId()).is_admin;
                //ViewData["isadmin"] = isAdmin;
                string query = "select * from buildings";
                List<SqlParameter> sqlparams = new List<SqlParameter>();
                if (searchkey != "")
                {
                    query = query + " where BuildingName like @searchkey";
                    sqlparams.Add(new SqlParameter("@searchkey", "%" + searchkey + "%"));

                }

                List<Building> buildings = db.Buildings.SqlQuery(query, sqlparams.ToArray()).ToList();
                //state of the page

                int perpage = 3;
                int bnumber = buildings.Count();
                int maxpage = (int)Math.Ceiling((decimal)bnumber / perpage) - 1;
                if (maxpage < 0) maxpage = 0;
                if (pagenum < 0) pagenum = 0;
                if (pagenum > maxpage) pagenum = maxpage;
                int start = (int)(perpage * pagenum);
                ViewData["pagenum"] = pagenum;
                ViewData["pagesummary"] = "";
                if (maxpage > 0)
                {
                    ViewData["pagesummary"] = (pagenum + 1) + " of " + (maxpage + 1);
                    List<SqlParameter> newparams = new List<SqlParameter>();

                    if (searchkey != "")
                    {
                        newparams.Add(new SqlParameter("@searchkey", "%" + searchkey + "%"));
                        ViewData["searchkey"] = searchkey;
                    }
                    newparams.Add(new SqlParameter("@start", start));
                    newparams.Add(new SqlParameter("@perpage", perpage));
                    string pagedquery = query + " order by BuildingId offset @start rows fetch first @perpage rows only ";
                    buildings = db.Buildings.SqlQuery(pagedquery, newparams.ToArray()).ToList();
                }

                return View(buildings);
            }
            else
            {
                //if the user is not logged in then it take them to the register page
                return RedirectToAction("Login", "Account");

            }
        }
        //show the building
        public ActionResult Show(int id)
        {
            string user = User.Identity.GetUserId();

            if (LoggedIn.isLoggedIn())
            {
                string query = "select * from Elevators where BuildingId = @id";
                var Parameter = new SqlParameter("@id", id);
                List<Elevator> elevators = db.Elevators.SqlQuery(query, Parameter).ToList();
                Building building = db.Buildings.Find(id);
                BuildingElevator viewmodel = new BuildingElevator();
                viewmodel.Elevators = elevators;
                viewmodel.Building = building;
                return View(viewmodel);
            }
            else
            {
                return RedirectToAction("List", "Building");
            }


        }
        //create building
        public ActionResult Create()
        {
            if (LoggedIn.isLoggedIn())
            {
                //giving the values to the viewmodel for show elevator
                BuildingElevator viewmodel = new BuildingElevator();
                viewmodel.Buildings = db.Buildings.ToList();
                return View(viewmodel);
            }
            else
            {
                return RedirectToAction("Index", "Error");
            }
        }
        // GET: Building/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Building model,string BuildingNumber, string BuildingName, string BuildingLimitElevatorNo)
        {
                Debug.WriteLine("this is my building name" + BuildingName);
                if (LoggedIn.isLoggedIn())
                {

                    string query = "insert into buildings  (BuildingNumber , BuildingName, BuildingLimitElevatorNo) values(@b_number, @b_name, @BuildingLimitElevatorNo)";
                    SqlParameter[] sqlparams = new SqlParameter[3];
                    sqlparams[0] = new SqlParameter("@b_number", BuildingNumber);
                    sqlparams[1] = new SqlParameter("@b_name", BuildingName);
                    sqlparams[2] = new SqlParameter("@BuildingLimitElevatorNo", BuildingLimitElevatorNo);
                    

                    db.Database.ExecuteSqlCommand(query, sqlparams);
                    return RedirectToAction("List");
                }
                return View(model);
        }
        //delete the building
        public ActionResult Delete(string id)
        {
            if (LoggedIn.isLoggedIn())
            {
                string query = "delete b from Buildings b join Elevators e ON  b.BuildingId =e.BuildingId where b.BuildingId = @id";
                SqlParameter param = new SqlParameter("@id", id);
                db.Database.ExecuteSqlCommand(query, param);
                return RedirectToAction("List");
            }
            else
            {
                return RedirectToAction("Index", "ErrorController");

            }

        }
    }

}