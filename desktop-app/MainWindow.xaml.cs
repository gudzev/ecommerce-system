using Backend;
using DesktopApp.Pages;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Net.Http.Json;
using System.Windows;
using System.Windows.Controls;

namespace WebStoreManagementApp
{
    public partial class MainWindow : Window
    {
        public static HttpClient client = new HttpClient();

        public static ObservableCollection<DeliveryOption> deliveryOptions = new ObservableCollection<DeliveryOption>();
        public static ObservableCollection<Product> products = new ObservableCollection<Product>();
        public static ObservableCollection<Order> orders = new ObservableCollection<Order>();
        public static ObservableCollection<Category> categories = new ObservableCollection<Category>();

        public static Dictionary<string, Page> pages = new()
        {
            ["productsPage"] = new ProductsPage(),
            ["ordersPage"] = new OrdersPage(),
            ["deliveryMethodsPage"] = new DeliveryMethodsPage(),
            ["categoriesPage"] = new CategoriesPage()
        };

        public static bool shouldRefreshCategories = true;
        public static bool shouldRefreshDeliveryOptions = true;
        public MainWindow()
        {
            InitializeComponent();
            MainFrame.Navigate(pages["productsPage"]);
        }

        private async void KategorijaBtn_Click(object sender, RoutedEventArgs e)
        {
            CurrentGridLabel.Content = "Kategorije";
            MainFrame.Navigate(pages["categoriesPage"]);
        }

        private async void ProizvodiBtn_Click(object sender, RoutedEventArgs e)
        {
            CurrentGridLabel.Content = "Proizvodi";
            MainFrame.Navigate(pages["productsPage"]);
        }

        private async void NarudzbineBtn_Click(object sender, RoutedEventArgs e)
        {
            CurrentGridLabel.Content = "Narudžbine";
            MainFrame.Navigate(pages["ordersPage"]);
        }
        private async void DostavaBtn_Click(object sender, RoutedEventArgs e)
        {
            CurrentGridLabel.Content = "Dostava";
            MainFrame.Navigate(pages["deliveryMethodsPage"]);
        }

        public static async Task getDeliveryOptions()
        {
            if (!shouldRefreshDeliveryOptions && deliveryOptions.Count > 0)
            {
                return;
            }

            shouldRefreshDeliveryOptions = false;

            try
            {
                HttpResponseMessage response = await client.GetAsync("https://webstoreapi-cpb8c7fqfxf6dree.germanywestcentral-01.azurewebsites.net/delivery-options");
                response.EnsureSuccessStatusCode();
                deliveryOptions = await response.Content.ReadFromJsonAsync<ObservableCollection<DeliveryOption>>();
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public static async Task getCategories()
        {
            if(!shouldRefreshCategories && categories.Count > 0)
            {
                return;
            }

            shouldRefreshCategories = false;

            try
            {
                HttpResponseMessage response = await client.GetAsync("https://webstoreapi-cpb8c7fqfxf6dree.germanywestcentral-01.azurewebsites.net/categories");
                response.EnsureSuccessStatusCode();
                categories = await response.Content.ReadFromJsonAsync<ObservableCollection<Category>>();
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}