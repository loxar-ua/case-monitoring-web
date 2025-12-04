using AutoMapper;
using shkandalData.DTOs.ArticleDtos;
using shkandalData.DTOs.ClusterDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShkandalServices
{
    public class AdminClusterService : IAdminClusterService
    {

        private readonly IAdminClustersRepository _repository;

        private readonly IMapper _mapper;

        public AdminClusterService(IAdminClustersRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<PagedList<ClusterAdminReadDto>> GetAllClustersAsync(int pageNumber, int pageSize)
        {
            var clusters = await _repository.GetAllClustersAsync(pageNumber, pageSize);

            var result = clusters.Select(cluster =>
                           _mapper.Map<ClusterAdminReadDto>(cluster));

            return result;
        }

        public async Task<ClusterAdminDetailedReadDto?> GetClusterByIdAsync(int id)
        {
            var cluster = await _repository.GetClusterByIdAsync(id);

            if (cluster == null)
            {
                return null;
            }

            var result = _mapper.Map<ClusterAdminDetailedReadDto>(cluster);

            return result;
        }

        public async Task<ClusterAdminUpdateDto?> UpdateAsync(int id, ClusterAdminUpdateRequest update)
        {

            var cluster = await _repository.GetClusterByIdAsync(id);
            if (cluster == null)
                return null;

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
            return result;
        }
    }
}
