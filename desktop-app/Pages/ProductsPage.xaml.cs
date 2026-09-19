using Backend.Models;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Net.Http.Json;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using WebStoreManagementApp;

namespace DesktopApp.Pages
{
    public partial class ProductsPage : Page
    {
        public ProductsPage()
        {
            InitializeComponent();
        }

        Product selectedProduct = new Product();

        List<ProductSpecification> specifications = new List<ProductSpecification>();
        List<Label> labels = new List<Label>();
        List<TextBox> textBoxes = new List<TextBox>();

        Translator t = new Translator();

        private async void ProductsPageLoaded(object sender, RoutedEventArgs e)
        {
            await fillCategoriesComboBox();
            await LoadProductsTable();
        }

        private async Task fillCategoriesComboBox()
        {

            await MainWindow.getCategories(); // fill combobox on products (default) grid
            categoryComboBox.ItemsSource = MainWindow.categories;
            categoryComboBox.DisplayMemberPath = "name";
        }

        private async void categoryComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedCategory = (Category)categoryComboBox.SelectedItem;

            HttpResponseMessage response = await MainWindow.client.GetAsync(MainWindow.API_URL + "/category-specifications?categoryId=" + selectedCategory.id);
            response.EnsureSuccessStatusCode();

            var categorySpecifications = await response.Content.ReadFromJsonAsync<List<CategorySpecification>>() ?? [];

            if (categorySpecifications.Count <= 0) return;

            clearProductSpecifications();

            for (int i = 0; i < categorySpecifications.Count; i++)
            {
                RowDefinition rowDefinition = new RowDefinition();
                rowDefinition.Height = new GridLength(60);

                SpecificationsGrid.RowDefinitions.Add(rowDefinition);
            }

            int row = 0;
            int labelColumn = 0;
            int textBoxColumn = 1;

            categorySpecifications.ForEach((categorySpecification) =>
            {
                Label label = new Label();

                if(categorySpecification.name != null)
                    label.Content = t.TranslateToSerbian(categorySpecification.name.ToLower(), true);

                label.Style = (Style)Application.Current.FindResource("TextBoxLabel");
                labels.Add(label);
                Grid.SetRow(label, row);
                Grid.SetColumn(label, labelColumn);
                SpecificationsGrid.Children.Add(label);


                TextBox textBox = new TextBox();
                textBox.Style = (Style)Application.Current.FindResource("TextBox");
                textBox.Text = "";
                textBoxes.Add(textBox);
                Grid.SetRow(textBox, row);
                Grid.SetColumn(textBox, textBoxColumn);
                SpecificationsGrid.Children.Add(textBox);

                ProductSpecification newSpec = new ProductSpecification();
                newSpec.category_specification_id = categorySpecification.category_specification_id;
                newSpec.name = categorySpecification.name;

                specifications.Add(newSpec);

                row++;
            });
        }

