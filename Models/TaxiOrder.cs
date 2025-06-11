using System;

namespace TaxiComponents.Models
{
    public class TaxiOrder
    {
        public string OrderId { get; set; }
        public string ClientId { get; set; }
        public string VehicleId { get; set; }
        public string Route { get; set; }
        public DateTime RequestedArrivalTime { get; set; }
        public TimeSpan? ActualDuration { get; set; }
        public decimal EstimatedCost { get; set; }
        public decimal? FinalCost { get; set; }
    }
}
