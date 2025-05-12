using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MusicStoreApp.Core.Data;
using MusicStoreApp.Core.Models.Entities;

namespace MusicStoreApp.Core.Services
{
    public class ProductService
    {
        private readonly MusicStoreDbContext _context;

        public ProductService(MusicStoreDbContext context)
        {
            _context = context;
        }

        public List<Product> GetAllProducts()
        {
            return _context.Products.ToList();
        }

        public List<Product> Search(string keyword)
        {
            return _context.Products
                .Where(p =>
                    p.Name.ToLower().Contains(keyword.ToLower()) ||
                    p.Artist.ToLower().Contains(keyword.ToLower()) ||
                    p.Genre.ToLower().Contains(keyword.ToLower()))
                .ToList();
        }

        public void AddProduct(Product product)
        {
            _context.Products.Add(product);
            _context.SaveChanges();
        }

        public bool RemoveProduct(int id)
        {
            var product = _context.Products.FirstOrDefault(p => p.Id == id);
            if (product != null)
            {
                _context.Products.Remove(product);
                _context.SaveChanges();
                return true;
            }
            return false;
        }
    }
}