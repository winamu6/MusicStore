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
    public partial class CustomerWindow : Window
    {
        private readonly ProductService _productService;
        private readonly OrderService _orderService;
        private readonly CartService _cartService = new();
        private List<Product> _allProducts = new();

        public CustomerWindow()
        {
            InitializeComponent();

            _productService = App.ServiceProvider.GetRequiredService<ProductService>();
            _orderService = App.ServiceProvider.GetRequiredService<OrderService>();

            Loaded += CustomerWindow_Loaded;
        }

        private void CustomerWindow_Loaded(object sender, RoutedEventArgs e)
        {
            LoadProducts();
            ApplyFilters();
        }

        private void LoadProducts()
        {
            _allProducts = _productService.GetAllProducts();
        }

        private void ApplyFilters()
        {
            if (ProductListView == null) return;

            var filteredProducts = _allProducts;

            if (!string.IsNullOrWhiteSpace(SearchBox?.Text))
            {
                filteredProducts = filteredProducts.Where(p =>
                    p.Name.Contains(SearchBox.Text, StringComparison.OrdinalIgnoreCase) ||
                    p.Artist.Contains(SearchBox.Text, StringComparison.OrdinalIgnoreCase)).ToList();
            }

            if (GenreComboBox?.SelectedItem is ComboBoxItem selectedGenre &&
                selectedGenre.Content.ToString() != "Все жанры")
            {
                filteredProducts = filteredProducts
                    .Where(p => p.Genre == selectedGenre.Content.ToString())
                    .ToList();
            }

            if (SortAsc?.IsChecked == true)
            {
                filteredProducts = filteredProducts.OrderBy(p => p.Price).ToList();
            }
            else if (SortDesc?.IsChecked == true)
            {
                filteredProducts = filteredProducts.OrderByDescending(p => p.Price).ToList();
            }

            ProductListView.ItemsSource = filteredProducts;
        }

        private void SearchBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            ApplyFilters();
        }

        private void FilterByGenre(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            ApplyFilters();
        }

        private void AddToCart_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.Tag is int productId)
            {
                var product = _allProducts.FirstOrDefault(p => p.Id == productId);
                if (product != null)
                {
                    _cartService.AddToCart(product);
                    UpdateTotalPrice();
                }
            }
        }

        private void UpdateTotalPrice()
        {
            TotalPriceText.Text = $"Общая сумма: {_cartService.GetTotalPrice()} руб.";
        }

        private void ClearCart_Click(object sender, RoutedEventArgs e)
        {
            _cartService.ClearCart();
            UpdateTotalPrice();
        }

        private void Checkout_Click(object sender, RoutedEventArgs e)
        {
            var cartItems = _cartService.GetCartItems();
            if (cartItems.Count == 0)
            {
                MessageBox.Show("Корзина пуста. Пожалуйста, добавьте товары.");
                return;
            }

            var order = new Order
            {
                CustomerName = "Покупатель",
                Products = cartItems.Select(ci => ci.Product).ToList(),
                IsConfirmed = false
            };

            _orderService.AddOrder(order);
            MessageBox.Show("Ваш заказ отправлен на подтверждение менеджером!");
            _cartService.ClearCart();
            UpdateTotalPrice();
        }

        private void Logout_Click(object sender, RoutedEventArgs e)
        {
            var loginWindow = new MainWindow();
            loginWindow.Show();
            this.Close();
        }

        private void SortOrderChanged(object sender, RoutedEventArgs e)
        {
            ApplyFilters();
        }
    }
}
