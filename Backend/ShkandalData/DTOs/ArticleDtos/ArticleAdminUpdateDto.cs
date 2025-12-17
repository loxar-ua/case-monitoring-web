using shkandalData.DTOs.ClusterDtos;
using shkandalData.DTOs.MediaDtos;

namespace shkandalData.DTOs.ArticleDtos
{
    public class ArticleAdminUpdateDto
    {
        public bool? IsChecked { get; set; }
        public bool? IsRelevant { get; set; }
        public int? ClusterId { get; set; }
    }
}
