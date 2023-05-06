using EventMate.Core.Model.Concrete;
using EventMate.Core.Repository;
using EventMate.Core.UnitOfWork;
using EventMate.Repository.Context;
using EventMate.Repository.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventMate.Repository.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext dbContext;
        public bool disposed;

        public IGenericRepository<Category> CategoryRepository { get; private set; }
        public IGenericRepository<City> CityRepository { get; private set; }
        public IGenericRepository<Event> EventRepository { get; private set; }
        public IGenericRepository<Ticket> TicketRepository { get; private set; }
        public IGenericRepository<Role> RoleRepository { get; private set; }
        public IGenericRepository<User> UserRepository { get; private set; }

        public UnitOfWork(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;

            CategoryRepository = new GenericRepository<Category>(dbContext);
            CityRepository = new GenericRepository<City>(dbContext);
            EventRepository = new GenericRepository<Event>(dbContext);
            TicketRepository = new GenericRepository<Ticket>(dbContext);
            RoleRepository = new GenericRepository<Role>(dbContext);
            UserRepository = new GenericRepository<User>(dbContext);
        }


        public void Commit()
        {
            using (var dbContextTransction = dbContext.Database.BeginTransaction())
            {
                try
                {
                    dbContext.SaveChanges();
                    dbContextTransction.Commit();
                }
                catch (Exception ex)
                {
                    // logging
                    dbContextTransction.Rollback();
                }
            }
        }

        public async Task CommitAsync()
        {
            using (var dbContextTransction = dbContext.Database.BeginTransaction())
            {
                try
                {
                    await dbContext.SaveChangesAsync();
                    dbContextTransction.Commit();
                }
                catch (Exception ex)
                {
                    // logging
                    dbContextTransction.Rollback();
                }
            }
        }

        protected virtual void Clean(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    dbContext.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Clean(true);
            GC.SuppressFinalize(this);
        }
    }
}
