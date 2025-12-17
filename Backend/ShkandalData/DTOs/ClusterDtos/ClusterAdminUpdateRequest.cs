using shkandalData.DTOs.ArticleDtos;

namespace shkandalData.DTOs.ClusterDtos
{
    public class ClusterAdminUpdateRequest
    { 
        public string? Name { get; set; }
        public bool? IsRelevant { get; set; }
        public string? Summary { get; set; }
        public string? FeaturedImageURL { get; set; }
    }
}
