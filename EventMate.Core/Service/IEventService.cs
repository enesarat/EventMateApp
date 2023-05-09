using EventMate.Core.DTO.Concrete.Account;
using EventMate.Core.DTO.Concrete.Event;
using EventMate.Core.DTO.Concrete.Response;
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
    public interface IEventService : IGenericService<Event,EventDto>
    {
        public Task<CustomResponse<NoContentResponse>> AddAsync(EventCreateDto eventCreateDto, HttpContext _context);
        public Task<CustomResponse<NoContentResponse>> UpdateAsync(EventUpdateDto eventUpdateDto);
        public Task<CustomResponse<IEnumerable<EventDto>>> GetEventsWithDetailsAsync();
        public Task<CustomResponse<EventDto>> GetEventWithDetailsAsync(int id);
        Task<ActiveAccountDto> GetCurrentAccount(HttpContext _context);
        Task<CustomResponse<List<EventDto>>> GetFilteredEventsAsync(EventFilter filter);

    }
}
