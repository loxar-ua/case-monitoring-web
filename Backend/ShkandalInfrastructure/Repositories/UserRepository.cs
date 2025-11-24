using Microsoft.EntityFrameworkCore;
using ShkandalData.Models;
using ShkandalData.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShkandalInfrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ShkandalDbContext _context;
        private readonly DbSet<User> _dbSet;
        public UserRepository(ShkandalDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<User>();
        }
        public async Task<User?> GetUserByUsername(string username)
        {
            return await _dbSet
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Username == username);
        }
    }
}
