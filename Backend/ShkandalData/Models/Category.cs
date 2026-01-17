
namespace ShkandalData.Models
{
    public class Category
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public ICollection<Cluster> Clusters { get; set; } = new List<Cluster>();

    }
}
