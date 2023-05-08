using EventMate.Core.DTO.Concrete.Response;
using EventMate.Core.DTO.Concrete.Ticket;
using EventMate.Core.DTO.Concrete.User;
using EventMate.Core.Model.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventMate.Core.Service
{
    public interface ITicketService : IGenericService<Ticket,TicketDto>
    {
        public Task<CustomResponse<NoContentResponse>> AddAsync(TicketCreateDto ticketCreateDto);
        public Task<CustomResponse<NoContentResponse>> UpdateAsync(TicketUpdateDto ticketUpdateDto);
        public Task<CustomResponse<IEnumerable<TicketDto>>> GetTicketsWithDetailsAsync();
        public Task<CustomResponse<TicketDto>> GetTicketWithDetailAsync(int id);
    }
}
