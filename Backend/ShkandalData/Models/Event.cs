using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShkandalData.Models
{
    public class Event
    {
        public int Id { get; set; }
        public required string Title { get; set; }
        public string Description { get; set; }
        public DateTime? EventTime { get; set; }
        public int? ClusterId { get; set; }
        public Cluster? Cluster { get; set; }
        public ICollection<Article> Articles { get; set; } = new List<Article>();

    }
}
