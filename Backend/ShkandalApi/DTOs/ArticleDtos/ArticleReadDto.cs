using shkandal_api.DTOs.MediaDtos;
using shkandalData.Models;

namespace shkandal_api.DTOs.ArticleDtos
{
    public class ArticleReadDto
    {
        public int Id { get; set; }
        public MediaReadDto Media { set; get; }
        public string Title { get; set; }
        public string Link { get; set; }
        public string FeaturedImageURL { get; set; }
        public string Author { get; set; }
        public string Content { get; set; }
        public DateTime PublishedAt { get; set; }
    }
}
