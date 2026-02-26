using shkandalData.DTOs.ArticleDtos;
using ShkandalData.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShkandalData.DTOs.EventDtos
{
    public class EventWithArticlesReadDto
    {
        public int Id { get; set; }
        public required string Title { get; set; }
        public string Description { get; set; }
        public DateTime? EventTime { get; set; }
        public ICollection<ArticleReadDto> Articles { get; set; } = new List<ArticleReadDto>();
    }
}
