namespace shkandalData.DTOs.ClusterDtos
{
    public class ClusterReadDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Summary { get; set; }
        public string? FeaturedImageURL { get; set; }
        public DateTime? LastUpdatedAt { get; set; }
    }
}
