using ShkandalData.Common;
using ShkandalData.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShkandalData.Repositories
{
    public interface IClusterRepository 
    {
        public Task<PagedList<Cluster>> GetAllAsync(string? searchTerm, int pageNumber, int pageSize);
        public Task<Cluster?> GetByIdAsync(int id);
        public Task<Cluster?> IncrementViewCounterAsync(int id);
    }
}
