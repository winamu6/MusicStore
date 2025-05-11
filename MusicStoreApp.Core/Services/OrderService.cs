using MusicStoreApp.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicStoreApp.Core.Services
{
    public class OrderService
    {
        private readonly List<Order> _orders = new();
        private int _nextId = 1;

        public void AddOrder(Order order)
        {
            order.Id = _nextId++;
            _orders.Add(order);
        }

        public List<Order> GetAllOrders() => _orders;

        public void ConfirmOrder(int id)
        {
            var order = _orders.FirstOrDefault(o => o.Id == id);
            if (order != null)
                order.IsConfirmed = true;
        }

        public void CancelOrder(int id)
        {
            var order = _orders.FirstOrDefault(o => o.Id == id);
            if (order != null)
                order.IsConfirmed = false;
        }
    }
}
