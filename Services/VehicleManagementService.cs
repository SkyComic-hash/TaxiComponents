using System;
using System.Collections.Generic;
using System.Linq;
using TaxiComponents.Interfaces;
using TaxiComponents.Models;

namespace TaxiComponents.Services
{
    public class VehicleManagementService : IVehicleManagementService
    {
        private readonly Dictionary<string, Vehicle> _vehicles = new();

        public VehicleManagementService()
        {
            // Для прикладу, додаємо кілька авто при створенні сервісу
            _vehicles["V1"] = new Vehicle { VehicleId = "V1", DriverName = "Іван Іванов", IsAvailable = true };
            _vehicles["V2"] = new Vehicle { VehicleId = "V2", DriverName = "Петро Петренко", IsAvailable = true };
            _vehicles["V3"] = new Vehicle { VehicleId = "V3", DriverName = "Марія Марченко", IsAvailable = true };
        }

        public void AssignDriver(string vehicleId, string driverName)
        {
            if (_vehicles.ContainsKey(vehicleId))
            {
                _vehicles[vehicleId].DriverName = driverName;
            }
        }

        public void SetVehicleAvailability(string vehicleId, bool isAvailable, DateTime? availableFrom = null)
        {
            if (_vehicles.ContainsKey(vehicleId))
            {
                _vehicles[vehicleId].IsAvailable = isAvailable;
                _vehicles[vehicleId].AvailableFrom = availableFrom;
            }
        }

        public bool IsVehicleAvailable(string vehicleId, DateTime requestedTime)
        {
            if (!_vehicles.ContainsKey(vehicleId)) return false;

            var vehicle = _vehicles[vehicleId];
            if (!vehicle.IsAvailable) return false;
            if (vehicle.AvailableFrom.HasValue && vehicle.AvailableFrom > requestedTime) return false;
            return true;
        }

        public Vehicle GetVehicle(string vehicleId)
        {
            return _vehicles.ContainsKey(vehicleId) ? _vehicles[vehicleId] : null;
        }

        public List<Vehicle> GetAllVehicles()
        {
            return _vehicles.Values.ToList();
        }
    }
}
