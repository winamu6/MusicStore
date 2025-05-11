using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicStoreApp.Core.Models
{
    namespace MusicStoreApp.Core.Models
    {
        public enum UserRole
        {
            Customer,
            Manager,
            Admin
        }

        public class User
        {
            public string Username { get; set; }
            public string Password { get; set; }
            public UserRole Role { get; set; }
        }
    }

}
