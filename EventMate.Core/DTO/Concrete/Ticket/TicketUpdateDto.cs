using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventMate.Core.DTO.Concrete.Ticket
{
    public class TicketUpdateDto
    {
        public int Id { get; set; }
        public string IdentifiedTicketNumber { get; set; }
        public int UserId { get; set; }
        public int EventId { get; set; }
    }
}
