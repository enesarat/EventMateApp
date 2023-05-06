using EventMate.Core.DTO.Abstract;
using EventMate.Core.Model.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventMate.Core.DTO.Concrete.Ticket
{
    public class TicketDto : BaseDto
    {
        public string IdentifiedTicketNumber { get; set; }
        public string User { get; set; }
        public string Event { get; set; }
    }
}
