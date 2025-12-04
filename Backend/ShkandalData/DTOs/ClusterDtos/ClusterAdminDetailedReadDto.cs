using shkandalData.DTOs.ArticleDtos;

namespace shkandalData.DTOs.ClusterDtos
{
    public class ClusterAdminDetailedReadDto
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public bool IsActive { get; set; }
        public string? Content { get; set; }
        public string? FeaturedImageURL { get; set; }
        public DateTime? LastUpdatedAt { get; set; }
        public ICollection<ArticleReadDto> Articles { get; set; } = new List<ArticleReadDto>();
    }
}
