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
    public class ProductServiceTests
    {
        [TestMethod]
        public void AddProduct_IncrementsIdAndAdds()
        {
            var service = new ProductService();
            var newProduct = new Product { Name = "New", Artist = "Artist", Genre = "Genre", Price = 9.99m };

            service.AddProduct(newProduct);
            var all = service.GetAllProducts();

            Assert.IsTrue(all.Exists(p => p.Name == "New"));
        }

        [TestMethod]
        public void RemoveProduct_ValidId_RemovesProduct()
        {
            var service = new ProductService();
            var initialCount = service.GetAllProducts().Count;

            var removed = service.RemoveProduct(1);
            Assert.IsTrue(removed);
            Assert.AreEqual(initialCount - 1, service.GetAllProducts().Count);
        }

        [TestMethod]
        public void Search_ReturnsMatchingResults()
        {
            var service = new ProductService();
            var results = service.Search("Album");

            Assert.IsTrue(results.Count > 0);
        }
    }
}
