using EventMate.Core.Model.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventMate.Core.Repository
{
    public interface IEventRepository : IGenericRepository<Event>
    {
        Task<IEnumerable<Event>> GetEventsWithDetails();
        Task<Event> GetEventWithDetails(int id);
        Task UpdateQuotaAfterSale(int id);
    }
}
