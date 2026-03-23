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

        private async void DostavaBtn_Click(object sender, RoutedEventArgs e)
        {
            showGrid(Dostava);
            CurrentGridLabel.Content = "Dostava";

            try
            {
                HttpResponseMessage response = await client.GetAsync("https://webstoreapi-cpb8c7fqfxf6dree.germanywestcentral-01.azurewebsites.net/delivery-options");
                response.EnsureSuccessStatusCode();

                if(deliveryOptions.Count == 0)
                {
                    deliveryOptions = await response.Content.ReadFromJsonAsync<List<DeliveryOption>>();

                }
                MethodsTable.ItemsSource = deliveryOptions;
                LoadFirstRow();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        private void LoadMethodsRow(object sender, SelectionChangedEventArgs e)
        {
            DeliveryOption deliveryOption = (DeliveryOption)MethodsTable.SelectedItem;

            MethodIDTextBox.Text = deliveryOption.id.ToString();
            minimumValueTextBox.Text = deliveryOption.free_shipping_minimum_value.ToString();
            optionNameTextBox.Text = deliveryOption.name.ToString();
            pricePerProductTextBox.Text = deliveryOption.price_per_item.ToString();
            optionIsDefaultCheckBox.IsChecked = deliveryOption.is_default;
        }

        private void LoadFirstRow()
        {
            MethodIDTextBox.Text = deliveryOptions[0].id.ToString();
            minimumValueTextBox.Text = deliveryOptions[0].free_shipping_minimum_value.ToString();
            optionNameTextBox.Text = deliveryOptions[0].name.ToString();
            pricePerProductTextBox.Text = deliveryOptions[0].price_per_item.ToString();
            optionIsDefaultCheckBox.IsChecked = deliveryOptions[0].is_default;
        }
    }
}