using ShkandalData.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShkandalInfrastructure.Repositories
{
    public interface IEventRepository
    {
        public Task<Event?> GetByIdAsync(int id);
    }
}
