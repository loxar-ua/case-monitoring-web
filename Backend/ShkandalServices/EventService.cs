using AutoMapper;
using shkandalData.DTOs.ClusterDtos;
using ShkandalData.DTOs.EventDtos;
using ShkandalInfrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShkandalServices
{
    public class EventService: IEventService
    {

        private readonly IEventRepository _repository;
        private readonly IMapper _mapper;

        public EventService(IEventRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<EventWithArticlesReadDto?> GetByIdAsync(int id)
        {
            var eventItem = await _repository.GetByIdAsync(id);

            if (eventItem == null)
            {
                return null;
            }

            var result = _mapper.Map<EventWithArticlesReadDto>(eventItem);

            return result;
        }
    }
}
