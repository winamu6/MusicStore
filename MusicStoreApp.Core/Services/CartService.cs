using Microsoft.EntityFrameworkCore;
using MusicStoreApp.Core.Data;
using MusicStoreApp.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicStoreApp.Core.Services
{
    public class CartService
    {
        private readonly MusicStoreDbContext _context;

        public CartService(MusicStoreDbContext context)
        {
            _context = context;
        }

        public void AddToCart(Product product, int quantity = 1)
        {
            var existing = _context.CartItems
                .Include(ci => ci.Product)
                .FirstOrDefault(ci => ci.Product.Id == product.Id);

            if (existing != null)
            {
                existing.Quantity += quantity;
                _context.CartItems.Update(existing);
            }
            else
            {
                _context.CartItems.Add(new CartItem
                {
                    Product = product,
                    Quantity = quantity
                });
            }

            _context.SaveChanges();
        }

        public void RemoveFromCart(int productId)
        {
            var item = _context.CartItems
                .Include(ci => ci.Product)
                .FirstOrDefault(ci => ci.Product.Id == productId);

            if (item != null)
            {
                _context.CartItems.Remove(item);
                _context.SaveChanges();
            }
        }

        public List<CartItem> GetCartItems()
        {
            return _context.CartItems
                .Include(ci => ci.Product)
                .ToList();
        }

        public void ClearCart()
        {
            var items = _context.CartItems.ToList();
            _context.CartItems.RemoveRange(items);
            _context.SaveChanges();
        }

        public decimal GetTotalPrice()
        {
            return _context.CartItems
                .Include(ci => ci.Product)
                .ToList()
                .Sum(ci => ci.Product.Price * ci.Quantity);
        }
    }
}
