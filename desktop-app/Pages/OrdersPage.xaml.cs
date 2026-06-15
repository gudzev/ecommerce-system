using Backend;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WebStoreManagementApp;

namespace DesktopApp.Pages
{
    /// <summary>
    /// Interaction logic for OrdersPage.xaml
    /// </summary>
    public partial class OrdersPage : Page
    {
        public OrdersPage()
        {
            InitializeComponent();
        }

        private async void OrdersPageLoaded(object sender, RoutedEventArgs e)
        {
            await fillDeliveryMethodsComboBox();
            await LoadOrdersTable(0);
            LoadFirstOrdersRow();
        }

        private async Task fillDeliveryMethodsComboBox()
        {
            await MainWindow.getDeliveryOptions();
            deliveryMethodComboBox.ItemsSource = MainWindow.deliveryOptions;
            deliveryMethodComboBox.DisplayMemberPath = "name";
        }

        private async Task LoadOrdersTable(int is_fulfilled)
        {
            try
            {
                HttpResponseMessage response = await MainWindow.client.GetAsync("https://localhost:7097/orders?is_fulfilled=" + is_fulfilled);
                response.EnsureSuccessStatusCode();

                MainWindow.orders = await response.Content.ReadFromJsonAsync<ObservableCollection<Order>>();
                OrdersTable.ItemsSource = MainWindow.orders;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void LoadOrdersRow(object sender, SelectionChangedEventArgs e)
        {
            Order order = (Order)OrdersTable.SelectedItem;

            if (order == null) order = (Order)OrdersTable.Items[0];

            emailTextBox.Text = order.email;
            nameTextBox.Text = order.name;
            surnameTextBox.Text = order.surname;
            streetTextBox.Text = order.street ?? "";
            apartmentNumberTextBox.Text = order.apartment_number ?? "";
            cityTextBox.Text = order.city ?? "";
            phoneTextBox.Text = order.phone_number;
            additionalTextBox.Text = order.additional ?? "";
            orderDateTextBox.Text = order.created_at.ToString();

            foreach (var deliveryMethod in MainWindow.deliveryOptions)
            {
                if (deliveryMethod.id == order.delivery_method_id)
                {
                    deliveryMethodComboBox.SelectedItem = deliveryMethod;
                    break;
                }
            }
        }

        private void LoadFirstOrdersRow()
        {
            if (OrdersTable.Items.Count == 0) return;

            Order order = (Order)OrdersTable.Items[0];

            emailTextBox.Text = order.email;
            nameTextBox.Text = order.name;
            surnameTextBox.Text = order.surname;
            streetTextBox.Text = order.street ?? "";
            apartmentNumberTextBox.Text = order.apartment_number ?? "";
            cityTextBox.Text = order.city ?? "";
            phoneTextBox.Text = order.phone_number;
            additionalTextBox.Text = order.additional ?? "";
            orderDateTextBox.Text = order.created_at.ToString();

            foreach (var deliveryMethod in MainWindow.deliveryOptions)
            {
                if (deliveryMethod.id == order.delivery_method_id)
                {
                    deliveryMethodComboBox.SelectedItem = deliveryMethod;
                    break;
                }
            }
        }

        private async void markAsFulfilledBtn_Click(object sender, RoutedEventArgs e)
        {
            Order? selectedOrder = null;

            if (OrdersTable.SelectedItem is Order o)
            {
                selectedOrder = o;
            }

            if (selectedOrder == null) return;

            try
            {
                HttpResponseMessage response = await MainWindow.client.PatchAsJsonAsync("https://localhost:7097/orders/", selectedOrder);
                response.EnsureSuccessStatusCode();
                await LoadOrdersTable(0);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        private async void historyBtn_Click(object sender, RoutedEventArgs e)
        {
            await LoadOrdersTable(1);
            LoadFirstOrdersRow();
        }
        private async void allOrdersBtn_Click(object sender, RoutedEventArgs e)
        {
            await LoadOrdersTable(0);
            LoadFirstOrdersRow();
        }
    }
}
