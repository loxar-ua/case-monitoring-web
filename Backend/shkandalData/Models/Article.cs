using ShkandalData.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace shkandalData.Models
{
    public class Article
    {
        public int Id { get; set; }
        public int? ClusterId { get; set; }
        public Cluster? Cluster { get; set; }
        public int MediaId { get; set; }
        public required Media Media { get; set; }
        public required string Title { get; set; }
        public string? Link { get; set; }
        public string? FeaturedImageURL { get; set; }
        public string? Author { get; set; }
        public string? Content { get; set; }
        public string? Status { get; set; }
        public DateTime? PublishedAt {  get; set; }
        public required bool IsRelevant { get; set; }
        public Pgvector.Vector? Embedding { get; set; }
    }
}
