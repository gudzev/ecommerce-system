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
    public partial class CategoriesPage : Page
    {
        public CategoriesPage()
        {
            InitializeComponent();
        }

        private async void CategoriesPageLoaded(object sender, RoutedEventArgs e)
        {
            await LoadCategoriesTable();
            LoadFirstCategoriesRow();
        }

        public static async Task getCategories()
        {
            if (!MainWindow.shouldRefreshCategories && MainWindow.categories.Count > 0)
            {
                return;
            }

            MainWindow.shouldRefreshCategories = false;

            try
            {
                HttpResponseMessage response = await MainWindow.client.GetAsync("https://localhost:7097/categories");
                response.EnsureSuccessStatusCode();
                MainWindow.categories = await response.Content.ReadFromJsonAsync<ObservableCollection<Category>>();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private async Task LoadCategoriesTable()
        {
            try
            {
                await getCategories();
                CategoriesTable.ItemsSource = MainWindow.categories;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
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

            MainWindow.shouldRefreshCategories = true;

            try
            {
                HttpResponseMessage response = await MainWindow.client.PostAsJsonAsync("https://localhost:7097/categories", newCategory);
                response.EnsureSuccessStatusCode();
                await LoadCategoriesTable();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private async void updateCategoryBtn_Click(object sender, RoutedEventArgs e)
        {
            if (categoryNameTextBox.Text == "") return;

            Category existingCategory = (Category)CategoriesTable.SelectedItem;
            existingCategory.name = categoryNameTextBox.Text;

            MainWindow.shouldRefreshCategories = true;

            try
            {
                HttpResponseMessage response = await MainWindow.client.PutAsJsonAsync("https://localhost:7097/categories", existingCategory);
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

            MainWindow.shouldRefreshCategories = true;

            try
            {
                HttpResponseMessage response = await MainWindow.client.DeleteAsync("https://localhost:7097/categories/" + selectedCategory.id);
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
