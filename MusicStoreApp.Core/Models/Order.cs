using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicStoreApp.Core.Models
{
    public class Order
    {
        public int Id { get; set; }
        public string CustomerName { get; set; }
        public List<Product> Products { get; set; }
        public decimal TotalPrice => Products.Sum(p => p.Price);
        public bool IsConfirmed { get; set; }
    }

}
