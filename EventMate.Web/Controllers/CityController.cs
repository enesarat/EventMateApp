using AutoMapper;
using EventMate.Core.DTO.Concrete.Category;
using EventMate.Core.DTO.Concrete.City;
using EventMate.Core.Model.Concrete;
using EventMate.Core.Service;
using EventMate.Core.UnitOfWork;
using EventMate.Repository.Context;
using Microsoft.AspNetCore.Mvc;

namespace EventMate.Web.Controllers
{
    public class CityController : Controller
    {
        private readonly ICityService _service;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ApplicationDbContext _dbContext;
        private readonly IMapper _mapper;
        public CityController(ICityService service, IUnitOfWork unitOfWork, ApplicationDbContext dbContext, IMapper mapper)
        {
            _service = service;
            _unitOfWork = unitOfWork;
            _dbContext = dbContext;
            _mapper = mapper;
        }

        // GET: /City
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var city = await _service.GetAllAsync();
            var cityDto = _mapper.Map<List<CityDto>>(city.Data);
            return View(cityDto);
        }

        // GET: /City/Details/5
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var city = await _service.GetByIdAsync(id);
            if (city == null)
            {
                return NotFound();
            }

            return View(city.Data);
        }

        // GET: /City/Create
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        // POST: /City/Create
        [HttpPost]
        public async Task<IActionResult> Create(CityCreateDto cityDto)
        {
            if (!ModelState.IsValid)
            {
                return View(cityDto);
            }
            await _service.AddAsync(cityDto);
            await _unitOfWork.CommitAsync();

            return RedirectToAction(nameof(Index));
        }

        // GET: /City/Edit/5
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var city = await _service.GetByIdAsync(id);
            if (city.Data == null)
            {
                return NotFound();
            }
            var cityDto = _mapper.Map<CityDto>(city.Data);
            return View(cityDto);
        }

        // POST: /City/Edit/5
        [HttpPost]
        public async Task<IActionResult> Edit(CityDto city)
        {
            if (!ModelState.IsValid)
            {
                return View(city);
            }
            await _service.UpdateAsync(city);
            await _unitOfWork.CommitAsync();

            return RedirectToAction(nameof(Index));
        }

        // GET: /City/Delete/5
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var city = await _service.GetByIdAsync(id);
            if (city == null)
            {
                return NotFound();
            }

            return View(city.Data);
        }

        // POST: /City/Delete/5
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var city = await _service.GetByIdAsync(id);
            await _service.DeleteAsync(city.Data.Id);
            await _unitOfWork.CommitAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
