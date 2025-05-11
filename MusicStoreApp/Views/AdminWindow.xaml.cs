using Microsoft.Extensions.DependencyInjection;
using MusicStoreApp.Core.Models;
using MusicStoreApp.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace MusicStoreApp.Views
{
    public partial class AdminWindow : Window
    {
        private readonly ProductService _productService;

        public AdminWindow()
        {
            InitializeComponent();
            _productService = App.ServiceProvider.GetRequiredService<ProductService>();
            LoadProducts();
        }

        private void LoadProducts()
        {
            ProductListView.ItemsSource = null;
            ProductListView.ItemsSource = _productService.GetAllProducts();
        }

        private void AddProduct_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(NameTextBox.Text) ||
                string.IsNullOrWhiteSpace(ArtistTextBox.Text) ||
                string.IsNullOrWhiteSpace(GenreTextBox.Text) ||
                string.IsNullOrWhiteSpace(PriceTextBox.Text))
            {
                MessageBox.Show("Заполните все поля.");
                return;
            }

            if (!decimal.TryParse(PriceTextBox.Text, out decimal price))
            {
                MessageBox.Show("Неверный формат цены.");
                return;
            }

            var newProduct = new Product
            {
                Name = NameTextBox.Text,
                Artist = ArtistTextBox.Text,
                Genre = GenreTextBox.Text,
                Price = price
            };

            _productService.AddProduct(newProduct);
            LoadProducts();

            NameTextBox.Text = "";
            ArtistTextBox.Text = "";
            GenreTextBox.Text = "";
            PriceTextBox.Text = "";
        }

        private void DeleteProduct_Click(object sender, RoutedEventArgs e)
        {
            if (ProductListView.SelectedItem is Product selectedProduct)
            {
                var result = MessageBox.Show($"Удалить товар \"{selectedProduct.Name}\"?", "Подтверждение", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                {
                    _productService.RemoveProduct(selectedProduct.Id);
                    LoadProducts();
                }
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите товар для удаления.");
            }
        }

        private void Logout_Click(object sender, RoutedEventArgs e)
        {
            var loginWindow = new MainWindow();
            loginWindow.Show();
            this.Close();
        }
    }
}
