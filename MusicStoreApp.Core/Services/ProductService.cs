using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MusicStoreApp.Core.Models;

namespace MusicStoreApp.Core.Services
{
    public class ProductService
    {
        private readonly List<Product> _products;

        public ProductService()
        {
            _products = new List<Product>
            {
                new Product { Id = 1, Name = "Album A", Artist = "Artist A", Price = 9.99m, Genre = "Rock" },
                new Product { Id = 2, Name = "Album B", Artist = "Artist B", Price = 14.99m, Genre = "Pop" },
                new Product { Id = 3, Name = "Album C", Artist = "Artist C", Price = 12.50m, Genre = "Jazz" }
            };
        }

        public List<Product> GetAllProducts() => _products;

        public List<Product> Search(string keyword)
        {
            return _products.Where(p =>
                p.Name.Contains(keyword, StringComparison.OrdinalIgnoreCase) ||
                p.Artist.Contains(keyword, StringComparison.OrdinalIgnoreCase) ||
                p.Genre.Contains(keyword, StringComparison.OrdinalIgnoreCase)).ToList();
        }

        public void AddProduct(Product product)
        {
            product.Id = _products.Max(p => p.Id) + 1;
            _products.Add(product);
        }

        public bool RemoveProduct(int id)
        {
            var product = _products.FirstOrDefault(p => p.Id == id);
            if (product != null)
            {
                _products.Remove(product);
                return true;
            }
            return false;
        }
    }
}
