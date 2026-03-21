using Backend;
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
        public MainWindow()
        {
            InitializeComponent();
            LoadGrids();
        }

        List<Grid> grids = new List<Grid>();

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

        private void DostavaBtn_Click(object sender, RoutedEventArgs e)
        {
            showGrid(Dostava);
            CurrentGridLabel.Content = "Dostava";
        }
    }
}