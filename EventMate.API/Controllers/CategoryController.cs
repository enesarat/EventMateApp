using AutoMapper;
using EventMate.Core.DTO.Concrete.Category;
using EventMate.Core.DTO.Concrete.Response;
using EventMate.Core.Model.Concrete;
using EventMate.Core.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EventMate.API.Controllers
{
    public class CategoryController : CustomBaseController
    {
        private readonly IMapper _mapper;
        private readonly ICategoryService _service;
        public CategoryController(IMapper mapper, ICategoryService service)
        {
            _mapper = mapper;
            _service = service;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var catgory = await _service.GetByIdAsync(id);
            var catgoryAsDto = _mapper.Map<CategoryDto>(catgory);

            return CustomActionResult(CustomResponse<Category>.Success(200, catgory));
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var catgories = await _service.GetAllAsync();
            var catgoriesAsDto = _mapper.Map<List<CategoryDto>>(catgories.ToList());

            return CustomActionResult(CustomResponse<List<CategoryDto>>.Success(200, catgoriesAsDto));
        }

        [HttpPost]
        public async Task<IActionResult> Create(CategoryCreateDto categoryDto)
        {
            var category = _mapper.Map<Category>(categoryDto);
            await _service.AddAsync(category);

            return CustomActionResult(CustomResponse<CategoryCreateDto>.Success(201, categoryDto));
        }

        [HttpPut]
        public async Task<IActionResult> Update(CategoryUpdateDto categoryDto)
        {
            var category = _mapper.Map<Category>(categoryDto);
            await _service.UpdateAsync(category);

            return CustomActionResult(CustomResponse<NoContentResponse>.Success(204));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var category = _service.GetByIdAsync(id);
            if(category == null)
            {
                return CustomActionResult(CustomResponse<NoContentResponse>.Fail(404, $"{typeof(Category).Name} ({id}) not found. Delete operation is not successfull. "));
            }
            await _service.DeleteAsync(id);

            return CustomActionResult(CustomResponse<NoContentResponse>.Success(204));
        }
    }
}
