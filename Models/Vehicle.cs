using System;

namespace TaxiComponents.Models
{
    public class Vehicle
    {
        public string VehicleId { get; set; }
        public string DriverName { get; set; }
        public bool IsAvailable { get; set; } = true;
        public DateTime? AvailableFrom { get; set; } = null;
    }
}

