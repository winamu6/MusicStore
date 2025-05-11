using MusicStoreApp.Core.Models.MusicStoreApp.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicStoreApp.Core.Services
{
    public class UserService
    {
        private readonly List<User> _users;

        public UserService()
        {
            _users = new List<User>
            {
                new User { Username = "admin", Password = "admin", Role = UserRole.Admin },
                new User { Username = "manager", Password = "manager", Role = UserRole.Manager },
                new User { Username = "user", Password = "user", Role = UserRole.Customer }
            };
        }

        public User? Authenticate(string username, string password)
        {
            return _users.FirstOrDefault(u =>
                u.Username.Equals(username, StringComparison.OrdinalIgnoreCase) &&
                u.Password == password);
        }

        public bool Register(string username, string password, UserRole role = UserRole.Customer)
        {
            if (_users.Any(u => u.Username.Equals(username, StringComparison.OrdinalIgnoreCase)))
                return false;

            _users.Add(new User
            {
                Username = username,
                Password = password,
                Role = role
            });

            return true;
        }

    }
}
