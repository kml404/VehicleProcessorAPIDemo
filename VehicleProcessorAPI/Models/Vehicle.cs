using System;
using System.ComponentModel.DataAnnotations;

namespace VehicleProcessorAPI.Models
{
    public class Vehicle
    {
        [Key]
        public string VIN { get; set; }
        public int modelYear { get; set; }
        public double mileage { get; set; }
        public DateTime dateCreated { get; }
        public DateTime dateLastUpdated { get; set; }
 
    }
}
