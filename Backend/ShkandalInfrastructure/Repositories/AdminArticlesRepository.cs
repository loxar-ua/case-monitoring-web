using Microsoft.EntityFrameworkCore;
using ShkandalData.Common;
using ShkandalData.Models;
using ShkandalData.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

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

        public async Task<PagedList<Article>> GetAllArticlesNotRelevant(int pageNumber, int pageSize)
        {
            var query = _dbSet
                .AsNoTracking()
                .Include(a => a.Media)
                .Include(a => a.Cluster)
                .Where(a => a.IsRelevant == false)
                .OrderByDescending(a => a.PublishedAt);

            return await PagedList<Article>.CreateAsync(query, pageNumber, pageSize);
        }

        public async Task<Article?> GetArticleById(int id)
        {
            return await _dbSet
                .Include(a => a.Media)
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
