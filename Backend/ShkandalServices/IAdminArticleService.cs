using shkandalData.DTOs.ArticleDtos;
using ShkandalData.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShkandalServices
{
    public interface IAdminArticleService
    {
        public Task<PagedList<ArticleAdminReadDto>> GetAllArticlesNotCheckedAsync(string? name, int pageNumber, int pageSize);

        public Task<ArticleAdminReadDto?> GetArticleByIdAsync(int id);

        public Task<ArticleAdminUpdateDto?> UpdateAsync(int id, ArticleAdminUpdateRequest update);
  
    }
}
