using ShkandalData.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShkandalData.DTOs.EventDtos
{
    public class EventReadDto
    {
        public int Id { get; set; }
        public required string Title { get; set; }
        public string Description { get; set; }
        public DateTime? Date { get; set; }
    }
}
