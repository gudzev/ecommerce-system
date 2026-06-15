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
    /// Interaction logic for DeliveryMethodsPage.xaml
    /// </summary>
    public partial class DeliveryMethodsPage : Page
    {
        public DeliveryMethodsPage()
        {
            InitializeComponent();
        }

        private async void DeliveryMethodsPageLoaded(object sender, RoutedEventArgs e)
        {
            await LoadMethodsTable();
            LoadFirstMethodsRow();
        }

        private async Task getDeliveryOptions()
        {
            if (!MainWindow.shouldRefreshDeliveryOptions && MainWindow.deliveryOptions.Count > 0)
            {
                return;
            }

            MainWindow.shouldRefreshDeliveryOptions = false;

            try
            {
                HttpResponseMessage response = await MainWindow.client.GetAsync("https://localhost:7097/delivery-options");
                response.EnsureSuccessStatusCode();
                MainWindow.deliveryOptions = await response.Content.ReadFromJsonAsync<ObservableCollection<DeliveryOption>>();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private async Task LoadMethodsTable()
        {
            try
            {
                await getDeliveryOptions();
                ; MethodsTable.ItemsSource = MainWindow.deliveryOptions;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        private void LoadMethodsRow(object sender, SelectionChangedEventArgs e)
        {
            DeliveryOption deliveryOption = (DeliveryOption)MethodsTable.SelectedItem;

            if (deliveryOption == null) deliveryOption = (DeliveryOption)MethodsTable.Items[0];

            minimumValueTextBox.Text = deliveryOption.free_shipping_minimum_value.ToString();
            optionNameTextBox.Text = deliveryOption.name;
            pricePerProductTextBox.Text = deliveryOption.price_per_item.ToString();
        }

        private void LoadFirstMethodsRow()
        {
            if (MethodsTable.Items.Count == 0) return;

            minimumValueTextBox.Text = MainWindow.deliveryOptions[0].free_shipping_minimum_value.ToString();
            optionNameTextBox.Text = MainWindow.deliveryOptions[0].name;
            pricePerProductTextBox.Text = MainWindow.deliveryOptions[0].price_per_item.ToString();
        }

        private async void methodDeleteBtn_Click(object sender, RoutedEventArgs e)
        {
            int deliveryMethodId;
            if (MethodsTable.SelectedItem is DeliveryOption selected)
            {
                deliveryMethodId = selected.id;
                MainWindow.shouldRefreshDeliveryOptions = true;
            }
            else
            {
                return;
            }

            try
            {
                HttpResponseMessage response = await MainWindow.client.DeleteAsync("https://localhost:7097/delivery-options/" + deliveryMethodId);
                response.EnsureSuccessStatusCode();
                await LoadMethodsTable();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private async void methodUpdateBtn_Click(object sender, RoutedEventArgs e)
        {
            DeliveryOption deliveryOption = getSelectedDeliveryOption();
            MainWindow.shouldRefreshDeliveryOptions = true;

            try
            {
                HttpResponseMessage response = await MainWindow.client.PutAsJsonAsync("https://localhost:7097/delivery-options/", deliveryOption);
                response.EnsureSuccessStatusCode();
                await LoadMethodsTable();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private async void methodAddBtn_Click(object sender, RoutedEventArgs e)
        {
            DeliveryOption deliveryOption = getSelectedDeliveryOption();
            MainWindow.shouldRefreshDeliveryOptions = true;

            try
            {
                HttpResponseMessage response = await MainWindow.client.PostAsJsonAsync("https://localhost:7097/delivery-options/", deliveryOption);
                response.EnsureSuccessStatusCode();
                await LoadMethodsTable();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private DeliveryOption getSelectedDeliveryOption()
        {
            DeliveryOption deliveryOption = new DeliveryOption();

            if (MethodsTable.SelectedItem is DeliveryOption selected)
            {
                deliveryOption.id = selected.id;
                deliveryOption.free_shipping_minimum_value = int.Parse(minimumValueTextBox.Text);
                deliveryOption.price_per_item = int.Parse(pricePerProductTextBox.Text);
                deliveryOption.name = optionNameTextBox.Text;

            }
            return deliveryOption;
        }
    }
}
