using System;
using System.Windows.Forms;
using TaxiComponents.Interfaces;
using TaxiComponents.Services;

namespace TaxiClientApp
{
    public partial class MainForm : Form
    {
        private readonly IClientService _clientService;
        private readonly IVehicleManagementService _vehicleService;
        private readonly ITaxiOrderService _orderService;

        public MainForm()
        {
            InitializeComponent();

            _clientService = new ClientService();
            _vehicleService = new VehicleManagementService();
            _orderService = new TaxiOrderService(_clientService, _vehicleService);

            InitializeUI();
        }

        private void InitializeUI()
        {
            // Заповнюємо список автомобілів
            comboBoxVehicle.Items.Clear();
            foreach (var vehicle in _vehicleService.GetAllVehicles())
            {
                comboBoxVehicle.Items.Add($"{vehicle.VehicleId} - Водій: {vehicle.DriverName} {(vehicle.IsAvailable ? "" : "(недоступний)")}");
            }

            comboBoxVehicle.SelectedIndex = 0;

            dateTimePickerArrival.Value = DateTime.Now.AddMinutes(15);
        }

        private void btnRegisterClient_Click(object sender, EventArgs e)
        {
            var clientId = txtClientId.Text.Trim();
            var clientName = txtClientName.Text.Trim();
            if (string.IsNullOrEmpty(clientId) || string.IsNullOrEmpty(clientName))
            {
                MessageBox.Show("Введіть ID та ім'я клієнта");
                return;
            }

            _clientService.RegisterClient(clientId, clientName);
            MessageBox.Show($"Клієнт {clientName} зареєстрований");
        }

        private void btnCreateOrder_Click(object sender, EventArgs e)
        {
            try
            {
                var clientId = txtClientId.Text.Trim();
                if (string.IsNullOrEmpty(clientId))
                {
                    MessageBox.Show("Введіть ID клієнта");
                    return;
                }

                string vehicleId = comboBoxVehicle.SelectedItem.ToString().Split(' ')[0];
                string route = txtRoute.Text.Trim();
                DateTime arrivalTime = dateTimePickerArrival.Value;

                string orderId = _orderService.CreateOrder(clientId, vehicleId, route, arrivalTime);

                decimal estimatedCost = _orderService.CalculateEstimatedCost(route, arrivalTime);
                MessageBox.Show($"Замовлення створено (ID: {orderId}). Прогнозована вартість: {estimatedCost} грн");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Помилка: " + ex.Message);
            }
        }

        private void btnCompleteOrder_Click(object sender, EventArgs e)
        {
            try
            {
                string orderId = txtOrderId.Text.Trim();
                if (string.IsNullOrEmpty(orderId))
                {
                    MessageBox.Show("Введіть ID замовлення");
                    return;
                }

                // Для спрощення - вводимо фактичну тривалість у хвилинах
                if (!int.TryParse(txtDuration.Text.Trim(), out int minutes))
                {
                    MessageBox.Show("Введіть коректну тривалість у хвилинах");
                    return;
                }

                _orderService.CompleteOrder(orderId, TimeSpan.FromMinutes(minutes));
                decimal finalCost = _orderService.GetFinalCost(orderId);
                MessageBox.Show($"Поїздка завершена. Остаточна вартість: {finalCost} грн");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Помилка: " + ex.Message);
            }
        }

        private void btnShowClientExpenses_Click(object sender, EventArgs e)
        {
            var clientId = txtClientId.Text.Trim();
            if (string.IsNullOrEmpty(clientId))
            {
                MessageBox.Show("Введіть ID клієнта");
                return;
            }

            decimal totalExpenses = _clientService.GetTotalExpenses(clientId);
            decimal discount = _clientService.GetDiscount(clientId);
            string clientName = _clientService.GetClientName(clientId);

            MessageBox.Show($"Клієнт: {clientName}\nЗагальні витрати: {totalExpenses} грн\nЗнижка: {discount * 100}%");
        }
    }
}
