using EventMate.Core.Model.Concrete;
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
    public class CategoryService : ICategoryService
    {
        private readonly IUnitOfWork _unitOfWork;

        public CategoryService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task AddAsync(Category item)
        {
            item.CreatedDate = DateTime.Now;
            item.CreatedBy = "SystemUser";
            await _unitOfWork.CategoryRepository.AddAsync(item);
            await _unitOfWork.CommitAsync();
        }

        public async Task<bool> AnyAsync(Expression<Func<Category, bool>> expression)
        {
            var status = await _unitOfWork.CategoryRepository.AnyAsync(expression);
            return status;
        }

        public async Task DeleteAsync(int id)
        {
            var item =_unitOfWork.CategoryRepository.GetByIdAsync(id).Result;
            _unitOfWork.CategoryRepository.Delete(item);
            await _unitOfWork.CommitAsync();
        }

        public async Task<IEnumerable<Category>> GetAllAsync()
        {
            var itemList = await _unitOfWork.CategoryRepository.GetAllAsync();
            return itemList;
        }

        public async Task<Category> GetByIdAsync(int id)
        {
            var item = await _unitOfWork.CategoryRepository.GetByIdAsync(id);
            return item;
        }

        public async Task UpdateAsync(Category item)
        {
            item.UpdatedDate = DateTime.Now;
            _unitOfWork.CategoryRepository.Update(item);
            await _unitOfWork.CommitAsync();
        }

        public Task<IQueryable<Category>> WhereAsync(Expression<Func<Category, bool>> expression)
        {
            var result = _unitOfWork.CategoryRepository.Where(expression);
            return (Task<IQueryable<Category>>)result;
        }
    }
}
