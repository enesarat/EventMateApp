using AutoMapper;
using EventMate.Core.DTO.Concrete.Event;
using EventMate.Core.DTO.Concrete.Response;
using EventMate.Core.DTO.Concrete.Role;
using EventMate.Core.DTO.Concrete.User;
using EventMate.Core.Model.Concrete;
using EventMate.Core.Repository;
using EventMate.Core.Service;
using EventMate.Core.UnitOfWork;
using EventMate.Repository.Repository;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventMate.Service.Service
{
    public class EventService : GenericService<Event, EventDto>, IEventService
    {
        private readonly IEventRepository _eventRepository;
        public EventService(IGenericRepository<Event> repository, IUnitOfWork unitOfWork, IMapper mapper, IEventRepository eventRepository) : base(repository, unitOfWork, mapper)
        {
            _eventRepository = eventRepository;
        }

        public async Task<CustomResponse<NoContentResponse>> AddAsync(EventCreateDto eventCreateDto)
        {
            var item = _mapper.Map<Event>(eventCreateDto);
            item.CreatedDate = DateTime.Now;
            item.CreatedBy = "SYSTEM";
            await _eventRepository.AddAsync(item);
            await _unitOfWork.CommitAsync();

            return CustomResponse<NoContentResponse>.Success(StatusCodes.Status204NoContent);
        }

        public async Task<CustomResponse<IEnumerable<EventDto>>> GetEventsWithDetailsAsync()
        {
            var entities = await _eventRepository.GetEventsWithRole();
            var entitiesQueryable = entities.ToList().AsQueryable();
            var activeEntities = entitiesQueryable.Where(x => x.IsActive == true);

            var entitiesAsDto = _mapper.Map<IEnumerable<EventDto>>(activeEntities);
            return CustomResponse<IEnumerable<EventDto>>.Success(StatusCodes.Status200OK, entitiesAsDto);
        }

        public async Task<CustomResponse<EventDto>> GetEventWithDetailsAsync(int id)
        {
            var entity = await _eventRepository.GetEventWithRole(id);
            if (entity != null && entity.IsActive != false)
            {
                var entityAsDto = _mapper.Map<EventDto>(entity);

                return CustomResponse<EventDto>.Success(StatusCodes.Status200OK, entityAsDto);
            }
            return CustomResponse<EventDto>.Fail(StatusCodes.Status404NotFound, $" {typeof(Event).Name} ({id}) not found. Retrieve operation is not successfull. ");
        }

        public async Task<CustomResponse<NoContentResponse>> UpdateAsync(EventUpdateDto eventUpdateDto)
        {
            if (await _eventRepository.AnyAsync(x => x.Id == eventUpdateDto.Id && x.IsActive == true))
            {
                var entity = _mapper.Map<Event>(eventUpdateDto);

                entity.UpdatedDate = DateTime.Now;
                _eventRepository.Update(entity);
                await _unitOfWork.CommitAsync();
                return CustomResponse<NoContentResponse>.Success(StatusCodes.Status204NoContent);
            }
            return CustomResponse<NoContentResponse>.Fail(StatusCodes.Status404NotFound, $" {typeof(Category).Name} ({eventUpdateDto.Id}) not found. Updete operation is not successfull. ");
        }
    
    }
}