        private async Task LoadProductsTable()
        {
            try
            {
                HttpResponseMessage response = await MainWindow.client.GetAsync(MainWindow.API_URL + "/products");
                response.EnsureSuccessStatusCode();
                MainWindow.products = await response.Content.ReadFromJsonAsync<ObservableCollection<Product>>() ?? [];

                ProductsTable.ItemsSource = MainWindow.products;
                await LoadFirstProductsRow();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private async void LoadProductsRow(object sender, SelectionChangedEventArgs e)
        {
            if(ProductsTable.SelectedItem is not Product p)
            {
                return;
            }

            Product product = (Product)ProductsTable.SelectedItem;
            selectedProduct = await MainWindow.client.GetFromJsonAsync<Product>(MainWindow.API_URL + "/products/" + product.id) ?? product;

            clearTextBoxes();
            productNameTextBox.Text = selectedProduct.name;
            imageURLTextBox.Text = (selectedProduct.images.Find(image => image.is_main_image == true))?.url;
            priceTextBox.Text = selectedProduct.price_rsd.ToString();
            salePriceTextBox.Text = selectedProduct.price_on_sale.ToString() ?? "";
            quantityTextBox.Text = selectedProduct.stock_quantity.ToString();
            descriptionTextBox.Text = selectedProduct?.description?.ToString();

            foreach (var category in MainWindow.categories)
            {
                if (category.id == selectedProduct?.category_id)
                {
                    categoryComboBox.SelectedItem = category;
                    break;
                }
            }

            await getProductSpecifications(product.id);
        }

        private async Task LoadFirstProductsRow()
        {
            if (ProductsTable.Items.Count == 0) return;

            Product? product = (Product)ProductsTable.Items[0] ?? null;

            if (product == null) return;

            productNameTextBox.Text = product.name;
            imageURLTextBox.Text = product.image_url;
            priceTextBox.Text = product.price_rsd.ToString();
            salePriceTextBox.Text = product.price_on_sale.ToString() ?? "";
            quantityTextBox.Text = product.stock_quantity.ToString();
            descriptionTextBox.Text = product?.description?.ToString();

            foreach (var category in MainWindow.categories)
            {
                if (category.id == product?.category_id)
                {
                    categoryComboBox.SelectedItem = category;
                }
            }

            await getProductSpecifications(product.id);
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
            newProduct.images.Add(new Backend.Models.Image(imageURLTextBox.Text));
            newProduct.price_rsd = Convert.ToInt32(priceTextBox.Text);
            newProduct.price_on_sale = (salePriceTextBox.Text == "") ? null : Convert.ToInt32(salePriceTextBox.Text);
            newProduct.category_id = categoryId;
            newProduct.stock_quantity = Convert.ToInt32(quantityTextBox.Text);
            newProduct.description = descriptionTextBox.Text;
            newProduct.specifications = getUpdatedSpecifications(specifications);

            try
            {
                HttpResponseMessage response = await MainWindow.client.PostAsJsonAsync(MainWindow.API_URL + "/products/", newProduct);
                response.EnsureSuccessStatusCode();
                await LoadProductsTable();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private async void productUpdateBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Product newProduct = selectedProduct;

                int categoryId = -1;

                foreach(var category in MainWindow.categories)
                {
                    if(categoryComboBox.SelectedItem == category)
                    {
                        categoryId = category.id;
                        break;
                    }
                }

                if (categoryId == -1) return;

                newProduct.name = productNameTextBox.Text;
                newProduct.price_rsd = Convert.ToInt32(priceTextBox.Text);
                newProduct.price_on_sale = (salePriceTextBox.Text == "") ? null : Convert.ToInt32(salePriceTextBox.Text);
                newProduct.stock_quantity = Convert.ToInt32(quantityTextBox.Text);
                newProduct.description = descriptionTextBox.Text;
                newProduct.category_id = categoryId;
                newProduct.specifications = specifications;
                newProduct.specifications = getUpdatedSpecifications(specifications);

                HttpResponseMessage response = await MainWindow.client.PutAsJsonAsync(MainWindow.API_URL + "/products/", selectedProduct);
                response.EnsureSuccessStatusCode();
                await LoadProductsTable();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private async void productDeactivateBtn_Click(object sender, RoutedEventArgs e)
        {
            await setProductStatus(false);
            await LoadProductsTable();
        }

        private async void productActivateBtn_Click(object sender, RoutedEventArgs e)
        {
            await setProductStatus(true);
            await LoadProductsTable();
        }

        async Task setProductStatus(bool status)
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

                HttpResponseMessage response = await MainWindow.client.PatchAsync(MainWindow.API_URL + "/products/" + productId + "/status?isActive=" + status,
                    null
                    );
                response.EnsureSuccessStatusCode();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        async Task getProductSpecifications(int productId)
        {
            clearProductSpecifications();
            Product? p;

            try
            {
                HttpResponseMessage response = await MainWindow.client.GetAsync(MainWindow.API_URL + "/products/" + productId);
                p = await response.Content.ReadFromJsonAsync<Product>() ?? null;

                if (p == null) return;

                specifications = p.specifications;

                int row = 0;
                int labelColumn = 0;
                int textBoxColumn = 1;

                for(int i = 0; i < p.specifications.Count; i++)
                {
                    RowDefinition rowDefinition = new RowDefinition();
                    rowDefinition.Height = new GridLength(60);

                    SpecificationsGrid.RowDefinitions.Add(rowDefinition);
                }

                p.specifications.ForEach(specification =>
                {
                    Label label = new Label();

                    if (specification.name != null)
                        label.Content = t.TranslateToSerbian(specification.name.ToLower(), true);
                    label.Style = (Style)Application.Current.FindResource("TextBoxLabel");

                    labels.Add(label);
                    Grid.SetRow(label, row);
                    Grid.SetColumn(label, labelColumn);
                    SpecificationsGrid.Children.Add(label);

                    
                    TextBox textBox = new TextBox();
                    textBox.Style = (Style)Application.Current.FindResource("TextBox");
                    textBox.Text = specification.value;
                    textBoxes.Add(textBox);
                    Grid.SetRow(textBox, row);
                    Grid.SetColumn(textBox, textBoxColumn);
                    SpecificationsGrid.Children.Add(textBox);

                    row++; 
                });
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        List<ProductSpecification> getUpdatedSpecifications(List<ProductSpecification> specificationList)
        {
            int i = 0;
            specificationList.ForEach(spec =>
            {
                spec.value = textBoxes[i].Text;
                i++;
            });

            return specifications;
        }

        void clearProductSpecifications()
        {
            labels.ForEach((label) =>
            {
                SpecificationsGrid.Children.Remove(label);
            });

            textBoxes.ForEach((textBox) =>
            {
                SpecificationsGrid.Children.Remove(textBox);
            });

            labels.Clear();
            textBoxes.Clear();

            specifications.Clear();

            SpecificationsGrid.RowDefinitions.Clear();
        }

        void clearTextBoxes()
        {
            productNameTextBox.Clear();
            imageURLTextBox.Clear();
            priceTextBox.Clear();
            salePriceTextBox.Clear();
            quantityTextBox.Clear();
            descriptionTextBox.Clear();
        }
    }
}
