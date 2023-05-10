using EventMate.Core.DTO.Concrete.Account;
using EventMate.Core.DTO.Concrete.Response;
using EventMate.Core.DTO.Concrete.Ticket;
using EventMate.Core.DTO.Concrete.User;
using EventMate.Core.Model.Concrete;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventMate.Core.Service
{
    public interface ITicketService : IGenericService<Ticket,TicketDto>
    {
        public Task<CustomResponse<NoContentResponse>> AddAsync(TicketCreateDto ticketCreateDto, HttpContext _context);
        public Task<CustomResponse<NoContentResponse>> UpdateAsync(TicketUpdateDto ticketUpdateDto);
        public Task<CustomResponse<IEnumerable<TicketDto>>> GetTicketsWithDetailsAsync();
        public Task<CustomResponse<TicketDto>> GetTicketWithDetailAsync(int id);
        Task<ActiveAccountDto> GetCurrentAccount(HttpContext context);
        Task<CustomResponse<TicketVerifyDto>> VerifyTicket(string ticketNumber);
    }
}
