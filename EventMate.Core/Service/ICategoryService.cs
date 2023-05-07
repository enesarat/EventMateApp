using EventMate.Core.DTO.Concrete.Category;
using EventMate.Core.DTO.Concrete.Response;
using EventMate.Core.Model.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventMate.Core.Service
{
    public interface ICategoryService : IGenericService<Category,CategoryDto>
    {
        public Task<CustomResponse<NoContentResponse>> AddAsync(CategoryCreateDto categoryCreateDto);
        public Task<CustomResponse<NoContentResponse>> UpdateAsync(CategoryUpdateDto categoryUpdateDto);


    }
}
