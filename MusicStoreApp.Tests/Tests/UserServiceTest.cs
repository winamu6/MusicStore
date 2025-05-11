using MusicStoreApp.Core.Models.MusicStoreApp.Core.Models;
using MusicStoreApp.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicStoreApp.Tests.Tests
{
    [TestClass]
    public class UserServiceTests
    {
        [TestMethod]
        public void Authenticate_ValidCredentials_ReturnsUser()
        {
            var service = new UserService();
            var user = service.Authenticate("admin", "admin");

            Assert.IsNotNull(user);
            Assert.AreEqual(UserRole.Admin, user.Role);
        }

        [TestMethod]
        public void Authenticate_InvalidCredentials_ReturnsNull()
        {
            var service = new UserService();
            var user = service.Authenticate("admin", "wrongpass");

            Assert.IsNull(user);
        }

        [TestMethod]
        public void Register_NewUser_AddsSuccessfully()
        {
            var service = new UserService();
            var result = service.Register("newuser", "pass");

            Assert.IsTrue(result);
            Assert.IsNotNull(service.Authenticate("newuser", "pass"));
        }

        [TestMethod]
        public void Register_ExistingUsername_Fails()
        {
            var service = new UserService();
            var result = service.Register("admin", "newpass");

            Assert.IsFalse(result);
        }
    }
}
