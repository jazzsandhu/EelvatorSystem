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
    
    public class ElevatorController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: Elevator
        public ActionResult List(string searchkey)
        {

            if (LoggedIn.isLoggedIn())
            {

                string query = "select * from elevators";
                // Debug.WriteLine("this is elevator list ");
                if (searchkey != "")
                {
                    query = query + " where ElevatorNumber like '%" + searchkey + "%'";
                }
                List<Elevator> elevators = db.Elevators.SqlQuery(query).ToList();
                return View(elevators);
            }

            else
            {
                return RedirectToAction("Login", "Account");
            }


        }
        public ActionResult Show(int id)
        {
            string user = User.Identity.GetUserId();

            if (LoggedIn.isLoggedIn())
            {

                //giving the values to the viewmodel for show elevator
                Elevator elevator = db.Elevators.Find(id);
                BuildingElevator viewmodel = new BuildingElevator();
                viewmodel.Elevator = elevator;
                return View(viewmodel);
            }
            else
            {
                return RedirectToAction("List", "Elevator");
            }


        }
        
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

        //create elevator
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Elevator model, string ElevatorNumber, string ElevatorLimitPeople, string ElevatorLimitWeight, int buildingId)
        {
            Debug.WriteLine("this is my elevator number" + ElevatorNumber +"and" + buildingId);
            if (LoggedIn.isLoggedIn())
            {

                //get the count of the eleveator related to the building Id
                string query1 = "select COUNT(*) from elevators where BuildingId = " + buildingId;
                var elevator = db.Elevators.Where(e => e.BuildingId.Equals(buildingId)).Count();
                Debug.WriteLine("this is my count " + elevator);
                //get the elevator limit 
                string query2 = "select * from Buildings where BuildingId =" + buildingId;
                Building building = db.Buildings.SqlQuery(query2).FirstOrDefault();
                Debug.WriteLine("this is my limit of elevators" + building);
                
                //check if the building elevator limit is always less and equal to total count of elevators
                if(building.BuildingLimitElevatorNo >= elevator)
                {
                    return RedirectToAction("Index", "Error");
                }
                else
                {
                    string query = "insert into elevators  (ElevatorNumber , ElevatorLimitPeople, ElevatorLimitWeight, BuildingId) values(@e_number, @e_limitpeople, @e_weightpeople,@b_id)";
                    SqlParameter[] sqlparams = new SqlParameter[4];
                    sqlparams[0] = new SqlParameter("@e_number", ElevatorNumber);
                    sqlparams[1] = new SqlParameter("@e_limitpeople", ElevatorLimitPeople);
                    sqlparams[2] = new SqlParameter("@e_weightpeople", ElevatorLimitWeight);
                    sqlparams[3] = new SqlParameter("@b_id", buildingId);


                    db.Database.ExecuteSqlCommand(query, sqlparams);
                    return RedirectToAction("List");
                }
                


            }
            return View(model);
        }
        //delete the elevator 
        public ActionResult Delete(int id)
        {
            if (LoggedIn.isLoggedIn())
            {
                string query = "delete from elevators where ElevatorId=@id";
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
