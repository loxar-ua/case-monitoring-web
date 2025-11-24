using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using shkandal_api.DTOs.ClusterDtos;
using shkandal_api.DTOs.ClusterDTOs;
using shkandalData.Models;

namespace shkandal_api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClusterController: ControllerBase
    {
        private readonly IClusterRepository _repository;
        private readonly IMapper _mapper;

        public ClusterController(IClusterRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ClusterReadDto>>> GetAllClustersAsync([FromQuery] string name, [FromQuery] int pageNumber, [FromQuery] int pageSize)
        {
            var clusters = await _repository.GetAllAsync(name, pageNumber, pageSize);

            var result = clusters.Select(cluster =>
                           _mapper.Map<ClusterReadDto>(cluster)).ToList();

            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ClusterDetailedReadDto>> GetClusterById(int id)
        {
            var cluster = await _repository.GetByIdAsync(id);

            if(cluster == null)
            {
                return NotFound();
            }

            var result = _mapper.Map<ClusterDetailedReadDto>(cluster);

            return Ok(result);
        }


        [HttpPatch("{id}")]
        public async Task<ActionResult<ClusterUpdateDto>> IncrementViewCounter(int id)
        {
            var cluster = await _repository.IncrementViewCounter(id);

            if (cluster == null)
            {
                return NotFound();
            }

            var result = _mapper.Map<ClusterUpdateDto>(cluster);

            return Ok(result);
        }
    }
}
