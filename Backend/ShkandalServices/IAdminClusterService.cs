using shkandalData.DTOs.ClusterDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShkandalServices
{
    public interface IAdminClusterService
    {
        public Task<ClusterAdminReadDto?> GetClusterByIdAsync(int id);

        public Task<ClusterAdminUpdateDto?> ClusterUpdate(int id, ClusterAdminUpdateRequest update);
    }
}
