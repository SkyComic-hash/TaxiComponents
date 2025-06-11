using System;
using System.Collections.Generic;
using TaxiComponents.Interfaces;
using TaxiComponents.Models;

namespace TaxiComponents.Services
{
    public class TaxiOrderService : ITaxiOrderService
    {
        private readonly Dictionary<string, TaxiOrder> _orders = new();
        private readonly IClientService _clientService;
        private readonly IVehicleManagementService _vehicleService;
        private int _orderCounter = 0;

        public TaxiOrderService(IClientService clientService, IVehicleManagementService vehicleService)
        {
            _clientService = clientService;
            _vehicleService = vehicleService;
        }

        public string CreateOrder(string clientId, string vehicleId, string route, DateTime arrivalTime)
        {
            if (!_vehicleService.IsVehicleAvailable(vehicleId, arrivalTime))
                throw new Exception("Обране авто недоступне на цей час");

            _orderCounter++;
            var orderId = $"ORD{_orderCounter:0000}";

            decimal estimatedCost = CalculateEstimatedCost(route, arrivalTime);

            var order = new TaxiOrder
            {
                OrderId = orderId,
                ClientId = clientId,
                VehicleId = vehicleId,
                Route = route,
                RequestedArrivalTime = arrivalTime,
                EstimatedCost = estimatedCost,
                FinalCost = null
            };

            _orders[orderId] = order;

            // Позначаємо авто як недоступне до часу прибуття + 1 година
            _vehicleService.SetVehicleAvailability(vehicleId, false, arrivalTime.AddHours(1));

            return orderId;
        }

        public decimal CalculateEstimatedCost(string route, DateTime arrivalTime)
        {
            // Простий приклад: вартість = 50 грн + 10 грн за кожен символ маршруту + коефіцієнт часу доби
            decimal basePrice = 50m;
            decimal routePrice = route.Length * 10m;
            decimal timeCoefficient = (arrivalTime.Hour >= 20 || arrivalTime.Hour < 6) ? 1.5m : 1m; // нічна ставка

            return basePrice + routePrice * timeCoefficient;
        }

        public void CompleteOrder(string orderId, TimeSpan actualDuration)
        {
            if (!_orders.ContainsKey(orderId))
                throw new Exception("Замовлення не знайдено");

            var order = _orders[orderId];
            order.ActualDuration = actualDuration;

            // Остаточна вартість: базова + 20 грн за кожну хвилину поїздки
            decimal cost = order.EstimatedCost + (decimal)actualDuration.TotalMinutes * 20m;

            // Застосовуємо знижку клієнта
            decimal discount = _clientService.GetDiscount(order.ClientId);
            cost = cost * (1 - discount);

            order.FinalCost = cost;

            // Оновлюємо витрати клієнта
            _clientService.AddExpense(order.ClientId, cost);

            // Авто знову доступне
            _vehicleService.SetVehicleAvailability(order.VehicleId, true, null);
        }

        public decimal GetFinalCost(string orderId)
        {
            return _orders.ContainsKey(orderId) && _orders[orderId].FinalCost.HasValue ? _orders[orderId].FinalCost.Value : 0m;
        }

        public TaxiOrder GetOrder(string orderId)
        {
            return _orders.ContainsKey(orderId) ? _orders[orderId] : null;
        }
    }
}
