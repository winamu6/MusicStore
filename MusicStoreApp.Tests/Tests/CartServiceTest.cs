using MusicStoreApp.Core.Models;
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
        [TestMethod]
        public void AddToCart_NewProduct_AddsItem()
        {
            var cart = new CartService();
            var product = new Product { Id = 1, Name = "Test", Price = 10m };

            cart.AddToCart(product, 2);
            var items = cart.GetCartItems();

            Assert.AreEqual(1, items.Count);
            Assert.AreEqual(2, items[0].Quantity);
        }

        [TestMethod]
        public void RemoveFromCart_RemovesCorrectItem()
        {
            var cart = new CartService();
            var product = new Product { Id = 1, Name = "Test", Price = 10m };

            cart.AddToCart(product);
            cart.RemoveFromCart(1);

            Assert.AreEqual(0, cart.GetCartItems().Count);
        }

        [TestMethod]
        public void GetTotalPrice_ReturnsCorrectSum()
        {
            var cart = new CartService();
            cart.AddToCart(new Product { Id = 1, Price = 10m }, 2);
            cart.AddToCart(new Product { Id = 2, Price = 5m }, 1);

            var total = cart.GetTotalPrice();
            Assert.AreEqual(25m, total);
        }
    }
}
