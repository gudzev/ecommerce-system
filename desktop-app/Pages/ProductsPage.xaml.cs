using Backend.Models;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Net.Http.Json;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using WebStoreManagementApp;
using Image = Backend.Models.Image;

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
        List<Image> images = new List<Image>();

        Translator t = new Translator();

        bool isLoadingProduct = false;

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
            if (isLoadingProduct) return;

            clearProductDetails();

            var selectedCategory = (Category)categoryComboBox.SelectedItem;

            HttpResponseMessage response = await MainWindow.client.GetAsync(MainWindow.API_URL + "/category-specifications?categoryId=" + selectedCategory.id);
            response.EnsureSuccessStatusCode();

            var categorySpecifications = await response.Content.ReadFromJsonAsync<List<CategorySpecification>>() ?? [];

            if (categorySpecifications.Count <= 0) return;

            for (int i = 0; i < categorySpecifications.Count; i++)
            {
                RowDefinition rowDefinition = new RowDefinition();
                rowDefinition.Height = new GridLength(60);

                SpecificationsGrid.RowDefinitions.Add(rowDefinition);
            }

            int labelColumn = 0;
            int textBoxColumn = 1;
            int row = addImages(selectedProduct, 0, labelColumn, textBoxColumn);

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

            isLoadingProduct = true;

            try
            {
                Product product = (Product)ProductsTable.SelectedItem;
                selectedProduct = await MainWindow.client.GetFromJsonAsync<Product>(MainWindow.API_URL + "/products/" + product.id) ?? product;

                if (selectedProduct == null) return;

                clearTextBoxes();
                productNameTextBox.Text = selectedProduct.name;
                priceTextBox.Text = selectedProduct.price_rsd.ToString();
                salePriceTextBox.Text = selectedProduct.price_on_sale.ToString() ?? "";
                quantityTextBox.Text = selectedProduct.stock_quantity.ToString();
                descriptionTextBox.Text = selectedProduct?.description?.ToString();

                foreach (var category in MainWindow.categories)
                {
                    if (category.id == selectedProduct?.category_id)
                    {
                        categoryComboBox.SelectedItem = category;
                        await getProductDetails(product.id);
                        break;
                    }
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                isLoadingProduct = false;
            }
        }

        private async Task LoadFirstProductsRow()
        {
            try
            {
                if (ProductsTable.Items.Count == 0) return;

                isLoadingProduct = true;

                Product? product = (Product)ProductsTable.Items[0] ?? null;
                selectedProduct = await MainWindow.client.GetFromJsonAsync<Product>(MainWindow.API_URL + "/products/" + product.id) ?? product;

                if (selectedProduct == null) return;

                productNameTextBox.Text = product.name;
                priceTextBox.Text = product.price_rsd.ToString();
                salePriceTextBox.Text = product.price_on_sale.ToString() ?? "";
                quantityTextBox.Text = product.stock_quantity.ToString();
                descriptionTextBox.Text = product?.description?.ToString();

                foreach (var category in MainWindow.categories)
                {
                    if (category.id == product?.category_id)
                    {
                        categoryComboBox.SelectedItem = category;
                        await getProductDetails(product.id);
                        return;
                    }
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                isLoadingProduct = false;
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
            newProduct.price_rsd = Convert.ToInt32(priceTextBox.Text);
            newProduct.price_on_sale = (salePriceTextBox.Text == "") ? null : Convert.ToInt32(salePriceTextBox.Text);
            newProduct.category_id = categoryId;
            newProduct.stock_quantity = Convert.ToInt32(quantityTextBox.Text);
            newProduct.description = descriptionTextBox.Text;
            newProduct.specifications = getUpdatedSpecifications(specifications);
            newProduct.images = getUpdatedImages(images);

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
                newProduct.specifications = getUpdatedSpecifications(specifications);
                newProduct.images = getUpdatedImages(images);

                foreach (var image in newProduct.images)
                {
                    MessageBox.Show($"URL: '{image.url}'");
                }

                HttpResponseMessage response = await MainWindow.client.PutAsJsonAsync(MainWindow.API_URL + "/products/", newProduct);
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

        async Task getProductDetails(int productId)
        {
            clearProductDetails();
            Product? p;

            try
            {
                HttpResponseMessage response = await MainWindow.client.GetAsync(MainWindow.API_URL + "/products/" + productId);
                p = await response.Content.ReadFromJsonAsync<Product>() ?? null;

                if (p == null) return;

                specifications = p.specifications;

                for(int i = 0; i < (p.specifications.Count + p.images.Count); i++)
                {
                    RowDefinition rowDefinition = new RowDefinition();
                    rowDefinition.Height = new GridLength(60);

                    SpecificationsGrid.RowDefinitions.Add(rowDefinition);
                }

                int labelColumn = 0;
                int textBoxColumn = 1;
                int row = addImages(p, 0, labelColumn, textBoxColumn);

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

        int addImages(Product p, int row, int labelColumn, int textBoxColumn)
        {
            int? imageNumber = 1;
            p.images.ForEach((image) =>
            {
                if (imageNumber == 1 && image.is_main_image == false)
                    image.is_main_image = true;

                images = p.images;

                Label label = new Label();

                label.Content = (imageNumber == 1) ? "Slika 1 (Glavna)" : "Slika" + ' ' + imageNumber;
                label.Style = (Style)Application.Current.FindResource("TextBoxLabel");

                labels.Add(label);
                Grid.SetRow(label, row);
                Grid.SetColumn(label, labelColumn);
                SpecificationsGrid.Children.Add(label);


                TextBox textBox = new TextBox();
                textBox.Style = (Style)Application.Current.FindResource("TextBox");
                textBox.Text = image.url;
                textBoxes.Add(textBox);
                Grid.SetRow(textBox, row);
                Grid.SetColumn(textBox, textBoxColumn);
                SpecificationsGrid.Children.Add(textBox);

                row++;
                imageNumber++;
            });

            return row;
        }

        List<Image> getUpdatedImages(List<Image> imageList)
        {
            List<Image> newImages = new List<Image>();
            for(int i = 0; i < imageList.Count; i++)
            {
                imageList[i].url = textBoxes[i].Text.Trim();

                if (imageList[i].url.Length > 0)
                {
                    newImages.Add(imageList[i]);
                }
            };

            return newImages;
        }

        List<ProductSpecification> getUpdatedSpecifications(List<ProductSpecification> specificationList)
        {
            int i = selectedProduct.images.Count;
            specificationList.ForEach(spec =>
            {
                spec.value = textBoxes[i].Text;
                i++;
            });

            return specifications;
        }

        void clearProductDetails()
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
            images.Clear();

            SpecificationsGrid.RowDefinitions.Clear();
        }

        void clearTextBoxes()
        {
            productNameTextBox.Clear();
            priceTextBox.Clear();
            salePriceTextBox.Clear();
            quantityTextBox.Clear();
            descriptionTextBox.Clear();
        }
    }
}
