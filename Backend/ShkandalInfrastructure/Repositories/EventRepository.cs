using Microsoft.EntityFrameworkCore;
using ShkandalData.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShkandalInfrastructure.Repositories
{
    public class EventRepository: IEventRepository
    {
        private readonly ShkandalDbContext _context;
        private readonly DbSet<Event> _dbSet;
        public EventRepository(ShkandalDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<Event>();
        }

        public async Task<Event?> GetByIdAsync(int id)
        {
            return await _dbSet
                .AsNoTracking()
                .Include(c => c.Articles.OrderByDescending(a => a.PublishedAt))
                    .ThenInclude(a => a.Media)
                .FirstOrDefaultAsync(c => c.Id == id);
        }
    }
}
