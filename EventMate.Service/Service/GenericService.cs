using AutoMapper;
using EventMate.Core.DTO.Concrete.Response;
using EventMate.Core.Model.Abstract;
using EventMate.Core.Repository;
using EventMate.Core.Service;
using EventMate.Core.UnitOfWork;
using EventMate.Service.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace EventMate.Service.Service
{
    public class GenericService<Entity, Dto> : IGenericService<Entity, Dto> where Entity : BaseModel where Dto : class
    {
        private readonly IGenericRepository<Entity> _repository;
        protected readonly IUnitOfWork _unitOfWork;
        protected readonly IMapper _mapper;

        public GenericService(IGenericRepository<Entity> repository, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<CustomResponse<NoContentResponse>> AddAsync(Dto item)
        {
            Entity newEntity = _mapper.Map<Entity>(item);

            await _repository.AddAsync(newEntity);
            await _unitOfWork.CommitAsync();

            var newDto = _mapper.Map<Dto>(newEntity);
            return CustomResponse<NoContentResponse>.Success(StatusCodes.Status204NoContent);
        }

        public async Task<CustomResponse<bool>> AnyAsync(Expression<Func<Entity, bool>> expression)
        {
            var anyEntity = await _repository.AnyAsync(expression);

            return CustomResponse<bool>.Success(StatusCodes.Status200OK, anyEntity);
        }

        public async Task<CustomResponse<NoContentResponse>> DeleteAsync(int id)
        {

            var entity = await _repository.GetByIdAsync(id);
            if (entity != null && entity.IsActive != false)
            {
                _repository.Delete(entity);
                await _unitOfWork.CommitAsync();

                return CustomResponse<NoContentResponse>.Success(StatusCodes.Status204NoContent);
            }
            return CustomResponse<NoContentResponse>.Fail(StatusCodes.Status404NotFound, $" {typeof(Entity).Name} ({id}) not found. Delete operation is not successfull. ");
        }

        public async Task<CustomResponse<IEnumerable<Dto>>> GetAllAsync()
        {
            var entities = await _repository.GetAllAsync();
            var entitiesQueryable = entities.ToList().AsQueryable();
            var activeEntities = entitiesQueryable.Where(x => x.IsActive == true);

            var dtos = _mapper.Map<IEnumerable<Dto>>(activeEntities);
            return CustomResponse<IEnumerable<Dto>>.Success(StatusCodes.Status200OK, dtos);
        }

        public async Task<CustomResponse<Dto>> GetByIdAsync(int id)
        {
            var entity = await _repository.GetByIdAsync(id);
            if (entity!=null && entity.IsActive != false)
            {
                var dto = _mapper.Map<Dto>(entity);

                return CustomResponse<Dto>.Success(StatusCodes.Status200OK, dto);
            }
            return CustomResponse<Dto>.Fail(StatusCodes.Status404NotFound, $" {typeof(Entity).Name} ({id}) not found. Retrieve operation is not successfull. ");
        }

        public async Task<CustomResponse<NoContentResponse>> UpdateAsync(Dto item)
        {
            var entity = _mapper.Map<Entity>(item);
            if (await _repository.AnyAsync(x => x.Id == entity.Id && x.IsActive == true))
            {
                _repository.Update(entity);
                await _unitOfWork.CommitAsync();
                return CustomResponse<NoContentResponse>.Success(StatusCodes.Status204NoContent);
            }
            return CustomResponse<NoContentResponse>.Fail(StatusCodes.Status404NotFound, $" {typeof(Entity)} ({entity.Id}) not found. Updete operation is not successfull. ");
        }

        public async Task<CustomResponse<IEnumerable<Dto>>> Where(Expression<Func<Entity, bool>> expression)
        {
            var entities = await _repository.Where(expression).ToListAsync();
            var dtos = _mapper.Map<IEnumerable<Dto>>(entities);

            return CustomResponse<IEnumerable<Dto>>.Success(StatusCodes.Status200OK, dtos);
        }
    }
}
