using AutoMapper;
using EventMate.Core.DTO.Concrete.Category;
using EventMate.Core.DTO.Concrete.City;
using EventMate.Core.DTO.Concrete.Response;
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
    public class CityService : GenericService<City, CityDto>, ICityService
    {
        private readonly ICityRepository _cityRepository;

        public CityService(IGenericRepository<City> repository, IUnitOfWork unitOfWork, IMapper mapper, ICityRepository cityRepository) : base(repository, unitOfWork, mapper)
        {
            _cityRepository = cityRepository;
        }

        public async Task<CustomResponse<NoContentResponse>> AddAsync(CityCreateDto cityCreateDto)
        {
            var item = _mapper.Map<City>(cityCreateDto);
            item.CreatedDate = DateTime.Now;
            item.CreatedBy = "SYSTEM";
            await _cityRepository.AddAsync(item);
            await _unitOfWork.CommitAsync();

            return CustomResponse<NoContentResponse>.Success(StatusCodes.Status204NoContent);
        }

        public async Task<CustomResponse<NoContentResponse>> UpdateAsync(CityUpdateDto cityUpdateDto)
        {
            if (await _cityRepository.AnyAsync(x => x.Id == cityUpdateDto.Id && x.IsActive == true))
            {
                var entity = _mapper.Map<City>(cityUpdateDto);

                entity.UpdatedDate = DateTime.Now;
                _cityRepository.Update(entity);
                await _unitOfWork.CommitAsync();
                return CustomResponse<NoContentResponse>.Success(StatusCodes.Status204NoContent);
            }
            return CustomResponse<NoContentResponse>.Fail(StatusCodes.Status404NotFound, $" {typeof(Category).Name} ({cityUpdateDto.Id}) not found. Updete operation is not successfull. ");
        }
    }
}
