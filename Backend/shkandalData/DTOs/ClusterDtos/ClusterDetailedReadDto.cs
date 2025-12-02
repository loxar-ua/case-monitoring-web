using shkandalData.DTOs.ArticleDtos;
using shkandalData.Models;

namespace shkandalData.DTOs.ClusterDtos
{
    public class ClusterDetailedReadDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int ViewCounter { get; set; }
        public string? Content { get; set; }
        public string? FeaturedImageURL { get; set; }
        public ICollection<ArticleReadDto> Articles { get; set; } = new List<ArticleReadDto>();
    }
}
