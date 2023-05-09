using EventMate.Core.Model.Concrete;
using EventMate.Core.Repository;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;

namespace EventMate.API.Filters
{
    public class EndDateSafetyFilter<T, Dto> : IAsyncActionFilter
    where T : Event
    where Dto : class
    {
        private readonly IGenericRepository<T> _service;
        public EndDateSafetyFilter(IGenericRepository<T> service)
        {
            _service = service;
        }
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {

            Dto dto = context.ActionArguments.Values.FirstOrDefault(x => x is Dto) as Dto;

            var dtoInfo = (dto?.GetType().GetProperty("Id")?.GetValue(dto), dto?.GetType().GetProperty("EndDate")?.GetValue(dto));
            var id = (int)dtoInfo.Item1;
            var endDate = (DateTime?)dtoInfo.Item2;


            var model = _service.GetByIdAsNoTrackingAsync(id).Result;


            var anyEntity = await _service.AnyAsync(x => x.Id == id && x.IsActive == true);
            if (model != null && dto != null && model.EndDate != GetEndDate(dto))
            {
                dto.GetType().GetProperty("EndDate")?.SetValue(dto, model.EndDate);

            }
            await next();

        }

        private DateTime GetEndDate(Dto dto)
        {
            var endDateProperty = dto.GetType().GetProperty("EndDate");
            if (endDateProperty != null && endDateProperty.PropertyType == typeof(DateTime))
            {
                return (DateTime)endDateProperty.GetValue(dto);
            }

            return DateTime.MinValue;
        }
    }
}
