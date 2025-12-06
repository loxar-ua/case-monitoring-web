using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using shkandalData.DTOs.ArticleDtos;
using ShkandalData.Common;
using ShkandalServices;


namespace shkandal_api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]
    public class AdminArticlesController : ControllerBase
    {
        private readonly IAdminArticleService _service;

        public AdminArticlesController(IAdminArticleService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<PagedList<ArticleAdminReadDto>>> GetAllArticlesNotCheckedAsync([FromQuery] string name, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            var result = await _service.GetAllArticlesNotCheckedAsync(name, pageNumber, pageSize);

            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ArticleAdminReadDto>> GetArticleByIdAsync(int id)
        {
            var result = await _service.GetArticleByIdAsync(id);
            if (result == null)
                return NotFound();

            return Ok(result);
        }


        [HttpPatch("{id}")]
        public async Task<ActionResult<ArticleAdminUpdateDto>> UpdateAsync(int id,
            [FromBody] ArticleAdminUpdateRequest update)
        {
            var result = await _service.UpdateAsync(id, update);
            if (result == null)
                return NotFound();

            return Ok(result);
        }
    }
}
