using Microsoft.EntityFrameworkCore;
using ShkandalData.Common;
using ShkandalData.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShkandalInfrastructure.Repositories
{
    public class CategoryRepository: ICategoryRepository
    {
        private readonly ShkandalDbContext _context;
        private readonly DbSet<Category> _dbSet;
        public CategoryRepository(ShkandalDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<Category>();
        }
        public async Task<List<Category>> GetAllCategoriesAsync()
        {
            return await _dbSet
                .AsNoTracking()
                .OrderBy(c => c.Name)
                .ToListAsync();
        }

        public async Task<Category?> GetCategoryByIdAsync(int id)
        {
            return await _dbSet
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<bool> CategoryExistsAsync(int id)
        {
            return await _dbSet.AnyAsync(c => c.Id == id);
        }
    
}
}
