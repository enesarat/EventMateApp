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
    public class TicketSeed : IEntityTypeConfiguration<Ticket>
    {
        public void Configure(EntityTypeBuilder<Ticket> builder)
        {
            builder.HasData(
                new Ticket { Id = 1, CreatedDate = DateTime.Now, CreatedBy = "SYSTEM", IsActive = true, EventId = 1, IdentifiedTicketNumber = $"EA_060522_01_{Guid.NewGuid().ToString()}", UserId = 2 },
                new Ticket { Id = 2, CreatedDate = DateTime.Now, CreatedBy = "SYSTEM", IsActive = true, EventId = 3, IdentifiedTicketNumber = $"EA_060522_01_{Guid.NewGuid().ToString()}", UserId = 2 },
                new Ticket { Id = 3, CreatedDate = DateTime.Now, CreatedBy = "SYSTEM", IsActive = true, EventId = 6, IdentifiedTicketNumber = $"EA_060522_01_{Guid.NewGuid().ToString()}", UserId = 2 },
                new Ticket { Id = 4, CreatedDate = DateTime.Now, CreatedBy = "SYSTEM", IsActive = true, EventId = 6, IdentifiedTicketNumber = $"EA_060522_01_{Guid.NewGuid().ToString()}", UserId = 3 }
              );
        }
    }
}
