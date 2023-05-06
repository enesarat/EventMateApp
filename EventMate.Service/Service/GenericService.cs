using EventMate.Core.Model.Abstract;
using EventMate.Core.Repository;
using EventMate.Core.Service;
using EventMate.Core.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace EventMate.Service.Service
{
    public class GenericService<T> : IGenericService<T> where T : BaseModel
    {
        private readonly IGenericRepository<T> _repository;
        private readonly IUnitOfWork _unitOfWork;
        public GenericService(IGenericRepository<T> repository, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task AddAsync(T entity)
        {
            await _repository.AddAsync(entity);
            await _unitOfWork.CommitAsync();
        }

        public async Task<bool> AnyAsync(Expression<Func<T, bool>> expression)
        {
            var anyEntity = await _repository.AnyAsync(expression);
            return anyEntity;
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await _repository.GetByIdAsync(id);
            _repository.Delete(entity);
            await _unitOfWork.CommitAsync();
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            var entities = await _repository.GetAllAsync();
            var entitiesQueryable = entities.ToList().AsQueryable();
            var activeEntities = entitiesQueryable.Where(x => x.IsActive == true);
            return activeEntities;
        }

        public async Task<T> GetByIdAsync(int id)
        {
            var entity = await _repository.GetByIdAsync(id);
            if (entity.IsActive != false)
            {
                return entity;
            }
            throw new Exception($"{typeof(T).Name} ({id}) not found. Retrieve operation is not successfull. \"");
        }

        public async Task UpdateAsync(T entity)
        {
            if (await _repository.AnyAsync(x => x.Id == entity.Id && x.IsActive == true))
            {
                _repository.Update(entity);
                await _unitOfWork.CommitAsync();
            }
            else
            {
                throw new Exception($" {typeof(T)} ({entity.Id}) not found. Updete operation is not successfull. ");
            }
        }

        public async Task<IQueryable<T>> WhereAsync(Expression<Func<T, bool>> expression)
        {
            var entities = await _repository.Where(expression).ToListAsync();
            return (IQueryable<T>)entities;
        }
    }
}
