using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using shkandalData.DTOs.ClusterDtos;
using ShkandalData.Common;
using ShkandalServices;

namespace shkandal_api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]
    public class AdminClustersController: ControllerBase
    {
        private readonly IAdminClusterService _service;

        public AdminClustersController(IAdminClusterService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<PagedList<ClusterAdminReadDto>>> GetAllClustersAsync([FromQuery] string name, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            var result = await _service.GetAllClustersAsync(name, pageNumber, pageSize);

            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ClusterAdminDetailedReadDto>> GetClusterByIdAsync(int id)
        {
            var result = await _service.GetClusterByIdAsync(id);

            if (result == null)
            {
                return NotFound();
            }

            return Ok(result);
        }


        [HttpPatch("{id}")]
        public async Task<ActionResult<ClusterAdminUpdateDto>> UpdateAsync(int id,
            [FromBody] ClusterAdminUpdateRequest update)
        {

            var result = await _service.UpdateAsync(id, update);
            if (result == null)
                return NotFound();

            return Ok(result);
        }
    }
}