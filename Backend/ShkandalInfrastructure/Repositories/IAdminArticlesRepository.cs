using ShkandalData.Common;
using ShkandalData.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShkandalInfrastructure.Repositories
{
    public interface IAdminArticlesRepository
    {
        public Task<PagedList<Article>> GetAllArticlesNotCheckedAsync(string? searchTerm, int pageNumber, int pageSize);
        public Task<Article?> GetArticleByIdAsync(int id);
        public Task<Article> UpdateAsync(Article article);
        public Task<bool> ClusterExistsAsync(int clusterId);
    }
}
