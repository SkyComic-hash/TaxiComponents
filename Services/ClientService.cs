using System.Collections.Generic;
using TaxiComponents.Interfaces;
using TaxiComponents.Models;

namespace TaxiComponents.Services
{
    public class ClientService : IClientService
    {
        private readonly Dictionary<string, Client> _clients = new();

        public void RegisterClient(string clientId, string name)
        {
            if (!_clients.ContainsKey(clientId))
            {
                _clients[clientId] = new Client { ClientId = clientId, Name = name, TotalExpenses = 0m };
            }
        }

        public decimal GetTotalExpenses(string clientId)
        {
            return _clients.ContainsKey(clientId) ? _clients[clientId].TotalExpenses : 0m;
        }

        public decimal GetDiscount(string clientId)
        {
            var expenses = GetTotalExpenses(clientId);
            if (expenses > 1000) return 0.15m; // 15% знижка
            if (expenses > 500) return 0.10m;  // 10% знижка
            if (expenses > 100) return 0.05m;  // 5% знижка
            return 0m;
        }

        public void AddExpense(string clientId, decimal amount)
        {
            if (_clients.ContainsKey(clientId))
            {
                _clients[clientId].TotalExpenses += amount;
            }
        }

        public string GetClientName(string clientId)
        {
            return _clients.ContainsKey(clientId) ? _clients[clientId].Name : string.Empty;
        }
    }
}
