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
    public class CitySeed : IEntityTypeConfiguration<City>
    {
        public void Configure(EntityTypeBuilder<City> builder)
        {
            builder.HasData(
                new City { Id = 1, Name = "İstenbul", CreatedDate = DateTime.Now, CreatedBy = "SYSTEM", IsActive = true },
                new City { Id = 2, Name = "Ankara", CreatedDate = DateTime.Now, CreatedBy = "SYSTEM", IsActive = true },
                new City { Id = 3, Name = "İzmir", CreatedDate = DateTime.Now, CreatedBy = "SYSTEM", IsActive = true },
                new City { Id = 4, Name = "Samsun", CreatedDate = DateTime.Now, CreatedBy = "SYSTEM", IsActive = true }
                );
        }
    }
}
