using ShkandalData.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShkandalData.Repositories
{
    public interface IAdminClustersRepository
    {
        public Task<Cluster?> GetClusterByIdAsync(int id);
        public Task UpdateAsync(Cluster cluster);
    }
}
