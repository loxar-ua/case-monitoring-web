using Microsoft.AspNetCore.Mvc;
using shkandalData.DTOs.ClusterDtos;
using ShkandalServices;


namespace shkandal_api.Controllers
{
    [ApiController]
    //[Authorize]
    [Route("api/[controller]")]
    public class ClusterController: ControllerBase
    {
        private readonly IClusterService _service;

        public ClusterController(IClusterService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<PagedList<ClusterReadDto>>> GetAllClustersAsync([FromQuery] string name, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            var result = await _service.GetAllClustersAsync(name, pageNumber, pageSize);

            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ClusterDetailedReadDto>> GetClusterById(int id)
        {
            var result = await _service.GetClusterById(id);

            if(result == null)
            {
                return NotFound();
            }

            return Ok(result);
        }


        [HttpPatch("{id}")]
        public async Task<ActionResult<ClusterUpdateDto>> IncrementViewCounter(int id)
        {
            var result = await _service.IncrementViewCounterAsync(id);

            if (result == null)
            {
                return NotFound();
            }

            return Ok(result);
        }
    }
}
