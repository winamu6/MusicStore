using Microsoft.EntityFrameworkCore;
using MusicStoreApp.Core.Data;
using MusicStoreApp.Core.Models.Entities;
using MusicStoreApp.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicStoreApp.Tests.Tests
{
    [TestClass]
    public class OrderServiceTests
    {
        private MusicStoreDbContext _context;
        private OrderService _orderService;

        [TestInitialize]
        public void Setup()
        {
            var dbName = $"TestDb_{Guid.NewGuid()}";
            var options = new DbContextOptionsBuilder<MusicStoreDbContext>()
                .UseInMemoryDatabase(databaseName: dbName)
                .Options;

            _context = new MusicStoreDbContext(options);
            _context.Database.EnsureCreated();

            _orderService = new OrderService(_context);
        }

        private Product CreateSampleProduct(int id = 1, string name = "Product1", decimal price = 10.0m)
        {
            return new Product
            {
                Id = id,
                Name = name,
                Artist = "Artist1",
                Genre = "Genre1",
                Price = price
            };
        }

        private Order CreateSampleOrder(List<Product> products = null, string customerName = "Customer")
        {
            return new Order
            {
                CustomerName = customerName,
                IsConfirmed = false,
                Products = products ?? new List<Product>()
            };
        }

        [TestMethod]
        public void AddOrder_ShouldAddNewOrder()
        {
            var product1 = CreateSampleProduct(1);
            var product2 = CreateSampleProduct(2);
            _context.Products.AddRange(product1, product2);
            _context.SaveChanges();

            var order = CreateSampleOrder(new List<Product> { product1, product2 });

            _orderService.AddOrder(order);

            var addedOrder = _context.Orders.Include(o => o.Products).FirstOrDefault(o => o.CustomerName == "Customer");

            Assert.IsNotNull(addedOrder);
            Assert.AreEqual(2, addedOrder.Products.Count);
            Assert.AreEqual("Customer", addedOrder.CustomerName);
        }

        [TestMethod]
        public void GetAllOrders_ShouldReturnAllOrders()
        {
            var product1 = CreateSampleProduct(1);
            var product2 = CreateSampleProduct(2);
            _context.Products.AddRange(product1, product2);
            _context.SaveChanges();

            var order1 = CreateSampleOrder(new List<Product> { product1 });
            var order2 = CreateSampleOrder(new List<Product> { product2 });
            _context.Orders.AddRange(order1, order2);
            _context.SaveChanges();

            var orders = _orderService.GetAllOrders();

            Assert.AreEqual(2, orders.Count);
            Assert.AreEqual(1, orders[0].ProductsCount);
            Assert.AreEqual(1, orders[1].ProductsCount);
        }

        [TestMethod]
        public void ConfirmOrder_ShouldConfirmOrder()
        {
            var product1 = CreateSampleProduct(1);
            _context.Products.Add(product1);
            _context.SaveChanges();

            var order = CreateSampleOrder(new List<Product> { product1 });
            _orderService.AddOrder(order);

            var addedOrder = _context.Orders.Include(o => o.Products).FirstOrDefault(o => o.CustomerName == "Customer");
            Assert.IsNotNull(addedOrder);
            Assert.IsFalse(addedOrder.IsConfirmed);

            _orderService.ConfirmOrder(addedOrder.Id);

            var confirmedOrder = _context.Orders.FirstOrDefault(o => o.Id == addedOrder.Id);
            Assert.IsTrue(confirmedOrder.IsConfirmed);
        }

        [TestMethod]
        public void CancelOrder_ShouldCancelOrder()
        {
            var product1 = CreateSampleProduct(1);
            _context.Products.Add(product1);
            _context.SaveChanges();

            var order = CreateSampleOrder(new List<Product> { product1 });
            _orderService.AddOrder(order);

            var addedOrder = _context.Orders.Include(o => o.Products).FirstOrDefault(o => o.CustomerName == "Customer");
            Assert.IsNotNull(addedOrder);
            Assert.IsFalse(addedOrder.IsConfirmed);

            _orderService.CancelOrder(addedOrder.Id);

            var cancelledOrder = _context.Orders.FirstOrDefault(o => o.Id == addedOrder.Id);
            Assert.IsFalse(cancelledOrder.IsConfirmed);
        }

        [TestMethod]
        public void CancelOrder_ShouldNotCancelNonExistentOrder()
        {
            try
            {
                _orderService.CancelOrder(999);
            }
            catch (Exception)
            {
                Assert.Fail("Expected no exception, but an exception was thrown.");
            }
        }
    }
}
