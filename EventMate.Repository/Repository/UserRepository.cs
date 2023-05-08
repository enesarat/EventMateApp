using EventMate.Core.Model.Concrete;
using EventMate.Core.Repository;
using EventMate.Repository.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace EventMate.Repository.Repository
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        public UserRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<IEnumerable<User>> GetUsersWithRole()
        {
            //Eager Loading
            return _context.Users.AsNoTracking().Include(x=>x.Role).AsEnumerable();
        }

        public Task<User> GetUserWithRole(int id)
        {
            //Eager Loading
            return _context.Users.Include(x => x.Role).FirstOrDefaultAsync(x => x.Id == id);
        }
    }
}
