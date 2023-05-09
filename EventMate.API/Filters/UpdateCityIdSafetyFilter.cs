using EventMate.Core.Model.Concrete;
using EventMate.Core.Repository;
using Microsoft.AspNetCore.Mvc.Filters;

namespace EventMate.API.Filters
{
    public class UpdateCityIdSafetyFilter<T, Dto> : IAsyncActionFilter
    where T : Event
    where Dto : class
    {
        private readonly IGenericRepository<T> _service;
        public UpdateCityIdSafetyFilter(IGenericRepository<T> service)
        {
            _service = service;
        }
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {

            Dto dto = context.ActionArguments.Values.FirstOrDefault(x => x is Dto) as Dto;

            var dtoInfo = (dto?.GetType().GetProperty("Id")?.GetValue(dto), dto?.GetType().GetProperty("CityId")?.GetValue(dto));
            var id = (int)dtoInfo.Item1;
            var cityId = (int?)dtoInfo.Item2;


            var model = _service.GetByIdAsNoTrackingAsync(id).Result;


            var anyEntity = await _service.AnyAsync(x => x.Id == id && x.IsActive == true);
            if (model != null && dto != null && model.CityId != GetCityId(dto))
            {
                dto.GetType().GetProperty("CityId")?.SetValue(dto, model.CityId);

            }
            await next();

        }

        private int GetCityId(Dto dto)
        {
            var cityIdProperty = dto.GetType().GetProperty("CityId");
            if (cityIdProperty != null && cityIdProperty.PropertyType == typeof(int))
            {
                return (int)cityIdProperty.GetValue(dto);
            }

            return 0;
        }
    }
}
