using AutoMapper;
using EventMate.Core.DTO.Concrete.Account;
using EventMate.Core.DTO.Concrete.Event;
using EventMate.Core.DTO.Concrete.Response;
using EventMate.Core.DTO.Concrete.Ticket;
using EventMate.Core.DTO.Concrete.User;
using EventMate.Core.Model.Concrete;
using EventMate.Core.Repository;
using EventMate.Core.Service;
using EventMate.Core.UnitOfWork;
using EventMate.Repository.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace EventMate.Service.Service
{
    public class TicketService : GenericService<Ticket, TicketDto>, ITicketService
    {
        private readonly ITicketRepository _ticketRepository;
        private readonly IUserRepository _userRepository;
        private readonly IEventRepository _eventRepository;
        private string errorMessage = " Validation failed. ";

        public TicketService(IGenericRepository<Ticket> repository, IUnitOfWork unitOfWork, IMapper mapper, ITicketRepository ticketRepository, IUserRepository userRepository, IEventRepository eventRepository) : base(repository, unitOfWork, mapper)
        {
            _ticketRepository = ticketRepository;
            _userRepository = userRepository;
            _eventRepository = eventRepository;
        }

        public async Task<CustomResponse<NoContentResponse>> AddAsync(TicketCreateDto ticketCreateDto, HttpContext _context)
        {
            var status = await TicketValidationChecker(ticketCreateDto);
            if (status)
            {
                var item = _mapper.Map<Ticket>(ticketCreateDto);
                item.CreatedDate = DateTime.Now;

                string creatorMail = GetCreatorInfo(await GetCurrentAccount(_context));
                if(creatorMail != null)
                {
                    var userTemp = _userRepository.Where(x=>x.Email== creatorMail).FirstOrDefault();
                    var eventTemp = _eventRepository.Where(x=>x.Id==ticketCreateDto.EventId).FirstOrDefault();
                    item.CreatedBy = creatorMail;
                    item.IdentifiedTicketNumber = GenerateUniqueTicketNumber(userTemp, eventTemp);
                    await _ticketRepository.AddAsync(item);
                    await _eventRepository.UpdateQuotaAfterSale(ticketCreateDto.EventId);
                    await _unitOfWork.CommitAsync();

                    return CustomResponse<NoContentResponse>.Success(StatusCodes.Status204NoContent);
                }
                return CustomResponse<NoContentResponse>.Fail(StatusCodes.Status401Unauthorized, $" This user cannot create {typeof(Ticket).Name} due to role. ");
            }
            return CustomResponse<NoContentResponse>.Fail(StatusCodes.Status400BadRequest, $"{errorMessage}");
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
        public static string GenerateUniqueTicketNumber(User user, Event _event)
        {
            string userToken = $"{user.Name.Substring(0, 3)}{user.Surname.Substring(0, 3)}".ToUpper();
            string eventToken = $"{_event.Name.Substring(0, 5)}{Guid.NewGuid().ToString().Substring(0, 5).ToUpper()}".ToUpper();
            string randomToken = Guid.NewGuid().ToString().Substring(0, 5).ToUpper();
            return $"{userToken}-{eventToken}-{randomToken}";
        }
        private string GetCreatorInfo(ActiveAccountDto account)
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

        private async Task<bool> TicketValidationChecker(TicketCreateDto ticketCreateDto)
        {
            var ticketValidator = await _ticketRepository.AnyAsync(x => x.IdentifiedTicketNumber == ticketCreateDto.IdentifiedTicketNumber);
            if (ticketValidator)
            {
                errorMessage = errorMessage + " The ticket number has already been bought. ";
            }
            var eventValidator = await _eventRepository.AnyAsync(x => x.Id == ticketCreateDto.EventId);
            if (!eventValidator)
            {
                errorMessage = errorMessage + " No such event found. ";
            }
            var userValidator = await _userRepository.AnyAsync(x => x.Id == ticketCreateDto.UserId);
            if (!userValidator)
            {
                errorMessage = errorMessage + " No such user found. ";
            }
           

            if (ticketValidator == false && eventValidator == true && userValidator == true)
            {
                return true;
            }
            return false;
        }

        public async Task<CustomResponse<IEnumerable<TicketDto>>> GetTicketsWithDetailsAsync()
        {
            var entities = await _ticketRepository.GetTicketsWithDetails();
            var entitiesQueryable = entities.ToList().AsQueryable();
            var activeEntities = entitiesQueryable.Where(x => x.IsActive == true);

            var entitiesAsDto = _mapper.Map<IEnumerable<TicketDto>>(activeEntities);
            return CustomResponse<IEnumerable<TicketDto>>.Success(StatusCodes.Status200OK, entitiesAsDto);
        }

        public async Task<CustomResponse<TicketDto>> GetTicketWithDetailAsync(int id)
        {
            var entity = await _ticketRepository.GetTicketWithDetails(id);
            if (entity != null && entity.IsActive != false)
            {
                var entityAsDto = _mapper.Map<TicketDto>(entity);

                return CustomResponse<TicketDto>.Success(StatusCodes.Status200OK, entityAsDto);
            }
            return CustomResponse<TicketDto>.Fail(StatusCodes.Status404NotFound, $" {typeof(Ticket).Name} ({id}) not found. Retrieve operation is not successfull. ");

        }

        public async Task<CustomResponse<NoContentResponse>> UpdateAsync(TicketUpdateDto ticketUpdateDto)
        {
            if (await _ticketRepository.AnyAsync(x => x.Id == ticketUpdateDto.Id && x.IsActive == true))
            {
                var entity = _mapper.Map<Ticket>(ticketUpdateDto);

                entity.UpdatedDate = DateTime.Now;
                _ticketRepository.Update(entity);
                await _unitOfWork.CommitAsync();
                return CustomResponse<NoContentResponse>.Success(StatusCodes.Status204NoContent);
            }
            return CustomResponse<NoContentResponse>.Fail(StatusCodes.Status404NotFound, $" {typeof(Ticket).Name} ({ticketUpdateDto.Id}) not found. Updete operation is not successfull. ");

        }
    }
}
