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
    public class TicketRepository : GenericRepository<Ticket>, ITicketRepository
    {
        public TicketRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<IEnumerable<Ticket>> GetTicketsWithDetails()
        {
            //Eager Loading
            return _context.Ticket.AsNoTracking().Include(x => x.User).Include(x => x.Event).AsEnumerable();
        }

        public Task<Ticket> GetTicketWithDetails(int id)
        {
            return _context.Ticket.Include(x => x.User).Include(x => x.Event).FirstOrDefaultAsync(x => x.Id == id);
        }
    }
}
