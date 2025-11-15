using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace shkandalData.Models
{
    public class User
    {
        public int Id { get; set; }
        public required string Username { get; set; }
        public required string PasswordHash { get; set; }
        public required UserRole Role { get; set; }

        public enum UserRole
        {
            Visitor = 0,
            Admin = 1
        }
    }
}
