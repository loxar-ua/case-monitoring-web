using AutoMapper;
using shkandalData.DTOs.ClusterDtos;
using ShkandalData.Common;
using ShkandalInfrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShkandalServices
{
    public class ClusterService : IClusterService
    {

        private readonly IClusterRepository _repository;

        private readonly IMapper _mapper;

        public ClusterService(IClusterRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<PagedList<ClusterReadDto>> GetAllAsync(string? name, int pageNumber, int pageSize)
        {
            var clusters = await _repository.GetAllAsync(name, pageNumber, pageSize);

            var result = clusters.Select(cluster =>
                           _mapper.Map<ClusterReadDto>(cluster));

            return result;
        }

        public async Task<ClusterDetailedReadDto?> GetByIdAsync(int id)
        {
            var cluster = await _repository.GetByIdAsync(id);

            if (cluster == null)
            {
                return null;
            }

            var result = _mapper.Map<ClusterDetailedReadDto>(cluster);

            return result;
        }

        public async Task<ClusterUpdateDto?> IncrementViewCounterAsync(int id)
        {
            var cluster = await _repository.IncrementViewCounterAsync(id);

            if (cluster == null)
            {
                return null;
            }

            var result = _mapper.Map<ClusterUpdateDto>(cluster);

            return result;
        }
    }
}
