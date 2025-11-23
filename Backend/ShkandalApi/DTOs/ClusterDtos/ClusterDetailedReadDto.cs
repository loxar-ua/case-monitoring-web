using shkandal_api.DTOs.ArticleDtos;
using shkandalData.Models;

namespace shkandal_api.DTOs.ClusterDtos
{
    public class ClusterDetailedReadDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int ViewCounter { get; set; }
        public string? Content { get; set; }
        public ICollection<ArticleReadDto> Articles { get; set; } = new List<ArticleReadDto>();
    }
}
