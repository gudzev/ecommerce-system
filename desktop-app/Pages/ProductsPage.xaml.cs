using Backend;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Net.Http.Json;
using System.Windows;
using System.Windows.Controls;
using WebStoreManagementApp;

namespace DesktopApp.Pages
{
    public partial class ProductsPage : Page
    {
        public ProductsPage()
        {
            InitializeComponent();
        }

        private async void ProductsPageLoaded(object sender, RoutedEventArgs e)
        {
            await fillCategoriesComboBox();
            LoadProductsTable();
        }

        private async Task fillCategoriesComboBox()
        {

            await MainWindow.getCategories(); // fill combobox on products (default) grid
            categoryComboBox.ItemsSource = MainWindow.categories;
            categoryComboBox.DisplayMemberPath = "name";
        }

        private async void LoadProductsTable()
        {
            try
            {
                HttpResponseMessage response = await MainWindow.client.GetAsync("https://webstoreapi-cpb8c7fqfxf6dree.germanywestcentral-01.azurewebsites.net/products");
                response.EnsureSuccessStatusCode();
                MainWindow.products = await response.Content.ReadFromJsonAsync<ObservableCollection<Product>>();

                ProductsTable.ItemsSource = MainWindow.products;
                LoadFirstProductsRow();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void LoadProductsRow(object sender, SelectionChangedEventArgs e)
        {
            Product product = (Product)ProductsTable.SelectedItem;

            if (product == null) product = (Product)ProductsTable.Items[0];

            productNameTextBox.Text = product.name;
            imageURLTextBox.Text = product.image_url;
            priceTextBox.Text = product.price_rsd.ToString();
            salePriceTextBox.Text = product.price_on_sale.ToString() ?? "";
            quantityTextBox.Text = product.stock_quantity.ToString();

            foreach (var category in MainWindow.categories)
            {
                if (category.id == product.category_id)
                {
                    categoryComboBox.SelectedItem = category;
                    break;
                }
            }
        }

        private void LoadFirstProductsRow()
        {
            if (ProductsTable.Items.Count == 0) return;

            Product product = (Product)ProductsTable.Items[0];

            productNameTextBox.Text = product.name;
            imageURLTextBox.Text = product.image_url;
            priceTextBox.Text = product.price_rsd.ToString();
            salePriceTextBox.Text = product.price_on_sale.ToString() ?? "";
            quantityTextBox.Text = product.stock_quantity.ToString();

            foreach (var category in MainWindow.categories)
            {
                if (category.id == product.category_id)
                {
                    categoryComboBox.SelectedItem = category;
                }
            }
        }

        private async void productAddBtn_Click(object sender, RoutedEventArgs e)
        {
            Product newProduct = new Product();

            int categoryId = -1;
            foreach (var category in MainWindow.categories)
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
            newProduct.price_on_sale = (salePriceTextBox.Text == "") ? null : Convert.ToInt32(salePriceTextBox.Text);
            newProduct.category_id = categoryId;
            newProduct.stock_quantity = Convert.ToInt32(quantityTextBox.Text);

            try
            {
                HttpResponseMessage response = await MainWindow.client.PostAsJsonAsync("https://webstoreapi-cpb8c7fqfxf6dree.germanywestcentral-01.azurewebsites.net/products/", newProduct);
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
            foreach (var category in MainWindow.categories)
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
            existingProduct.price_on_sale = (salePriceTextBox.Text == "") ? null : Convert.ToInt32(salePriceTextBox.Text);
            existingProduct.category_id = categoryId;
            existingProduct.stock_quantity = Convert.ToInt32(quantityTextBox.Text);

            try
            {
                HttpResponseMessage response = await MainWindow.client.PutAsJsonAsync("https://webstoreapi-cpb8c7fqfxf6dree.germanywestcentral-01.azurewebsites.net/products/", existingProduct);
                response.EnsureSuccessStatusCode();
                LoadProductsTable();
            }
            catch (Exception ex)
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

                HttpResponseMessage response = await MainWindow.client.PatchAsync("https://webstoreapi-cpb8c7fqfxf6dree.germanywestcentral-01.azurewebsites.net/products/" + productId + "/status?isActive=" + status,
                    null
                    );
                response.EnsureSuccessStatusCode();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
