using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShkandalData.Models
{
    public class Cluster
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public bool IsActive { get; set; }
        public required int ViewCounter { get; set; }
        public string? Content { get; set; }
        public string? FeaturedImageURL { get; set; }
        public DateTime? LastUpdatedAt { get; set; }
        public ICollection<Article> Articles { get; set; } = new List<Article>();
    }
}
