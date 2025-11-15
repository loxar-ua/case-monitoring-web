using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace shkandalData.Models
{
    public class Cluster
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public bool IsActive { get; set; }

        public ICollection<Article> Articles { get; set; } = new List<Article>();
    }
}
