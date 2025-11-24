using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using shkandal_api.DTOs.ArticleDtos;
using shkandal_api.DTOs.ClusterDtos;
using shkandal_api.DTOs.ClusterDTOs;
using shkandalData.Models;

namespace shkandal_api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]
    public class AdminArticlesController:ControllerBase
    {
        private readonly IAdminArticlesControllerRepository _repository;
        private readonly IMapper _mapper;

        public AdminArticlesController(IAdminArticlesControllerRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ArticleAdminReadDto>>> GetAllArticlesNotRelevant([FromQuery] int pageNumber, [FromQuery] int pageSize)
        {
            var articles = await _repository.GetAllArticlesNotRelevant(pageNumber, pageSize);

            var result = articles.Select(article =>
                           _mapper.Map<ArticleAdminReadDto>(article)).ToList();

            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ArticleAdminReadDto>> GetArticleById(int id)
        {
            var article = await _repository.GetArticleById(id);
            if (article == null)
                return NotFound();

            var result = _mapper.Map<ArticleAdminReadDto>(article);

            return Ok(result);
        }


        [HttpPatch("{id}")]
        public async Task<ActionResult<ArticleAdminUpdateDto>> ArticleUpdate(int id,
            [FromBody] ArticleAdminUpdateRequest update)
        {
            var article = await _repository.GetArticleById(id);
            if (article == null)
                return NotFound();

            if (update.DetachCluster)
                article.ClusterId = null;

            if (update.IsRelevant.HasValue)
                article.IsRelevant = update.IsRelevant.Value;

            if (update.ClusterId.HasValue)
            {
                if(await _repository.ClusterExistsAsync(update.ClusterId))
                    article.ClusterId = update.ClusterId.Value;
                else
                    return NotFound();
            }

            var articleUpdated = await _repository.UpdateAsync(article);

            var result = _mapper.Map<ArticleAdminUpdateDto>(articleUpdated);
            return Ok(result);
        }
    }
}
