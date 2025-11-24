using ShkandalData.Common;
using ShkandalData.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShkandalData.Repositories
{
    public interface IAdminArticlesRepository
    {
        public Task<PagedList<Article>> GetAllArticlesNotRelevant(int pageNumber, int pageSize);
        public Task<Article?> GetArticleById(int id);
        public Task UpdateAsync(Article article);
        public Task<bool> ClusterExistsAsync(int clusterId);
    }
}
