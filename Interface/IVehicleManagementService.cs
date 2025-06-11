using System.Collections.Generic;

namespace TaxiComponents.Interfaces
{
    public interface IVehicleManagementService
    {
        void AssignDriver(string vehicleId, string driverName);
        void SetVehicleAvailability(string vehicleId, bool isAvailable, DateTime? availableFrom = null);
        bool IsVehicleAvailable(string vehicleId, DateTime requestedTime);
        Vehicle GetVehicle(string vehicleId);
        List<Vehicle> GetAllVehicles();
    }
}
