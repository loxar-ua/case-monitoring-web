using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using shkandalData.DTOs.ArticleDtos;
using ShkandalServices;


namespace shkandal_api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]
    public class AdminArticlesController:ControllerBase
    {
        private readonly IAdminArticlesService _service;

        public AdminArticlesController(IAdminArticlesService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<PagedList<ArticleAdminReadDto>>> GetAllArticlesNotRelevant([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            var result = await _service.GetAllArticlesNotRelevant(pageNumber, pageSize);

            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ArticleAdminReadDto>> GetArticleById(int id)
        {
            var result = await _service.GetArticleById(id);
            if (result == null)
                return NotFound();

            return Ok(result);
        }


        [HttpPatch("{id}")]
        public async Task<ActionResult<ArticleAdminUpdateDto>> ArticleUpdate(int id,
            [FromBody] ArticleAdminUpdateRequest update)
        {
            var result = await _service.ArticleUpdate(id, update);
            if (result == null)
                return NotFound();

            return Ok(result);
        }
    }
}
