using AutoMapper;
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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventMate.Service.Service
{
    public class TicketService : GenericService<Ticket, TicketDto>, ITicketService
    {
        private readonly ITicketRepository _ticketRepository;
        public TicketService(IGenericRepository<Ticket> repository, IUnitOfWork unitOfWork, IMapper mapper, ITicketRepository ticketRepository) : base(repository, unitOfWork, mapper)
        {
            _ticketRepository = ticketRepository;
        }

        public async Task<CustomResponse<NoContentResponse>> AddAsync(TicketCreateDto ticketCreateDto)
        {
            var item = _mapper.Map<Ticket>(ticketCreateDto);
            item.CreatedDate = DateTime.Now;
            item.CreatedBy = "SYSTEM";
            await _ticketRepository.AddAsync(item);
            await _unitOfWork.CommitAsync();

            return CustomResponse<NoContentResponse>.Success(StatusCodes.Status204NoContent);
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
