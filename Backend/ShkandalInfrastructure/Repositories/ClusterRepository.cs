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
    public class ClusterRepository : IClusterRepository
    {
        private readonly ShkandalDbContext _context;
        private readonly DbSet<Cluster> _dbSet;
        public ClusterRepository(ShkandalDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<Cluster>();
        }
        public async Task<PagedList<Cluster>> GetAllAsync(string? searchTerm, int? categoryId, string? sortBy, int pageNumber, int pageSize)
        {
            var query = _dbSet.AsNoTracking().Where(c => c.IsRelevant == true);
            
            if(!string.IsNullOrWhiteSpace(searchTerm))
                query = SearchExtensions.ApplySearch(query, searchTerm);
            else
                query = query.ApplyCategoryFilter(categoryId).ApplySorting(sortBy);

            return await PagedList<Cluster>.CreateAsync(query, pageNumber, pageSize);
        }

        public async Task<Cluster?> GetByIdAsync(int id)
        {
            return await _dbSet
                .AsNoTracking()
                .Include(c => c.Events.OrderBy(a => a.Date))
                    .ThenInclude(e => e.Articles)
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<Cluster?> IncrementViewCounterAsync(int id)
        {
            var updatedRows = await _dbSet
                .Where(c => c.Id == id)
                .ExecuteUpdateAsync(s => s.SetProperty(c =>c.ViewCounter, c  => c.ViewCounter + 1));
            if (updatedRows == 0) return null;

            return await _dbSet
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == id);
        }
    }
}
