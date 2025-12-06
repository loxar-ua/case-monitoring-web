using shkandalData.DTOs.ClusterDtos;
using ShkandalData.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShkandalServices
{
    public interface IAdminClusterService
    {
        public Task<PagedList<ClusterAdminReadDto>> GetAllClustersAsync(string? name, int pageNumber, int pageSize);

        public Task<ClusterAdminDetailedReadDto?> GetClusterByIdAsync(int id);

        public Task<ClusterAdminUpdateDto?> UpdateAsync(int id, ClusterAdminUpdateRequest update);
    }
}
