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
        private readonly List<CartItem> _cartItems = new();

        public void AddToCart(Product product, int quantity = 1)
        {
            var existing = _cartItems.FirstOrDefault(ci => ci.Product.Id == product.Id);
            if (existing != null)
                existing.Quantity += quantity;
            else
                _cartItems.Add(new CartItem { Product = product, Quantity = quantity });
        }

        public void RemoveFromCart(int productId)
        {
            var item = _cartItems.FirstOrDefault(ci => ci.Product.Id == productId);
            if (item != null)
                _cartItems.Remove(item);
        }

        public List<CartItem> GetCartItems() => _cartItems;

        public void ClearCart() => _cartItems.Clear();

        public decimal GetTotalPrice() =>
            _cartItems.Sum(ci => ci.Product.Price * ci.Quantity);
    }
}
