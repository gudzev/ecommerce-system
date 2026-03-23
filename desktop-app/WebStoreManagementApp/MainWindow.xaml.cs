using Backend;
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
using System.Text.Json;

namespace WebStoreManagementApp
{
    public partial class MainWindow : Window
    {
        private static HttpClient client = new HttpClient();
        private List<Grid> grids = new List<Grid>();
        private List<DeliveryOption> deliveryOptions = new List<DeliveryOption>();
        public MainWindow()
        {
            InitializeComponent();
            LoadGrids();
        }

        private void LoadGrids()
        {
            grids.Add(Proizvodi);
            grids.Add(Narudzbine);
            grids.Add(Dostava);
            showGrid(Proizvodi); // show default grid
        }

        private void showGrid(Grid g)
        {
            foreach (var grid in grids)
            {
                if(grid == g)
                {
                    grid.Visibility = Visibility.Visible;
                }
                else
                {
                    grid.Visibility = Visibility.Hidden;
                }

            }
        }

        private void ProizvodiBtn_Click(object sender, RoutedEventArgs e)
        {
            showGrid(Proizvodi);
            CurrentGridLabel.Content = "Proizvodi";
        }

        private void NarudzbineBtn_Click(object sender, RoutedEventArgs e)
        {
            showGrid(Narudzbine);
            CurrentGridLabel.Content = "Narudžbine";
        }





        /* Dostava */
        private void DostavaBtn_Click(object sender, RoutedEventArgs e)
        {
            showGrid(Dostava);
            CurrentGridLabel.Content = "Dostava";

            if (deliveryOptions.Count != 0)
            {
                MethodsTable.ItemsSource = deliveryOptions;
            }
            else
            {
                LoadMethodsTable();
            }
        }

        private async void LoadMethodsTable()
        {
            try
            {
                HttpResponseMessage response = await client.GetAsync("https://webstoreapi-cpb8c7fqfxf6dree.germanywestcentral-01.azurewebsites.net/delivery-options");
                response.EnsureSuccessStatusCode();
                deliveryOptions = await response.Content.ReadFromJsonAsync<List<DeliveryOption>>();

                MethodsTable.ItemsSource = deliveryOptions;
                LoadFirstMethodsRow();
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
            optionNameTextBox.Text = deliveryOption.name.ToString();
            pricePerProductTextBox.Text = deliveryOption.price_per_item.ToString();
        }

        private void LoadFirstMethodsRow()
        {
            minimumValueTextBox.Text = deliveryOptions[0].free_shipping_minimum_value.ToString();
            optionNameTextBox.Text = deliveryOptions[0].name.ToString();
            pricePerProductTextBox.Text = deliveryOptions[0].price_per_item.ToString();
        }

        private async void methodDeleteBtn_Click(object sender, RoutedEventArgs e)
        {
            int deliveryMethodId;
            if (MethodsTable.SelectedItem is DeliveryOption selected)
            {
                deliveryMethodId = selected.id;
            }
            else
            {
                return;
            }

            try
            {
                HttpResponseMessage response = await client.DeleteAsync("https://webstoreapi-cpb8c7fqfxf6dree.germanywestcentral-01.azurewebsites.net/delete-delivery-option/" + deliveryMethodId);
                response.EnsureSuccessStatusCode();
                LoadMethodsTable();
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private async void methodUpdateBtn_Click(object sender, RoutedEventArgs e)
        {
            DeliveryOption deliveryOption = getDeliveryOption();

            try
            {
                HttpResponseMessage response = await client.PutAsJsonAsync("https://webstoreapi-cpb8c7fqfxf6dree.germanywestcentral-01.azurewebsites.net/update-delivery-option/", deliveryOption);
                response.EnsureSuccessStatusCode();
                LoadMethodsTable();
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message); 
            }
        }

        private async void methodAddBtn_Click(object sender, RoutedEventArgs e)
        {
            DeliveryOption deliveryOption = getDeliveryOption();

            try
            {
                HttpResponseMessage response = await client.PostAsJsonAsync("https://webstoreapi-cpb8c7fqfxf6dree.germanywestcentral-01.azurewebsites.net/add-delivery-option/", deliveryOption);
                response.EnsureSuccessStatusCode();
                LoadMethodsTable();
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message); 
            }
        }

        private DeliveryOption getDeliveryOption()
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