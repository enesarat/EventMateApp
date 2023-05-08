using EventMate.Core.DTO.Concrete.Event;
using EventMate.Core.DTO.Concrete.Response;
using EventMate.Core.DTO.Concrete.User;
using EventMate.Core.Model.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventMate.Core.Service
{
    public interface IEventService : IGenericService<Event,EventDto>
    {
        public Task<CustomResponse<NoContentResponse>> AddAsync(EventCreateDto eventCreateDto);
        public Task<CustomResponse<NoContentResponse>> UpdateAsync(EventUpdateDto eventUpdateDto);
        public Task<CustomResponse<IEnumerable<EventDto>>> GetEventsWithDetailsAsync();
        public Task<CustomResponse<EventDto>> GetEventWithDetailsAsync(int id);
    }
}
