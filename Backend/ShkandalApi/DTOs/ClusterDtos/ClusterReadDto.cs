using shkandalData.Models;

namespace shkandal_api.DTOs.ClusterDTOs
{
    public class ClusterReadDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Content { get; set; }
        public string? FeaturedImageURL { get; set; }
        public DateTime? LastUpdatedAt { get; set; }
  }
}
