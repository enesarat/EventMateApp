using EventMate.Core.Model.Concrete;
using EventMate.Core.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace EventMate.Core.UnitOfWork
{
    public interface IUnitOfWork
    {
        IGenericRepository<Category> CategoryRepository { get; }
        IGenericRepository<City> CityRepository { get; }
        IGenericRepository<Event> EventRepository { get; }
        IGenericRepository<Role> RoleRepository { get; }
        IGenericRepository<Ticket> TicketRepository { get; }
        IGenericRepository<User> UserRepository { get; }
        Task CommitAsync();
        void Commit();
    }
}
