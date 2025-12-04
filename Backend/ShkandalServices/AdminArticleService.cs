using AutoMapper;
using shkandalData.DTOs.ArticleDtos;
using ShkandalData.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShkandalServices
{
    public class AdminArticleService
    {

        private readonly IAdminArticlesRepository _repository;

        private readonly IMapper _mapper;

        public AdminArticleService(IAdminArticlesRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<PagedList<ArticleAdminReadDto>> GetAllArticlesNotCheckedAsync(int pageNumber, int pageSize)
        {
            var articles = await _repository.GetAllArticlesNotCheckedAsync(pageNumber, pageSize);

            var result = articles.Select(article =>
                           _mapper.Map<ArticleAdminReadDto>(article));

            return result;
        }

        public async Task<ArticleAdminReadDto?> GetArticleByIdAsync(int id)
        {
            var article = await _repository.GetArticleByIdAsync(id);
            if (article == null)
                return null;

            var result = _mapper.Map<ArticleAdminReadDto>(article);

            return result;
        }

        public async Task<ArticleAdminUpdateDto?> UpdateAsync(int id, ArticleAdminUpdateRequest update)
        {
            var article = await _repository.GetArticleByIdAsync(id);
            if (article == null)
                return null;

            if (update.DetachCluster)
                article.ClusterId = null;

            if (update.IsRelevant.HasValue)
                article.IsRelevant = update.IsRelevant.Value;

            if (update.ClusterId.HasValue)
            {
                if (await _repository.ClusterExistsAsync(update.ClusterId.Value))
                    article.ClusterId = update.ClusterId.Value;
                else
                    return null;
            }

            var articleUpdated = await _repository.UpdateAsync(article);

            var result = _mapper.Map<ArticleAdminUpdateDto>(articleUpdated);
            return result;
        }
    }
}
