using AutoMapper;
using EventMate.Core.DTO.Concrete.Category;
using EventMate.Core.DTO.Concrete.Ticket;
using EventMate.Core.Model.Concrete;
using EventMate.Core.Service;
using EventMate.Core.UnitOfWork;
using EventMate.Repository.Context;
using Microsoft.AspNetCore.Mvc;
using System.Net.Sockets;

namespace EventMate.Web.Controllers
{
    public class TicketController : Controller
    {
        private readonly ITicketService _service;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ApplicationDbContext _dbContext;
        private readonly IMapper _mapper;
        public TicketController(ITicketService service, IUnitOfWork unitOfWork, ApplicationDbContext dbContext, IMapper mapper)
        {
            _service = service;
            _unitOfWork = unitOfWork;
            _dbContext = dbContext;
            _mapper = mapper;
        }

        // GET: /Ticket
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var tickets = await _service.GetAllAsync();
            var ticketsDto = _mapper.Map<List<TicketDto>>(tickets.Data);
            return View(ticketsDto);
        }

        // GET: /Ticket/Details/5
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ticket = await _service.GetByIdAsync(id);
            if (ticket == null)
            {
                return NotFound();
            }

            return View(ticket.Data);
        }

        // GET: /Ticket/Create
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        // POST: /Ticket/Create
        [HttpPost]
        public async Task<IActionResult> Create(TicketDto ticketDto)
        {
            if (!ModelState.IsValid)
            {
                return View(ticketDto);
            }
            await _service.AddAsync(ticketDto);
            await _unitOfWork.CommitAsync();

            return RedirectToAction(nameof(Index));
        }

        // GET: /Ticket/Edit/5
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ticket = await _service.GetByIdAsync(id);
            if (ticket.Data == null)
            {
                return NotFound();
            }
            var ticketDto = _mapper.Map<TicketDto>(ticket.Data);
            return View(ticketDto);
        }

        // POST: /Ticket/Edit/5
        [HttpPost]
        public async Task<IActionResult> Edit(TicketDto ticket)
        {
            if (!ModelState.IsValid)
            {
                return View(ticket);
            }
            await _service.UpdateAsync(ticket);
            await _unitOfWork.CommitAsync();

            return RedirectToAction(nameof(Index));
        }

        // GET: /Ticket/Delete/5
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _service.GetByIdAsync(id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category.Data);
        }

        // POST: /Ticket/Delete/5
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var ticket = await _service.GetByIdAsync(id);
            await _service.DeleteAsync(ticket.Data.Id);
            await _unitOfWork.CommitAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
