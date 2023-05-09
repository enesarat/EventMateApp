using AutoMapper;
using EventMate.Core.DTO.Concrete.Account;
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
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace EventMate.Service.Service
{
    public class EventService : GenericService<Event, EventDto>, IEventService
    {
        private readonly IEventRepository _eventRepository;
        private readonly IUserRepository _userRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly ICityRepository _cityRepository;
        private readonly IHttpContextAccessor _contextAccessor;
        private string errorMessage =" Validation failed. ";
        public EventService(IGenericRepository<Event> repository, IUnitOfWork unitOfWork, IMapper mapper, IEventRepository eventRepository, IHttpContextAccessor contextAccessor, IUserRepository userRepository, ICategoryRepository categoryRepository, ICityRepository cityRepository) : base(repository, unitOfWork, mapper)
        {
            _eventRepository = eventRepository;
            _contextAccessor = contextAccessor;
            _userRepository = userRepository;
            _categoryRepository = categoryRepository;
            _cityRepository = cityRepository;
        }

        public async Task<CustomResponse<NoContentResponse>> AddAsync(EventCreateDto eventCreateDto, HttpContext _context)
        {
            var status = await EventValidationChecker(eventCreateDto);
            if(status)
            {
                var item = _mapper.Map<Event>(eventCreateDto);
                item.CreatedDate = DateTime.Now;

                string creatorMail = GetCreatorInfo(await GetCurrentAccount(_context));
                if(creatorMail != null)
                {
                    item.CreatedBy = creatorMail;
                    await _eventRepository.AddAsync(item);
                    await _unitOfWork.CommitAsync();

                    return CustomResponse<NoContentResponse>.Success(StatusCodes.Status204NoContent);
                }
                return CustomResponse<NoContentResponse>.Fail(StatusCodes.Status401Unauthorized, $" This user cannot create {typeof(Event).Name} due to role. ");

            }
            return CustomResponse<NoContentResponse>.Fail(StatusCodes.Status400BadRequest, $"{errorMessage}");
        }
        public string GetCreatorInfo(ActiveAccountDto account)
        {
            if (account.Role == "Admin")
            {
                return "SYSTEM";
            }
            else if (account.Role == "Paticipant")
            {
                return account.Email;
            }
            else
            {
                return null;
            }
        }
        public async Task<ActiveAccountDto> GetCurrentAccount(HttpContext _context)
        {
            try
            {
                var identity = _context.User.Identity as ClaimsIdentity;
                var userClaims = identity.Claims;
                var accountEmail = userClaims.FirstOrDefault(o => o.Type == ClaimTypes.Email)?.Value;
                var user = _userRepository.Where(x => x.Email == accountEmail).FirstOrDefault();

                if (user != null && user.RefreshToken != null)
                {
                    ActiveAccountDto currentaccount = new ActiveAccountDto
                    {
                        Email = userClaims.FirstOrDefault(o => o.Type == ClaimTypes.Email)?.Value,
                        Name = userClaims.FirstOrDefault(o => o.Type == ClaimTypes.Name)?.Value,
                        Surname = userClaims.FirstOrDefault(o => o.Type == ClaimTypes.Surname)?.Value,
                        Role = userClaims.FirstOrDefault(o => o.Type == ClaimTypes.Role)?.Value

                    };
                    return currentaccount;
                }
                else
                    throw new InvalidOperationException("Could not access active user information.");
            }
            catch (Exception)
            {

                throw new InvalidOperationException(" You must be in an active session to perform the operation. ");
            }
            
        }
        private async Task<bool> EventValidationChecker(EventCreateDto eventCreateDto)
        {
            var nameValidator = await _eventRepository.AnyAsync(x => x.Name == eventCreateDto.Name);
            if (nameValidator) {
                errorMessage = errorMessage + " There is another event with this name. ";  
            }
            var categoryValidator = await _categoryRepository.AnyAsync(x => x.Id == eventCreateDto.CategoryId);
            if (!categoryValidator)
            {
                errorMessage = errorMessage + " No such category found. ";
            }
            var cityValidator = await _cityRepository.AnyAsync(x => x.Id == eventCreateDto.CityId);
            if (!cityValidator)
            {
                errorMessage = errorMessage + " No such city was found. ";
            }

            if (nameValidator==false&&categoryValidator==true&&cityValidator==true)
            {
                return true;
            }
            return false;
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
            return CustomResponse<NoContentResponse>.Fail(StatusCodes.Status404NotFound, $" {typeof(Event).Name} ({eventUpdateDto.Id}) not found. Updete operation is not successfull. ");
        }
    
    }
}
