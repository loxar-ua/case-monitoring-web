using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using shkandal_api.DTOs.ClusterDtos;
using shkandal_api.DTOs.ClusterDTOs;

namespace shkandal_api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]
    public class AdminClustersController: ControllerBase
    {
        private readonly IAdminClustersControllerRepository _repository;
        private readonly IMapper _mapper;

        public AdminClustersController(IAdminClustersControllerRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<ClusterAdminReadDto>> GetClusterByIdAsync(int id)
        {
            var cluster = await _repository.GetClusterByIdAsync(id);

            if (cluster == null)
            {
                return NotFound();
            }

            var result = _mapper.Map<ClusterAdminReadDto>(cluster);

            return Ok(result);
        }


        [HttpPatch("{id}")]
        public async Task<ActionResult<ClusterAdminUpdateDto>> ClusterUpdate(int id,
            [FromBody] ClusterAdminUpdateRequest update)
        {

            var cluster = await _repository.GetClusterById(id);
            if (cluster == null)
                return NotFound();

            if (!string.IsNullOrWhiteSpace(update.Name))
                cluster.Name = update.Name;

            if (update.IsActive.HasValue)
                cluster.IsActive = update.IsActive.Value;

            if (!string.IsNullOrWhiteSpace(update.Content))
                cluster.Content = update.Content;

            if (!string.IsNullOrWhiteSpace(update.FeaturedImageURL))
                cluster.FeaturedImageURL = update.FeaturedImageURL;

            var clusterUpdated = await _repository.UpdateAsync(cluster);

            var result = _mapper.Map<ClusterAdminUpdateDto>(clusterUpdated);
            return Ok(result);
        }
    }
}