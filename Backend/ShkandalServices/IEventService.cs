using shkandalData.DTOs.ClusterDtos;
using ShkandalData.DTOs.EventDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShkandalServices
{
    public interface IEventService
    {
        public Task<EventWithArticlesReadDto?> GetByIdAsync(int id);
    }
}
