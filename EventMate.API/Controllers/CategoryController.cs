using AutoMapper;
using EventMate.API.Filters;
using EventMate.Core.DTO.Concrete.Category;
using EventMate.Core.DTO.Concrete.Response;
using EventMate.Core.DTO.Concrete.User;
using EventMate.Core.Model.Concrete;
using EventMate.Core.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EventMate.API.Controllers
{
    public class CategoryController : CustomBaseController
    {
        private readonly ICategoryService _service;
        private readonly IHttpContextAccessor _contextAccessor;

        public CategoryController(ICategoryService service, IHttpContextAccessor contextAccessor)
        {
            _service = service;
            _contextAccessor = contextAccessor;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            return CustomActionResult(await _service.GetByIdAsync(id));
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return CustomActionResult(await _service.GetAllAsync());
        }

        [HttpPost]
        public async Task<IActionResult> Create(CategoryCreateDto categoryDto)
        {
            return CustomActionResult(await _service.AddAsync(categoryDto));
        }

        [HttpPut]
        [ServiceFilter(typeof(CreateDateSafetyFilter<Category,CategoryUpdateDto>))]
        [ServiceFilter(typeof(CreatedBySafetyFilter<Category, CategoryUpdateDto>))]
        public async Task<IActionResult> Update([FromBody] CategoryUpdateDto categoryDto)
        {
            return CustomActionResult(await _service.UpdateAsync(categoryDto));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            return CustomActionResult(await _service.DeleteAsync(id));
        }
    }
}
