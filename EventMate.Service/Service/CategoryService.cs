using EventMate.Core.Model.Concrete;
using EventMate.Core.Repository;
using EventMate.Core.Service;
using EventMate.Core.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace EventMate.Service.Service
{
    public class CategoryService : GenericService<Category>, ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoryService(IGenericRepository<Category> repository, IUnitOfWork unitOfWork, ICategoryRepository categoryRepository) : base(repository, unitOfWork)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task AddAsync(Category item)
        {
            item.CreatedDate = DateTime.Now;
            item.CreatedBy = "SystemUser";
            await _categoryRepository.AddAsync(item);
            await _unitOfWork.CommitAsync();
        }

        
    }
}
