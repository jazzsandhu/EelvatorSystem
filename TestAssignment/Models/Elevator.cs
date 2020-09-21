using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TestAssignment.Models
{
    public class Elevator
    {
        [Key]
        public int ElevatorId { get; set; }

        [Required(ErrorMessage = "* This field is required.")]
        public int ElevatorNumber { get; set; }

        [Required(ErrorMessage = "* This field is required.")]
        public int ElevatorLimitPeople { get; set; }

        [Required(ErrorMessage = "* This field is required.")]
        public int ElevatorLimitWeight { get; set; }

        //one building has many elevators
        public int BuildingId { get; set; }
        [ForeignKey("BuildingId")]
        public virtual Building Building { get; set; }
    }
}