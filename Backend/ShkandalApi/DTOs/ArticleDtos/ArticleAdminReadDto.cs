using shkandal_api.DTOs.ClusterDTOs;
using shkandal_api.DTOs.MediaDtos;

namespace shkandal_api.DTOs.ArticleDtos
{
    public class ArticleAdminReadDto
    {
        public int Id { get; set; }
        public MediaReadDto Media { set; get; }
        public string Title { get; set; }
        public string Link { get; set; }
        public string FeaturedImageURL { get; set; }
        public string Author { get; set; }
        public string Content { get; set; }
        public DateTime PublishedAt { get; set; }
        public bool IsRelevant { get; set; }
        public ClusterReadDto Cluster { get; set; }
    }
}
