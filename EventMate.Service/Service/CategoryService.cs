using AutoMapper;
using EventMate.Core.DTO.Concrete.Category;
using EventMate.Core.DTO.Concrete.Response;
using EventMate.Core.Model.Concrete;
using EventMate.Core.Repository;
using EventMate.Core.Service;
using EventMate.Core.UnitOfWork;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace EventMate.Service.Service
{
    public class CategoryService : GenericService<Category,CategoryDto>, ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoryService(IGenericRepository<Category> repository, IUnitOfWork unitOfWork, IMapper mapper, ICategoryRepository categoryRepository) : base(repository, unitOfWork, mapper)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<CustomResponse<NoContentResponse>> AddAsync(CategoryCreateDto categoryCreateDto)
        {
            var item = _mapper.Map<Category>(categoryCreateDto);
            item.CreatedDate = DateTime.Now;
            item.CreatedBy = "SYSTEM";
            await _categoryRepository.AddAsync(item);
            await _unitOfWork.CommitAsync();

            return CustomResponse<NoContentResponse>.Success(StatusCodes.Status204NoContent);
        }

        public async Task<CustomResponse<NoContentResponse>> UpdateAsync(CategoryUpdateDto categoryUpdateDto)
        {
            var entity = _mapper.Map<Category>(categoryUpdateDto);
            if (await _categoryRepository.AnyAsync(x => x.Id == entity.Id && x.IsActive == true))
            {
                _categoryRepository.Update(entity);
                await _unitOfWork.CommitAsync();
                return CustomResponse<NoContentResponse>.Success(StatusCodes.Status204NoContent);
            }
            return CustomResponse<NoContentResponse>.Fail(StatusCodes.Status404NotFound, $" {typeof(Category).Name} ({entity.Id}) not found. Updete operation is not successfull. ");
        }
    }
}
