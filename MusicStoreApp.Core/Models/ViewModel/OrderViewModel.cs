using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicStoreApp.Core.Models.ViewModel
{
    public class OrderViewModel
    {
        public int Id { get; set; }
        public string CustomerName { get; set; }
        public string IsConfirmed { get; set; }
        public decimal TotalPrice { get; set; }
        public int ProductsCount { get; set; }
    }

}
