using Microsoft.EntityFrameworkCore;
using MusicStoreApp.Core.Data;
using MusicStoreApp.Core.Models;
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
    public class CartServiceTests
    {
        private MusicStoreDbContext _context;
        private CartService _cartService;

        [TestInitialize]
        public void Setup()
        {
            var dbName = $"TestDb_{Guid.NewGuid()}"; // Уникальная база на каждый тест
            var options = new DbContextOptionsBuilder<MusicStoreDbContext>()
                .UseInMemoryDatabase(databaseName: dbName)
                .Options;

            _context = new MusicStoreDbContext(options);
            _context.Database.EnsureCreated();

            _cartService = new CartService(_context);
        }

        // Метод для создания продуктов с обязательными полями
        private Product CreateSampleProduct(int id = 1, decimal price = 10.0m, string artist = "Artist", string genre = "Genre")
        {
            return new Product
            {
                Id = id,
                Name = $"Product{id}",
                Artist = artist, // добавлено обязательное поле
                Price = price,
                Genre = genre // добавлено обязательное поле
            };
        }

        [TestMethod]
        public void AddToCart_ShouldAddNewItem()
        {
            var product = CreateSampleProduct();
            _context.Products.Add(product);
            _context.SaveChanges();

            _cartService.AddToCart(product, 2);

            var items = _cartService.GetCartItems();
            Assert.AreEqual(1, items.Count);
            Assert.AreEqual(2, items[0].Quantity);
            Assert.AreEqual(product.Id, items[0].Product.Id);
        }

        [TestMethod]
        public void AddToCart_ShouldIncreaseQuantity_WhenItemExists()
        {
            var product = CreateSampleProduct();
            _context.Products.Add(product);
            _context.SaveChanges();

            _cartService.AddToCart(product, 1);
            _cartService.AddToCart(product, 3);

            var item = _cartService.GetCartItems().First();
            Assert.AreEqual(4, item.Quantity);
        }

        [TestMethod]
        public void RemoveFromCart_ShouldRemoveItem()
        {
            var product = CreateSampleProduct();
            _context.Products.Add(product);
            _context.SaveChanges();

            _cartService.AddToCart(product, 1);
            _cartService.RemoveFromCart(product.Id);

            var items = _cartService.GetCartItems();
            Assert.AreEqual(0, items.Count);
        }

        [TestMethod]
        public void ClearCart_ShouldRemoveAllItems()
        {
            var product1 = CreateSampleProduct(1);
            var product2 = CreateSampleProduct(2);
            _context.Products.AddRange(product1, product2);
            _context.SaveChanges();

            _cartService.AddToCart(product1, 1);
            _cartService.AddToCart(product2, 1);

            _cartService.ClearCart();

            var items = _cartService.GetCartItems();
            Assert.AreEqual(0, items.Count);
        }

        [TestMethod]
        public void GetTotalPrice_ShouldReturnCorrectSum()
        {
            var product1 = CreateSampleProduct(1, 10);
            var product2 = CreateSampleProduct(2, 15);
            _context.Products.AddRange(product1, product2);
            _context.SaveChanges();

            _cartService.AddToCart(product1, 2); // 20
            _cartService.AddToCart(product2, 1); // 15

            var total = _cartService.GetTotalPrice();
            Assert.AreEqual(35, total);
        }
    }
}