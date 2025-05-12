using MusicStoreApp.Core.Models.Entities;
using MusicStoreApp.Core.Models.ViewModel;
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
    public partial class ManagerWindow : Window
    {
        private readonly OrderService _orderService;

        public ManagerWindow(OrderService orderService)
        {
            InitializeComponent();
            _orderService = orderService;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            LoadOrders();
        }

        private void LoadOrders()
        {
            OrderListView.ItemsSource = null;
            
            var orders = _orderService.GetAllOrders();

            OrderListView.ItemsSource = orders;
        }


        private void ConfirmOrder_Click(object sender, RoutedEventArgs e)
        {
            if (OrderListView.SelectedItem is OrderViewModel selectedOrder)
            {
                if (selectedOrder.IsConfirmed == "Подтвержден")
                {
                    MessageBox.Show("Заказ уже подтвержден.");
                    return;
                }

                _orderService.ConfirmOrder(selectedOrder.Id);
                LoadOrders();
                MessageBox.Show($"Заказ #{selectedOrder.Id} подтверждён.");
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите заказ.");
            }
        }

        private void CancelOrder_Click(object sender, RoutedEventArgs e)
        {
            if (OrderListView.SelectedItem is OrderViewModel selectedOrder)
            {
                if (selectedOrder.IsConfirmed == "Подтвержден")
                {
                    MessageBox.Show("Невозможно отменить подтвержденный заказ.");
                    return;
                }

                _orderService.CancelOrder(selectedOrder.Id);
                LoadOrders();
                MessageBox.Show($"Заказ #{selectedOrder.Id} отменён.");
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите заказ.");
            }
        }


        private void Logout_Click(object sender, RoutedEventArgs e)
        {
            var loginWindow = new MainWindow();
            loginWindow.Show();
            this.Close();
        }

        private void OrderListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (OrderListView.SelectedItem is OrderViewModel selectedOrder)
            {
                var details = $"Заказ #{selectedOrder.Id} - Покупатель: {selectedOrder.CustomerName}, " +
                              $"Товаров: {selectedOrder.ProductsCount}, " +
                              $"Сумма: {selectedOrder.TotalPrice}, " +
                              $"Статус: {selectedOrder.IsConfirmed}";
                OrderDetailsTextBlock.Text = details;
            }
            else
            {
                OrderDetailsTextBlock.Text = "Выберите заказ для просмотра деталей";
            }
        }

    }
}
