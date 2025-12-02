using shkandalData.DTOs.ArticleDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShkandalServices
{
    public interface IAdminArticleService
    {
        public Task<PagedList<ArticleAdminReadDto>> GetAllArticlesNotRelevant(int pageNumber, int pageSize);

        public Task<ArticleAdminReadDto?> GetArticleById(int id);

        public Task<ArticleAdminUpdateDto?> ArticleUpdate(int id, ArticleAdminUpdateRequest update);
  
    }
}
