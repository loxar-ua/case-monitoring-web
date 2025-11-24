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
    public class AdminArticlesRepository : IAdminArticlesRepository
    {
        private readonly ShkandalDbContext _context;
        private readonly DbSet<Article> _dbSet;
        public AdminArticlesRepository(ShkandalDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<Article>();
        }

        public async Task<IEnumerable<Article>> GetAllArticlesNotRelevant()
        {
            return await _dbSet
                .AsNoTracking()
                .Where(a => a.IsRelevant == false)
                .ToListAsync();
        }

        public async Task<Article?> GetArticleById(int id)
        {
            return await _dbSet
                .Include(a => a.Cluster)
                .FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task UpdateAsync(Article article)
        {
            _dbSet.Update(article);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> ClusterExistsAsync(int clusterId)
        {
            return await _context.Clusters.AnyAsync(c => c.Id == clusterId);
        }
    }
}
