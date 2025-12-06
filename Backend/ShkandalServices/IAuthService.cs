using ShkandalData.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShkandalServices
{
    public interface IAuthService
    {
        Task<string> LoginAdmin(string username, string password);
        string HashPassword(User user, string password);
    }
}
