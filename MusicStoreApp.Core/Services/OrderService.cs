using Microsoft.EntityFrameworkCore;
using MusicStoreApp.Core.Data;
using MusicStoreApp.Core.Models.Entities;
using MusicStoreApp.Core.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicStoreApp.Core.Services
{
    public class OrderService
    {
        private readonly MusicStoreDbContext _context;

        public OrderService(MusicStoreDbContext context)
        {
            _context = context;
        }

        public void AddOrder(Order order)
        {
            var productIds = order.Products.Select(p => p.Id).ToList();

            var existingProducts = _context.Products
                                           .Where(p => productIds.Contains(p.Id))
                                           .ToList();

            var newOrder = new Order
            {
                CustomerName = order.CustomerName,
                IsConfirmed = order.IsConfirmed,
                Products = new List<Product>()
            };

            foreach (var product in existingProducts)
            {
                newOrder.Products.Add(product);
            }

            _context.Orders.Add(newOrder);
            _context.SaveChanges();
        }

        public List<OrderViewModel> GetAllOrders()
        {
            var orders = _context.Orders
                                 .Include(o => o.Products)
                                 .ToList();

            var orderViewModels = orders.Select(order => new OrderViewModel
            {
                Id = order.Id,
                CustomerName = order.CustomerName,
                TotalPrice = order.Products.Sum(p => p.Price),
                IsConfirmed = order.IsConfirmed ? "Подтвержден" : "Не подтвержден",
                ProductsCount = order.Products.Count
            }).ToList();

            return orderViewModels;
        }

        public void ConfirmOrder(int id)
        {
            var order = _context.Orders.FirstOrDefault(o => o.Id == id);
            if (order != null)
            {
                order.IsConfirmed = true;
                _context.SaveChanges();
            }
        }

        public void CancelOrder(int id)
        {
            var order = _context.Orders.FirstOrDefault(o => o.Id == id);
            if (order != null)
            {
                order.IsConfirmed = false;
                _context.SaveChanges();
            }
        }
    }
}
