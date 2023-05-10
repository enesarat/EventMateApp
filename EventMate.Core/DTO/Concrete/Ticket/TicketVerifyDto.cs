using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventMate.Core.DTO.Concrete.Ticket
{
    public class TicketVerifyDto
    {
        public bool TicketValidity { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
    }
}
