using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TestAssignment.Models.ViewModels
{
    public class BuildingElevator
    {
 
        public virtual Building Building { get; set; }
        public virtual Elevator Elevator { get; set; }
        //list of elevators
        public virtual List<Elevator> Elevators { get; set; }
        //if we need list of buildings
        public virtual List<Building> Buildings { get; set; }
    }
}