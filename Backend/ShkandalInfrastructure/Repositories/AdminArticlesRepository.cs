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
    public class AdminArticlesRepository : IAdminArticlesRepository
    {
        private readonly ShkandalDbContext _context;
        private readonly DbSet<Article> _dbSet;
        public AdminArticlesRepository(ShkandalDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<Article>();
        }

        public async Task<PagedList<Article>> GetAllArticlesNotCheckedAsync(string? searchTerm, int pageNumber, int pageSize)
        {
            var baseQuery = _dbSet
                .AsNoTracking()
                .Include(a => a.Media)
                .Include(a => a.Cluster)
                .Where(a => a.IsChecked == false);

            var query = SearchExtensions.ApplySearch(baseQuery, searchTerm);

            return await PagedList<Article>.CreateAsync(query, pageNumber, pageSize);
        }

        public async Task<Article?> GetArticleByIdAsync(int id)
        {
            return await _dbSet
                .Include(a => a.Media)
                .Include(a => a.Cluster)
                .FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<Article> UpdateAsync(Article article)
        {
            _dbSet.Update(article);
            await _context.SaveChangesAsync();

            return article;
        }

        public async Task<bool> ClusterExistsAsync(int clusterId)
        {
            return await _context.Clusters.AnyAsync(c => c.Id == clusterId);
        }
    }
}
