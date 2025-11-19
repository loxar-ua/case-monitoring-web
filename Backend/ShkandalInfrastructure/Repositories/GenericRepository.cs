using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShkandalInfrastructure.ShkandalDbContext;

namespace shkandalData.Repositories
{
    public class GenericRepository<T>(ShkandalDbContext context) : IGenericRepository<T> where T : class
    {
    }
}
