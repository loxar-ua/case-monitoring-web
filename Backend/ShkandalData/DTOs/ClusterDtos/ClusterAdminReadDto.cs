using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace shkandalData.DTOs.ClusterDtos
{
    public class ClusterAdminReadDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public string? Content { get; set; }
        public string? FeaturedImageURL { get; set; }
        public DateTime? LastUpdatedAt { get; set; }
    }
}
