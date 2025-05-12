using Microsoft.EntityFrameworkCore;
using MusicStoreApp.Core.Data;
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
    public class UserServiceTests
    {
        private MusicStoreDbContext _context;
        private UserService _userService;

        [TestInitialize]
        public void Setup()
        {
            var dbName = $"TestDb_{Guid.NewGuid()}";
            var options = new DbContextOptionsBuilder<MusicStoreDbContext>()
                .UseInMemoryDatabase(databaseName: dbName)
                .Options;

            _context = new MusicStoreDbContext(options);
            _context.Database.EnsureCreated();
            _userService = new UserService(_context);
        }


        [TestMethod]
        public void Register_ShouldAddUser_WhenUsernameIsUnique()
        {
            var result = _userService.Register("testuser", "password123");

            Assert.IsTrue(result);
            Assert.AreEqual(1, _context.Users.Count());
            Assert.AreEqual("testuser", _context.Users.First().Username);
        }

        [TestMethod]
        public void Register_ShouldReturnFalse_WhenUsernameAlreadyExists()
        {
            _context.Users.Add(new User { Username = "existing", Password = "pass" });
            _context.SaveChanges();

            var result = _userService.Register("existing", "newpass");

            Assert.IsFalse(result);
            Assert.AreEqual(1, _context.Users.Count());
        }

        [TestMethod]
        public void Authenticate_ShouldReturnUser_WhenCredentialsAreCorrect()
        {
            _userService.Register("testuser", "password123");

            var user = _userService.Authenticate("testuser", "password123");

            Assert.IsNotNull(user);
            Assert.AreEqual("testuser", user.Username);
        }

        [TestMethod]
        public void Authenticate_ShouldBeCaseInsensitive_OnUsername()
        {
            _userService.Register("TestUser", "password123");

            var user = _userService.Authenticate("testuser", "password123");

            Assert.IsNotNull(user);
            Assert.AreEqual("TestUser", user.Username);
        }


        [TestMethod]
        public void Authenticate_ShouldReturnNull_WhenPasswordIsIncorrect()
        {
            _userService.Register("testuser", "password123");

            var user = _userService.Authenticate("testuser", "wrongpass");

            Assert.IsNull(user);
        }

        [TestMethod]
        public void Authenticate_ShouldReturnNull_WhenUserDoesNotExist()
        {
            var user = _userService.Authenticate("nouser", "nopass");

            Assert.IsNull(user);
        }
    }
}
