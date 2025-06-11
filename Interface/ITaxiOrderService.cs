using System;

namespace TaxiComponents.Interfaces
{
    public interface ITaxiOrderService
    {
        string CreateOrder(string clientId, string vehicleId, string route, DateTime arrivalTime);
        decimal CalculateEstimatedCost(string route, DateTime arrivalTime);
        void CompleteOrder(string orderId, TimeSpan actualDuration);
        decimal GetFinalCost(string orderId);
        TaxiOrder GetOrder(string orderId);
    }
}
