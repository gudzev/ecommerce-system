using Backend;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Net.Http.Json;
using System.Windows;
using System.Windows.Controls;

namespace WebStoreManagementApp
{
    public partial class MainWindow : Window
    {
        private static HttpClient client = new HttpClient();

        private List<Grid> grids = new List<Grid>();

        private ObservableCollection<DeliveryOption> deliveryOptions = new ObservableCollection<DeliveryOption>();
        private ObservableCollection<Product> products = new ObservableCollection<Product>();
        private ObservableCollection<Order> orders = new ObservableCollection<Order>();
        private ObservableCollection<Category> categories = new ObservableCollection<Category>();
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
            grids.Add(Kategorije);

            showGrid(Proizvodi); // show default grid
            await fillCategoriesComboBox();
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
        private async void ProizvodiBtn_Click(object sender, RoutedEventArgs e)
        {
            showGrid(Proizvodi);
            CurrentGridLabel.Content = "Proizvodi";
            await fillCategoriesComboBox();
            LoadProductsTable();
        }

        private async Task fillCategoriesComboBox()
        {

            await getCategories(); // fill combobox on products (default) grid
            categoryComboBox.ItemsSource = categories;
            categoryComboBox.DisplayMemberPath = "name";
        }

        private async void LoadProductsTable()
        {
            try
            {
                HttpResponseMessage response = await client.GetAsync("https://webstoreapi-cpb8c7fqfxf6dree.germanywestcentral-01.azurewebsites.net/products");
                response.EnsureSuccessStatusCode();
                products = await response.Content.ReadFromJsonAsync<ObservableCollection<Product>>();

                ProductsTable.ItemsSource = products;
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

            if(product == null) product = (Product)ProductsTable.Items[0];

            productNameTextBox.Text = product.name;
            imageURLTextBox.Text = product.image_url;
            priceTextBox.Text = product.price_rsd.ToString();
            salePriceTextBox.Text = product.price_on_sale.ToString() ?? "";
            quantityTextBox.Text = product.stock_quantity.ToString();

            foreach(var category in categories)
            {
                if(category.id == product.category_id)
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

            foreach (var category in categories)
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
            newProduct.price_on_sale = (salePriceTextBox.Text == "") ? null : Convert.ToInt32(salePriceTextBox.Text);
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
            existingProduct.price_on_sale = (salePriceTextBox.Text == "") ? null : Convert.ToInt32(salePriceTextBox.Text);
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
        private async void NarudzbineBtn_Click(object sender, RoutedEventArgs e)
        {
            showGrid(Narudzbine);
            CurrentGridLabel.Content = "Narudžbine";
            await fillDeliveryMethodsComboBox();
            await LoadOrdersTable(0);
            LoadFirstOrdersRow();
        }

        private async Task fillDeliveryMethodsComboBox()
        {
            await fillDeliveryOptions();
            deliveryMethodComboBox.ItemsSource = deliveryOptions;
            deliveryMethodComboBox.DisplayMemberPath = "name";
        }

        private async Task LoadOrdersTable(int is_fulfilled)
        {
            try
            {
                HttpResponseMessage response = await client.GetAsync("https://webstoreapi-cpb8c7fqfxf6dree.germanywestcentral-01.azurewebsites.net/orders?is_fulfilled=" + is_fulfilled);
                response.EnsureSuccessStatusCode();

                orders = await response.Content.ReadFromJsonAsync<ObservableCollection<Order>>();
                OrdersTable.ItemsSource = orders;
            }
            catch(Exception ex)
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

            foreach (var deliveryMethod in deliveryOptions)
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

            foreach (var deliveryMethod in deliveryOptions)
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

            if(OrdersTable.SelectedItem is Order o)
            {
                selectedOrder = o;
            }

            if (selectedOrder == null) return;

            try
            {
                HttpResponseMessage response = await client.PatchAsJsonAsync("https://webstoreapi-cpb8c7fqfxf6dree.germanywestcentral-01.azurewebsites.net/update-order/", selectedOrder);
                response.EnsureSuccessStatusCode();
                await LoadOrdersTable(0);
            }
            catch(Exception ex)
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









        /* Dostava */
        private void DostavaBtn_Click(object sender, RoutedEventArgs e)
        {
            showGrid(Dostava);
            CurrentGridLabel.Content = "Dostava";
            LoadMethodsTable();
        }

        private async Task fillDeliveryOptions()
        {
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

        private async void LoadMethodsTable()
        {
            try
            {
                await fillDeliveryOptions();
;               MethodsTable.ItemsSource = deliveryOptions;
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
            optionNameTextBox.Text = deliveryOption.name;
            pricePerProductTextBox.Text = deliveryOption.price_per_item.ToString();
        }

        private void LoadFirstMethodsRow()
        {
            if (MethodsTable.Items.Count == 0) return;

            minimumValueTextBox.Text = deliveryOptions[0].free_shipping_minimum_value.ToString();
            optionNameTextBox.Text = deliveryOptions[0].name;
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


        /* Kategorije */
        private async Task getCategories()
        {
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

        private async Task LoadCategoriesTable()
        {
            try
            {
                await getCategories();
                CategoriesTable.ItemsSource = categories;
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        private async void KategorijaBtn_Click(object sender, RoutedEventArgs e)
        {
            showGrid(Kategorije);
            CurrentGridLabel.Content = "Kategorije";
            await LoadCategoriesTable();
            LoadFirstCategoriesRow();
        }

        private void LoadCategoriesRow(object sender, SelectionChangedEventArgs e)
        {
            Category category = (Category)CategoriesTable.SelectedItem;

            if (category == null) category = (Category)CategoriesTable.Items[0];

            categoryNameTextBox.Text = category.name;
        }

        private void LoadFirstCategoriesRow()
        {
            if (CategoriesTable.Items.Count == 0) return;

            Category category = (Category)CategoriesTable.Items[0];
            categoryNameTextBox.Text = category.name;
        }

        private async void addCategoryBtn_Click(object sender, RoutedEventArgs e)
        {
            if (categoryNameTextBox.Text == "") return;

            Category newCategory = new Category();

            newCategory.name = categoryNameTextBox.Text;

            try
            {
                HttpResponseMessage response = await client.PostAsJsonAsync("https://webstoreapi-cpb8c7fqfxf6dree.germanywestcentral-01.azurewebsites.net/add-category", newCategory);
                response.EnsureSuccessStatusCode();
                await LoadCategoriesTable();
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message); 
            }
        }

        private async void updateCategoryBtn_Click(object sender, RoutedEventArgs e)
        {
            if (categoryNameTextBox.Text == "") return;

            Category existingCategory = (Category)CategoriesTable.SelectedItem;
            existingCategory.name = categoryNameTextBox.Text;

            try
            {
                HttpResponseMessage response = await client.PutAsJsonAsync("https://webstoreapi-cpb8c7fqfxf6dree.germanywestcentral-01.azurewebsites.net/update-category", existingCategory);
                response.EnsureSuccessStatusCode();
                await LoadCategoriesTable();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private async void removeCategoryBtn_Click(object sender, RoutedEventArgs e)
        {
            if (CategoriesTable.SelectedItem == null) return;
            Category selectedCategory = (Category)CategoriesTable.SelectedItem;

            try
            {
                HttpResponseMessage response = await client.DeleteAsync("https://webstoreapi-cpb8c7fqfxf6dree.germanywestcentral-01.azurewebsites.net/delete-category/" + selectedCategory.id);
                response.EnsureSuccessStatusCode();
                await LoadCategoriesTable();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}