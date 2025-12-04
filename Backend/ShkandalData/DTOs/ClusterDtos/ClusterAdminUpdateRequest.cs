using shkandalData.DTOs.ArticleDtos;

namespace shkandalData.DTOs.ClusterDtos
{
    public class ClusterAdminUpdateRequest
    { 
        public string? Name { get; set; }
        public bool? IsActive { get; set; }
        public string? Content { get; set; }
        public string? FeaturedImageURL { get; set; }
    }
}
