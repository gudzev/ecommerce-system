using Backend.Models;
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
    /// Interaction logic for SpecificationsPage.xaml
    /// </summary>
    public partial class SpecificationsPage : Page
    {
        public SpecificationsPage()
        {
            InitializeComponent();

        }

        CategorySpecification selectedSpecification = new CategorySpecification();

        private async void Specifikacije_Loaded(object sender, RoutedEventArgs e)
        {
            await LoadCategoriesComboBox();

            Category category = (Category)categoryComboBox.Items[0];
            await LoadSpecificationsTable(category.id);
        }

        async Task LoadCategoriesComboBox()
        {
            await MainWindow.getCategories();
            categoryComboBox.ItemsSource = MainWindow.categories;
            categoryComboBox.DisplayMemberPath = "name";

            categoryComboBox.SelectedIndex = 0;
        }

        private void CategorySpecificationsTable_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedSpecification = (CategorySpecification)CategorySpecificationsTable.SelectedItem;

            if (selectedSpecification == null)
            {
                specificationNameTextBox.Text = "";
                return;
            }


            specificationNameTextBox.Text = selectedSpecification.name;
        }

        private async void categoryComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Category category = (Category)categoryComboBox.SelectedItem;
            await LoadSpecificationsTable(category.id);
        }

        private async Task LoadSpecificationsTable(int categoryId)
        {
            try
            {
                HttpResponseMessage response = await MainWindow.client.GetAsync("https://localhost:7097/category-specifications?categoryId=" + categoryId);
                response.EnsureSuccessStatusCode();
                CategorySpecificationsTable.ItemsSource = await response.Content.ReadFromJsonAsync<ObservableCollection<CategorySpecification>>();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
