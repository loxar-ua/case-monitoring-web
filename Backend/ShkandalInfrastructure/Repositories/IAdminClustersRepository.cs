using ShkandalData.Common;
using ShkandalData.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShkandalInfrastructure.Repositories
{
    public interface IAdminClustersRepository
    {
        public Task<PagedList<Cluster>> GetAllClustersAsync(string? searchTerm, int pageNumber, int pageSize);
        public Task<Cluster?> GetClusterByIdAsync(int id);
        public Task<Cluster> UpdateAsync(Cluster cluster);
    }
}
