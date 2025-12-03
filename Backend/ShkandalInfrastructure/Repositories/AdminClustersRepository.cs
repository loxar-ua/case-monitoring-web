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
    public class AdminClustersRepository : IAdminClustersRepository
    {
        private readonly ShkandalDbContext _context;
        private readonly DbSet<Cluster> _dbSet;
        public AdminClustersRepository(ShkandalDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<Cluster>();
        }
        public async Task<PagedList<Cluster>> GetAllClustersAsync(string? searchTerm, int pageNumber, int pageSize)
        {
            var baseQuery = _dbSet.AsNoTracking();

            var query = SearchExtensions.ApplySearch(baseQuery, searchTerm);

            return await PagedList<Cluster>.CreateAsync(query, pageNumber, pageSize);
        }

        public async Task<Cluster?> GetClusterByIdAsync(int id)
        {
            return await _dbSet
                .Include(c => c.Articles.OrderByDescending(a => a.PublishedAt))
                    .ThenInclude(a => a.Media)
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<Cluster> UpdateAsync(Cluster cluster)
        {
            _dbSet.Update(cluster);
            await _context.SaveChangesAsync();

            return cluster;
        }
    }
}
