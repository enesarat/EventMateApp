using EventMate.API.Filters;
using EventMate.Core.DTO.Concrete.Category;
using EventMate.Core.DTO.Concrete.Event;
using EventMate.Core.DTO.Concrete.Ticket;
using EventMate.Core.Model.Concrete;
using EventMate.Core.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EventMate.API.Controllers
{

    public class TicketController : CustomBaseController
    {
        private readonly ITicketService _service;
        private readonly IHttpContextAccessor _contextAccessor;

        public TicketController(ITicketService service, IHttpContextAccessor contextAccessor)
        {
            _service = service;
            _contextAccessor = contextAccessor;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            return CustomActionResult(await _service.GetTicketWithDetailAsync(id));
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return CustomActionResult(await _service.GetTicketsWithDetailsAsync());
        }

        [HttpPost]
        public async Task<IActionResult> Create(TicketCreateDto ticketCreateDto)
        {
            return CustomActionResult(await _service.AddAsync(ticketCreateDto, _contextAccessor.HttpContext));
        }
        [HttpGet("VerifyTicket")]
        public async Task<IActionResult> VerifyTicket([FromQuery]string ticketNumber)
        {
            return CustomActionResult(await _service.VerifyTicket(ticketNumber));
        }

        [HttpPut]
        [ServiceFilter(typeof(CreateDateSafetyFilter<Ticket, TicketUpdateDto>))]
        [ServiceFilter(typeof(CreatedBySafetyFilter<Ticket, TicketUpdateDto>))]
        [ServiceFilter(typeof(UpdateTicketNumSafetyFilter<Ticket, TicketUpdateDto>))]
        public async Task<IActionResult> Update([FromBody] TicketUpdateDto ticketUpdateDto)
        {
            return CustomActionResult(await _service.UpdateAsync(ticketUpdateDto));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            return CustomActionResult(await _service.DeleteAsync(id));
        }
    }
}
