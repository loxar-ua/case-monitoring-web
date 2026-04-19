using ShkandalData.DTOs.EventDtos;

namespace shkandalData.DTOs.ClusterDtos
{
    public class ClusterDetailedReadDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int ViewCounter { get; set; }
        public string? Summary { get; set; }
        public string? FeaturedImageURL { get; set; }
        public ICollection<EventWithArticlesReadDto> Events { get; set; } = new List<EventWithArticlesReadDto>();
    }
}
