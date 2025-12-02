using AutoMapper;
using shkandalData.DTOs.ClusterDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShkandalServices
{
    public class AdminClusterService: IAdminClusterService
    {

        private readonly IAdminClusterRepository _repository;

        private readonly IMapper _mapper;

        public AdminClusterService(IAdminClusterRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }


        public async Task<ClusterAdminReadDto?> GetClusterByIdAsync(int id)
        {
            var cluster = await _repository.GetClusterByIdAsync(id);

            if (cluster == null)
            {
                return null;
            }

            var result = _mapper.Map<ClusterAdminReadDto>(cluster);

            return result;
        }

        public async Task<ClusterAdminUpdateDto?> ClusterUpdate(int id,ClusterAdminUpdateRequest update)
        {

            var cluster = await _repository.GetClusterById(id);
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
