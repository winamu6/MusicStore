using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicStoreApp.Core.Models.Entities
{
    public class Order
    {
        [Key]
        public int Id { get; set; }
        public string CustomerName { get; set; }
        public bool IsConfirmed { get; set; }

        public List<Product> Products { get; set; } = new();

        [NotMapped]
        public decimal TotalPrice => Products.Sum(p => p.Price);
    }

}
