using EventMate.Core.Model.Concrete;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventMate.Repository.DataSeed
{
    public class RoleSeed : IEntityTypeConfiguration<Role>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Role> builder)
        {
            builder.HasData(
                new Role { Id = 1, Name = "Admin", CreatedDate = DateTime.Now, CreatedBy = "SYSTEM", IsActive = true},
                new Role { Id = 2, Name = "Personnel", CreatedDate = DateTime.Now, CreatedBy = "SYSTEM", IsActive = true },
                new Role { Id = 3, Name = "Paticipant", CreatedDate = DateTime.Now, CreatedBy = "SYSTEM", IsActive = true }
              );
        }
    }
}
