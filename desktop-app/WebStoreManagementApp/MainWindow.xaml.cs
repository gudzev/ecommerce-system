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
        private List<Product> products = new List<Product>();
        private List<Order> orders = new List<Order>();
        private List<Category> categories = new List<Category>();
        public MainWindow()
        {
            InitializeComponent();
            LoadGrids();
        }

        private async void LoadGrids()
        {
            grids.Add(Proizvodi);
            grids.Add(Narudzbine);
            grids.Add(Dostava);

            showGrid(Proizvodi); // show default grid

            await getCategories(); // fill combobox on products (default) grid
            categoryComboBox.ItemsSource = categories;
            categoryComboBox.DisplayMemberPath = "name";

            LoadProductsTable();
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


        /* Proizvodi */
        private void ProizvodiBtn_Click(object sender, RoutedEventArgs e)
        {
            showGrid(Proizvodi);
            CurrentGridLabel.Content = "Proizvodi";

            LoadProductsTable();
        }

        private async void LoadProductsTable()
        {
            try
            {
                HttpResponseMessage response = await client.GetAsync("https://webstoreapi-cpb8c7fqfxf6dree.germanywestcentral-01.azurewebsites.net/products");
                response.EnsureSuccessStatusCode();
                products = await response.Content.ReadFromJsonAsync<List<Product>>();

                ProductsTable.ItemsSource = products;
                LoadFirstProductsRow();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        private void LoadProductsRow(object sender, SelectionChangedEventArgs e)
        {
            Product product = (Product)ProductsTable.SelectedItem;

            if(product == null) product = (Product)ProductsTable.Items[0];

            productNameTextBox.Text = product.name;
            imageURLTextBox.Text = product.image_url;
            priceTextBox.Text = product.price_rsd.ToString();
            salePriceTextBox.Text = (product.price_on_sale != null) ? product.price_on_sale.ToString() : "";
            categoryComboBox.SelectedIndex = product.category_id - 1;
            quantityTextBox.Text = product.stock_quantity.ToString();
        }

        private void LoadFirstProductsRow()
        {
            productNameTextBox.Text = products[0].name;
            imageURLTextBox.Text = products[0].image_url;
            priceTextBox.Text = products[0].price_rsd.ToString();
            salePriceTextBox.Text = (products[0].price_on_sale != null) ? products[0].price_rsd.ToString() : "";
            categoryComboBox.SelectedIndex = products[0].category_id;
            quantityTextBox.Text = products[0].stock_quantity.ToString();
        }

        private async void productAddBtn_Click(object sender, RoutedEventArgs e)
        {
            Product newProduct = new Product();

            int categoryId = -1;
            foreach (var category in categories)
            {
                if (categoryComboBox.SelectedItem == category)
                {
                    categoryId = category.id;
                    break;
                }
            }

            if (categoryId == -1) return;

            newProduct.name = productNameTextBox.Text;
            newProduct.image_url = imageURLTextBox.Text;
            newProduct.price_rsd = Convert.ToInt32(priceTextBox.Text);
            newProduct.price_on_sale = (salePriceTextBox.Text != "") ? Convert.ToInt32(salePriceTextBox.Text) : null;
            newProduct.category_id = categoryId;
            newProduct.stock_quantity = Convert.ToInt32(quantityTextBox.Text);

            try
            {
                HttpResponseMessage response = await client.PostAsJsonAsync("https://webstoreapi-cpb8c7fqfxf6dree.germanywestcentral-01.azurewebsites.net/add-product/", newProduct);
                response.EnsureSuccessStatusCode();
                LoadProductsTable();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private async void productUpdateBtn_Click(object sender, RoutedEventArgs e)
        {
            Product existingProduct = new Product();

            if (ProductsTable.SelectedItem is Product selectedProduct)
            {
                existingProduct.id = selectedProduct.id;
            }
            else
            {
                return;
            }

            int categoryId = -1;
            foreach (var category in categories)
            {
                if (categoryComboBox.SelectedItem == category)
                {
                    categoryId = category.id;
                    break;
                }
            }

            if (categoryId == -1) return;

            existingProduct.name = productNameTextBox.Text;
            existingProduct.image_url = imageURLTextBox.Text;
            existingProduct.price_rsd = Convert.ToInt32(priceTextBox.Text);
            existingProduct.price_on_sale = (salePriceTextBox.Text != "") ? Convert.ToInt32(salePriceTextBox.Text) : null;
            existingProduct.category_id = categoryId;
            existingProduct.stock_quantity = Convert.ToInt32(quantityTextBox.Text);

            try
            {
                HttpResponseMessage response = await client.PutAsJsonAsync("https://webstoreapi-cpb8c7fqfxf6dree.germanywestcentral-01.azurewebsites.net/update-product/", existingProduct);
                response.EnsureSuccessStatusCode();
                LoadProductsTable();
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private async void productDeactivateBtn_Click(object sender, RoutedEventArgs e)
        {
            setProductStatus(false);
            LoadProductsTable();
        }

        private async void productActivateBtn_Click(object sender, RoutedEventArgs e)
        {
            setProductStatus(true);
            LoadProductsTable();
        }

        async void setProductStatus(bool status)
        {
            try
            {
                int productId;
                if (ProductsTable.SelectedItem is Product selectedProduct)
                {
                    productId = selectedProduct.id;
                }
                else
                {
                    return;
                }

                HttpResponseMessage response = await client.PatchAsync("https://webstoreapi-cpb8c7fqfxf6dree.germanywestcentral-01.azurewebsites.net/products/" + productId + "/status?isActive=" + status,
                    null
                    );
                response.EnsureSuccessStatusCode();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }








        /* Narudzbine */
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
            LoadMethodsTable();
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


        /* Categories */
        private async Task getCategories()
        {
            try
            {
                HttpResponseMessage response = await client.GetAsync("https://webstoreapi-cpb8c7fqfxf6dree.germanywestcentral-01.azurewebsites.net/categories");
                response.EnsureSuccessStatusCode();
                categories = await response.Content.ReadFromJsonAsync<List<Category>>();
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}