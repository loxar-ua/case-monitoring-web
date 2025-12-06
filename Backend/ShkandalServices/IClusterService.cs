using shkandalData.DTOs.ClusterDtos;
using ShkandalData.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShkandalServices
{
    public interface IClusterService
    {
        public Task<PagedList<ClusterReadDto>> GetAllAsync(string? name, int pageNumber, int pageSize);
        public Task<ClusterDetailedReadDto?> GetByIdAsync(int id);
        public Task<ClusterUpdateDto?> IncrementViewCounterAsync(int id);
    }
}
