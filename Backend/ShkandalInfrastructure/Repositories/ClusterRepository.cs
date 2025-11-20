using Microsoft.EntityFrameworkCore;
using ShkandalData.Models;
using ShkandalData.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShkandalInfrastructure.Repositories
{
    public class ClusterRepository : GenericRepository<Cluster>, IClusterRepository
    {
        public ClusterRepository(ShkandalDbContext context) : base(context) { }

        public async Task<IEnumerable<Cluster>> SearchByNameAsync(string term)
        {
            return await _dbSet
                .Where(c => c.Name.Contains(term))
                .ToListAsync();
        }
    }
}
