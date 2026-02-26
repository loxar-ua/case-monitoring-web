using Microsoft.AspNetCore.Mvc;
using ShkandalData.DTOs.EventDtos;
using ShkandalServices;

namespace shkandal_api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EventController : ControllerBase
    {
        private readonly IEventService _service;

        public EventController(IEventService service)
        {
            _service = service;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<EventWithArticlesReadDto>> GetByIdAsync(int id)
        {
            var result = await _service.GetByIdAsync(id);

            if (result == null)
            {
                return NotFound();
            }

            return Ok(result);
        }
    }
}
