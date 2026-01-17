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
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;

        public ClusterService(IClusterRepository repository, ICategoryRepository categoryRepository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
            _categoryRepository = categoryRepository;
        }

        public async Task<PagedList<ClusterReadDto>> GetAllAsync(string? name, int? categoryId, string? sortBy, int pageNumber, int pageSize)
        {
            if (categoryId.HasValue && categoryId > 0)
            {
                if (! await _categoryRepository.CategoryExistsAsync(categoryId.Value))
                {
                    categoryId = null;
                }
            }

            var clusters = await _repository.GetAllAsync(name, categoryId, sortBy, pageNumber, pageSize);

            var dtos = _mapper.Map<IEnumerable<ClusterReadDto>>(clusters.Items);

            return new PagedList<ClusterReadDto>(
                dtos.ToList(),
                clusters.TotalCount,
                clusters.CurrentPage,
                clusters.PageSize
            );
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
