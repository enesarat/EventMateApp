using EventMate.Core.Model.Concrete;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventMate.Repository.DataSeed
{
    public class UserSeed : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasData(
                new User { Id = 1, CreatedDate = DateTime.Now, CreatedBy = "SYSTEM", IsActive = true, Name = "Jack", Surname = "Sparrow", Email = "blackPearl@gmail.com", Password = "IMJD2023!*", RoleId = 1 },
                new User { Id = 2, CreatedDate = DateTime.Now, CreatedBy = "SYSTEM", IsActive = true, Name = "Enes", Surname = "Arat", Email = "enesArat@gmail.com", Password = "EA2023!*", RoleId = 3 },
                new User { Id = 3, CreatedDate = DateTime.Now, CreatedBy = "SYSTEM", IsActive = true, Name = "Eren", Surname = "Arat", Email = "erenArat@gmail.com", Password = "EA2023!*", RoleId = 3 }
              );
        }
    }
}
