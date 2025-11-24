using shkandal_api.DTOs.ClusterDTOs;
using shkandal_api.DTOs.MediaDtos;

namespace shkandal_api.DTOs.ArticleDtos
{
    public class ArticleAdminUpdateDto
    {
        public bool IsRelevant { get; set; }
        public int ClusterId { get; set; }
    }
}
