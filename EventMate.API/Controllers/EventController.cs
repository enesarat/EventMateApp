using EventMate.API.Filters;
using EventMate.Core.DTO.Concrete.Event;
using EventMate.Core.DTO.Concrete.User;
using EventMate.Core.Model.Concrete;
using EventMate.Core.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EventMate.API.Controllers
{

    public class EventController : CustomBaseController
    {
        private readonly IEventService _service;

        public EventController(IEventService service)
        {
            _service = service;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            return CustomActionResult(await _service.GetEventWithDetailsAsync(id));
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return CustomActionResult(await _service.GetEventsWithDetailsAsync());
        }

        [HttpPost]
        public async Task<IActionResult> Create(EventCreateDto eventCreateDto)
        {
            return CustomActionResult(await _service.AddAsync(eventCreateDto));
        }

        [HttpPut]
        [ServiceFilter(typeof(CreateDateSafetyFilter<Event, EventUpdateDto>))]
        public async Task<IActionResult> Update([FromBody] EventUpdateDto eventUpdateDto)
        {
            return CustomActionResult(await _service.UpdateAsync(eventUpdateDto));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            return CustomActionResult(await _service.DeleteAsync(id));
        }
    }
}
