using shkandalData.DTOs.ClusterDtos;
using shkandalData.DTOs.MediaDtos;

namespace shkandalData.DTOs.ArticleDtos
{
    public class ArticleAdminUpdateDto
    {
        public bool IsRelevant { get; set; }
        public int? ClusterId { get; set; }
    }
}
