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
    public class ProductServiceTests
    {
        private MusicStoreDbContext _context;
        private ProductService _productService;

        [TestInitialize]
        public void Setup()
        {
            var dbName = $"TestDb_{Guid.NewGuid()}";
            var options = new DbContextOptionsBuilder<MusicStoreDbContext>()
                .UseInMemoryDatabase(databaseName: dbName)
                .Options;

            _context = new MusicStoreDbContext(options);
            _context.Database.EnsureCreated();

            _productService = new ProductService(_context);
        }

        private Product CreateSampleProduct(int id = 1, string name = "Product1", string artist = "Artist1", string genre = "Genre1", decimal price = 10.0m)
        {
            return new Product
            {
                Id = id,
                Name = name,
                Artist = artist,
                Genre = genre,
                Price = price
            };
        }

        [TestMethod]
        public void GetAllProducts_ShouldReturnAllProducts()
        {
            var product1 = CreateSampleProduct(1);
            _context.Products.AddRange(product1);
            _context.SaveChanges();

            var products = _productService.GetAllProducts();

            products = products.OrderBy(p => p.Id).ToList();

            Assert.AreEqual("Product1", products[0].Name);
        }


        [TestMethod]
        public void Search_ShouldReturnProductsMatchingKeyword()
        {
            var product1 = CreateSampleProduct(1, "Rock Album", "Artist1", "Rock", 15.0m);
            var product2 = CreateSampleProduct(2, "Pop Album", "Artist2", "Pop", 20.0m);
            _context.Products.AddRange(product1, product2);
            _context.SaveChanges();

            var searchResults = _productService.Search("Rock");

            Assert.AreEqual(1, searchResults.Count);
            Assert.AreEqual("Rock Album", searchResults[0].Name);
        }

        [TestMethod]
        public void AddProduct_ShouldAddProduct()
        {
            var product = CreateSampleProduct();
            _productService.AddProduct(product);

            var addedProduct = _context.Products.FirstOrDefault(p => p.Name == "Product1");

            Assert.IsNotNull(addedProduct);
            Assert.AreEqual("Product1", addedProduct.Name);
        }

        [TestMethod]
        public void RemoveProduct_ShouldRemoveProduct_WhenProductExists()
        {
            var product = CreateSampleProduct();
            _context.Products.Add(product);
            _context.SaveChanges();

            var result = _productService.RemoveProduct(product.Id);

            Assert.IsTrue(result);
            Assert.IsNull(_context.Products.FirstOrDefault(p => p.Id == product.Id));
        }

        [TestMethod]
        public void RemoveProduct_ShouldReturnFalse_WhenProductDoesNotExist()
        {
            var result = _productService.RemoveProduct(999);

            Assert.IsFalse(result);
        }
    }
}
