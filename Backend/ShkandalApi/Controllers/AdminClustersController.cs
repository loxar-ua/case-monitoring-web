using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using shkandalData.DTOs.ClusterDtos;
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

        [HttpGet("{id}")]
        public async Task<ActionResult<ClusterAdminReadDto>> GetClusterByIdAsync(int id)
        {
            var result = await _service.GetClusterByIdAsync(id);

            if (result == null)
            {
                return NotFound();
            }

            return Ok(result);
        }


        [HttpPatch("{id}")]
        public async Task<ActionResult<ClusterAdminUpdateDto>> ClusterUpdate(int id,
            [FromBody] ClusterAdminUpdateRequest update)
        {

            var result = await _service.ClusterUpdate(id, update);
            if (result == null)
                return NotFound();

            return result;
        }
    }
}