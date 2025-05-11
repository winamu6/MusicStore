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
            OrderListView.ItemsSource = _orderService.GetAllOrders();
        }

        private void ConfirmOrder_Click(object sender, RoutedEventArgs e)
        {
            if (OrderListView.SelectedItem is Order selectedOrder)
            {
                if (selectedOrder.IsConfirmed)
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
            if (OrderListView.SelectedItem is Order selectedOrder)
            {
                if (selectedOrder.IsConfirmed)
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
    }
}
