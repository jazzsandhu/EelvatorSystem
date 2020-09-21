using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TestAssignment.Models
{
    public class Building
    {
        [Key]
        public int BuildingId { get; set; }
        [Required(ErrorMessage = "* This field is required.")]
        public string BuildingNumber { get; set; }
        [Required(ErrorMessage = "* This field is required.")]
        public string BuildingName { get; set; }
        [Required(ErrorMessage = "* This field is required.")]
        public int BuildingLimitElevatorNo { get; set; }

        //one building have many elevator 
        public ICollection<Elevator> elevators { get; set; }
    }
}