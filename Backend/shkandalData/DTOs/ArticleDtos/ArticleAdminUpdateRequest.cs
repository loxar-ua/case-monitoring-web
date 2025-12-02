namespace shkandalData.DTOs.ArticleDtos
{
    public class ArticleAdminUpdateRequest
    {
        public bool? IsRelevant { get; set; }
        public int? ClusterId { get; set; }
        public bool DetachCluster { get; set; }
    }
}
