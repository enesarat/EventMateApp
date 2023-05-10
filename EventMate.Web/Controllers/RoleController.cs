using AutoMapper;
using EventMate.Core.DTO.Concrete.City;
using EventMate.Core.DTO.Concrete.Role;
using EventMate.Core.Model.Concrete;
using EventMate.Core.Service;
using EventMate.Core.UnitOfWork;
using EventMate.Repository.Context;
using Microsoft.AspNetCore.Mvc;

namespace EventMate.Web.Controllers
{
    public class RoleController : Controller
    {
        private readonly IRoleService _service;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ApplicationDbContext _dbContext;
        private readonly IMapper _mapper;
        public RoleController(IRoleService service, IUnitOfWork unitOfWork, ApplicationDbContext dbContext, IMapper mapper)
        {
            _service = service;
            _unitOfWork = unitOfWork;
            _dbContext = dbContext;
            _mapper = mapper;
        }

        // GET: /Role
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var role = await _service.GetAllAsync();
            var roleDto = _mapper.Map<List<RoleDto>>(role.Data);
            return View(roleDto);
        }

        // GET: /Role/Details/5
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var role = await _service.GetByIdAsync(id);
            if (role == null)
            {
                return NotFound();
            }

            return View(role.Data);
        }

        // GET: /Role/Create
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        // POST: /Role/Create
        [HttpPost]
        public async Task<IActionResult> Create(RoleCreateDto roleDto)
        {
            if (!ModelState.IsValid)
            {
                return View(roleDto);
            }
            await _service.AddAsync(roleDto);
            await _unitOfWork.CommitAsync();

            return RedirectToAction(nameof(Index));
        }

        // GET: /Role/Edit/5
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var role = await _service.GetByIdAsync(id);
            if (role.Data == null)
            {
                return NotFound();
            }
            var roleDto = _mapper.Map<RoleDto>(role.Data);
            return View(roleDto);
        }

        // POST: /Role/Edit/5
        [HttpPost]
        public async Task<IActionResult> Edit(RoleDto role)
        {
            if (!ModelState.IsValid)
            {
                return View(role);
            }
            await _service.UpdateAsync(role);
            await _unitOfWork.CommitAsync();

            return RedirectToAction(nameof(Index));
        }

        // GET: /Role/Delete/5
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var role = await _service.GetByIdAsync(id);
            if (role == null)
            {
                return NotFound();
            }

            return View(role.Data);
        }

        // POST: /Role/Delete/5
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var role = await _service.GetByIdAsync(id);
            await _service.DeleteAsync(role.Data.Id);
            await _unitOfWork.CommitAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
