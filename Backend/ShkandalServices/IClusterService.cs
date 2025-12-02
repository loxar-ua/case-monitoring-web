using shkandalData.DTOs.ClusterDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShkandalServices
{
    public interface IClusterService
    {
        public Task<PagedList<ClusterReadDto>> GetAllClustersAsync(string? name, int pageNumber, int pageSize);
        public Task<ClusterDetailedReadDto?> GetClusterById(int id);
        public Task<ClusterUpdateDto?> IncrementViewCounterAsync(int id);
    }
}
