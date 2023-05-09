using EventMate.Core.Model.Concrete;
using EventMate.Core.Repository;
using EventMate.Repository.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventMate.Repository.Repository
{
    public class EventRepository : GenericRepository<Event>, IEventRepository
    {
        public EventRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<IEnumerable<Event>> GetEventsWithDetails()
        {
            //Eager Loading
            return _context.Events.AsNoTracking().Include(x => x.Category).Include(x => x.City).AsEnumerable();
        }

        public Task<Event> GetEventWithDetails(int id)
        {
            //Eager Loading
            return _context.Events.Include(x => x.Category).Include(x => x.City).FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task UpdateQuotaAfterSale(int id)
        {
            var _event =await  _context.Events.FindAsync(id);
            _event.Quota = -1;
            _context.Events.Update(_event);
        }
    }
}
