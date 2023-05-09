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
                new Ticket { Id = 1, CreatedDate = DateTime.Now, CreatedBy = "SYSTEM", IsActive = true, EventId = 1, IdentifiedTicketNumber = $"TESSTR-ROCK'8927E-13B0C", UserId = 2 },
                new Ticket { Id = 2, CreatedDate = DateTime.Now, CreatedBy = "SYSTEM", IsActive = true, EventId = 3, IdentifiedTicketNumber = $"TESSTR-STANLFB1CF-BDE30", UserId = 2 },
                new Ticket { Id = 3, CreatedDate = DateTime.Now, CreatedBy = "SYSTEM", IsActive = true, EventId = 6, IdentifiedTicketNumber = $"TESSTR-CHANP25173-BAE4F", UserId = 2 },
                new Ticket { Id = 4, CreatedDate = DateTime.Now, CreatedBy = "SYSTEM", IsActive = true, EventId = 6, IdentifiedTicketNumber = $"TESSTR-CHANPBB1B8-7A99E", UserId = 3 }
              );
        }
    }
}
