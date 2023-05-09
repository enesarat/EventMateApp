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
        private readonly IHttpContextAccessor _contextAccessor;

        public EventController(IEventService service, IHttpContextAccessor contextAccessor)
        {
            _service = service;
            _contextAccessor = contextAccessor;
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
            return CustomActionResult(await _service.AddAsync(eventCreateDto, _contextAccessor.HttpContext));
        }

        [HttpPut]
        [ServiceFilter(typeof(CreateDateSafetyFilter<Event, EventUpdateDto>))]
        [ServiceFilter(typeof(StartDateSafetyFilter<Event, EventUpdateDto>))]
        [ServiceFilter(typeof(EndDateSafetyFilter<Event, EventUpdateDto>))]
        [ServiceFilter(typeof(CreatedBySafetyFilter<Event, EventUpdateDto>))]
        [ServiceFilter(typeof(UpdateEventNameSafetyFilter<Event, EventUpdateDto>))]
        [ServiceFilter(typeof(UpdateEventDescSafetyFilter<Event, EventUpdateDto>))]
        [ServiceFilter(typeof(UpdateCityIdSafetyFilter<Event, EventUpdateDto>))]
        [ServiceFilter(typeof(UpdateCategoryIdSafetyFilter<Event, EventUpdateDto>))]
        [ServiceFilter(typeof(UpdateIsApprSafetyFiltercs<Event, EventUpdateDto>))]
        public async Task<IActionResult> Update([FromBody] EventUpdateDto eventUpdateDto)
        {
            return CustomActionResult(await _service.UpdateAsync(eventUpdateDto));
        }

        [HttpDelete("{id}")]
        [ServiceFilter(typeof(EventStartDateFilter))]
        public async Task<IActionResult> Delete(int id)
        {
            return CustomActionResult(await _service.DeleteAsync(id));
        }
    }
}
