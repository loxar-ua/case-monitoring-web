using ShkandalData.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShkandalData.Repositories
{
    public interface IClusterRepository : IGenericRepository<Cluster>
    {
        Task<IEnumerable<Cluster>> SearchByNameAsync(string term);
    }
}
