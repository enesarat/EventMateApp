using AutoMapper;
using EventMate.Core.DTO.Concrete.Category;
using EventMate.Core.Model.Concrete;
using EventMate.Core.Service;
using EventMate.Core.UnitOfWork;
using EventMate.Repository.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EventMate.Web.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ICategoryService _service;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ApplicationDbContext _dbContext;
        private readonly IMapper _mapper;
        public CategoryController(ICategoryService service, IUnitOfWork unitOfWork, ApplicationDbContext dbContext, IMapper mapper)
        {
            _service = service;
            _unitOfWork = unitOfWork;
            _dbContext = dbContext;
            _mapper = mapper;
        }

        // GET: /Category
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var categories = await _service.GetAllAsync();
            var categoriesDto = _mapper.Map<List<CategoryDto>>(categories.Data);
            return View(categoriesDto);
        }

        // GET: /Category/Details/5
        [HttpGet]
        public async Task<IActionResult> Details(int id)
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

        // GET: /Category/Create
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        // POST: /Category/Create
        [HttpPost]
        public async Task<IActionResult> Create(CategoryCreateDto categoryDto)
        {
            if (!ModelState.IsValid)
            {
                return View(categoryDto);
            }
            await _service.AddAsync(categoryDto);
            await _unitOfWork.CommitAsync();

            return RedirectToAction(nameof(Index));
        }

        // GET: /Category/Edit/5
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _service.GetByIdAsync(id);
            if (category.Data == null)
            {
                return NotFound();
            }
            var categoryDto = _mapper.Map<CategoryDto>(category.Data);
            return View(categoryDto);
        }
        
        // POST: /Category/Edit/5
        [HttpPost]
        public async Task<IActionResult> Edit(CategoryDto category)
        {
            if (!ModelState.IsValid)
            {
                return View(category);
            }
            await _service.UpdateAsync(category);
            await _unitOfWork.CommitAsync();

            return RedirectToAction(nameof(Index));
        }

        // GET: /Category/Delete/5
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

        // POST: /Category/Delete/5
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var category = await _service.GetByIdAsync(id);
            await _service.DeleteAsync(category.Data.Id);
            await _unitOfWork.CommitAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
