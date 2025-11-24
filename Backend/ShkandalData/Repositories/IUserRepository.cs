using ShkandalData.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShkandalData.Repositories
{
    public interface IUserRepository
    {
        public Task<User?> GetUserByUsername(string username);
    }
}
