using MusicStoreApp.Core.Data;
using MusicStoreApp.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicStoreApp.Core.Services
{
    public class UserService
    {
        private readonly MusicStoreDbContext _context;

        public UserService(MusicStoreDbContext context)
        {
            _context = context;
        }

        public User? Authenticate(string username, string password)
        {
            return _context.Users
                .Where(u => u.Username.ToLower() == username.ToLower() && u.Password == password)
                .FirstOrDefault();
        }

        public bool Register(string username, string password, UserRole role = UserRole.Customer)
        {
            if (_context.Users.Any(u => u.Username.ToLower() == username.ToLower()))
                return false;

            var user = new User
            {
                Username = username,
                Password = password,
                Role = role
            };

            _context.Users.Add(user);
            _context.SaveChanges();

            return true;
        }
    }
}
