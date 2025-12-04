using shkandalData.DTOs.ClusterDtos;
using shkandalData.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShkandalServices
{
    public interface IAdminClusterService
    {
        public Task<PagedList<ClusterAdminReadDto>> GetAllClustersAsync(int pageNumber, int pageSize);

        public Task<ClusterAdminDetailedReadDto?> GetClusterByIdAsync(int id);

        public Task<ClusterAdminUpdateDto?> UpdateAsync(int id, ClusterAdminUpdateRequest update);
    }
}
