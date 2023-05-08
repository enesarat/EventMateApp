using EventMate.Core.DTO.Concrete.Category;
using EventMate.Core.DTO.Concrete.City;
using EventMate.Core.DTO.Concrete.Response;
using EventMate.Core.Model.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventMate.Core.Service
{
    public interface ICityService : IGenericService<City,CityDto>
    {
        public Task<CustomResponse<NoContentResponse>> AddAsync(CityCreateDto cityCreateDto);
        public Task<CustomResponse<NoContentResponse>> UpdateAsync(CityUpdateDto cityUpdateDto);
    }
}
