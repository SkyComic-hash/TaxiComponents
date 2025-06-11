using System.Collections.Generic;

namespace TaxiComponents.Interfaces
{
    public interface IClientService
    {
        void RegisterClient(string clientId, string name);
        decimal GetTotalExpenses(string clientId);
        decimal GetDiscount(string clientId);
        void AddExpense(string clientId, decimal amount);
        string GetClientName(string clientId);
    }
}
