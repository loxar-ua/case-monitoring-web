using AutoMapper;
using shkandalData.DTOs.ArticleDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShkandalServices
{
    public class AdminArticleService
    {

        private readonly IAdminArticleRepository _repository;

        private readonly IMapper _mapper;

        public AdminArticleService(IAdminArticleRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<PagedList<ArticleAdminReadDto>> GetAllArticlesNotRelevant(int pageNumber, int pageSize)
        {
            var articles = await _repository.GetAllArticlesNotRelevant(pageNumber, pageSize);

            var result = articles.Select(article =>
                           _mapper.Map<ArticleAdminReadDto>(article));

            return result;
        }

        public async Task<ArticleAdminReadDto?> GetArticleById(int id)
        {
            var article = await _repository.GetArticleById(id);
            if (article == null)
                return null;

            var result = _mapper.Map<ArticleAdminReadDto>(article);

            return result;
        }

        public async Task<ArticleAdminUpdateDto?> ArticleUpdate(int id, ArticleAdminUpdateRequest update)
        {
            var article = await _repository.GetArticleById(id);
            if (article == null)
                return null;

            if (update.DetachCluster)
                article.ClusterId = null;

            if (update.IsRelevant.HasValue)
                article.IsRelevant = update.IsRelevant.Value;

            if (update.ClusterId.HasValue)
            {
                if (await _repository.ClusterExistsAsync(update.ClusterId))
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
