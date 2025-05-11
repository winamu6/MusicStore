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
    public class OrderServiceTests
    {
        [TestMethod]
        public void AddOrder_AssignsIdAndAdds()
        {
            var service = new OrderService();
            var order = new Order { CustomerName = "John", Products = new List<Product>() };

            service.AddOrder(order);
            var all = service.GetAllOrders();

            Assert.AreEqual(1, all.Count);
            Assert.AreEqual(1, all[0].Id);
        }

        [TestMethod]
        public void ConfirmOrder_SetsIsConfirmed()
        {
            var service = new OrderService();
            var order = new Order { CustomerName = "Jane" };
            service.AddOrder(order);

            service.ConfirmOrder(1);
            Assert.IsTrue(service.GetAllOrders()[0].IsConfirmed);
        }

        [TestMethod]
        public void CancelOrder_SetsIsConfirmedFalse()
        {
            var service = new OrderService();
            var order = new Order { CustomerName = "Jake", IsConfirmed = true };
            service.AddOrder(order);

            service.CancelOrder(1);
            Assert.IsFalse(service.GetAllOrders()[0].IsConfirmed);
        }
    }
}
