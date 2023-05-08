using EventMate.API.Filters;
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

        public TicketController(ITicketService service)
        {
            _service = service;
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
            return CustomActionResult(await _service.AddAsync(ticketCreateDto));
        }

        [HttpPut]
        [ServiceFilter(typeof(CreateDateSafetyFilter<Ticket, TicketUpdateDto>))]
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
