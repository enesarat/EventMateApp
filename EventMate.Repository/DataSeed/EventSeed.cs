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
    public class EventSeed : IEntityTypeConfiguration<Event>
    {
        public void Configure(EntityTypeBuilder<Event> builder)
        {
            builder.HasData(
                new Event { Id = 1, Name = "Rock'n Coke", CreatedDate = DateTime.Now, CreatedBy = "SYSTEM", IsActive = true, Address = "Sample Address", CategoryId = 2, CityId = 1, Description = "Sample Description", Quota = 1500, StartDate = new DateTime(2023, 5, 10), EndDate = new DateTime(2023, 5, 10) },
                new Event { Id = 2, Name = "90'lar Türkçe Pop", CreatedDate = DateTime.Now, CreatedBy = "SYSTEM", IsActive = true, Address = "Sample Address", CategoryId = 2, CityId = 2, Description = "Sample Description", Quota = 1500, StartDate = new DateTime(2023, 5, 10), EndDate = new DateTime(2023, 5, 10) },
                new Event { Id = 3, Name = "Stanley Kubrick Sineması", CreatedDate = DateTime.Now, CreatedBy = "SYSTEM", IsActive = true, Address = "Sample Address", CategoryId = 1, CityId = 4, Description = "Sample Description", Quota = 1500, StartDate = new DateTime(2023, 5, 10), EndDate = new DateTime(2023, 5, 10) },
                new Event { Id = 4, Name = "Istanbul Technology and Innovation Meeting", CreatedDate = DateTime.Now, CreatedBy = "SYSTEM", IsActive = true, Address = "Sample Address", CategoryId = 3, CityId = 1, Description = "Sample Description", Quota = 1500, StartDate = new DateTime(2023, 5, 10), EndDate = new DateTime(2023, 5, 10) },
                new Event { Id = 5, Name = "Tesla'nın Dehası", CreatedDate = DateTime.Now, CreatedBy = "SYSTEM", IsActive = true, Address = "Sample Address", CategoryId = 4, CityId = 3, Description = "Sample Description", Quota = 1500, StartDate = new DateTime(2023, 5, 10), EndDate = new DateTime(2023, 5, 10) },
                new Event { Id = 6, Name = "Chanpions League Finale Istanbul 23", CreatedDate = DateTime.Now, CreatedBy = "SYSTEM", IsActive = true, Address = "Sample Address", CategoryId = 5, CityId = 1, Description = "Sample Description", Quota = 1500, StartDate = new DateTime(2023, 5, 10), EndDate = new DateTime(2023, 5, 10) }
                );
        }
    }
}
