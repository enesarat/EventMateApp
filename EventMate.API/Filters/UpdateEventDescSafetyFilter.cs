using EventMate.Core.Model.Concrete;
using EventMate.Core.Repository;
using Microsoft.AspNetCore.Mvc.Filters;

namespace EventMate.API.Filters
{
    public class UpdateEventDescSafetyFilter<T, Dto> : IAsyncActionFilter
    where T : Event
    where Dto : class
    {
        private readonly IGenericRepository<T> _service;
        public UpdateEventDescSafetyFilter(IGenericRepository<T> service)
        {
            _service = service;
        }
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {

            Dto dto = context.ActionArguments.Values.FirstOrDefault(x => x is Dto) as Dto;

            var dtoInfo = (dto?.GetType().GetProperty("Id")?.GetValue(dto), dto?.GetType().GetProperty("Description")?.GetValue(dto));
            var id = (int)dtoInfo.Item1;
            var description = (string?)dtoInfo.Item2;


            var model = _service.GetByIdAsNoTrackingAsync(id).Result;


            var anyEntity = await _service.AnyAsync(x => x.Id == id && x.IsActive == true);
            if (model != null && dto != null && model.Description != GetDescription(dto))
            {
                dto.GetType().GetProperty("Description")?.SetValue(dto, model.Description);

            }
            await next();

        }

        private string GetDescription(Dto dto)
        {
            var descriptionProperty = dto.GetType().GetProperty("Description");
            if (descriptionProperty != null && descriptionProperty.PropertyType == typeof(string))
            {
                return (string)descriptionProperty.GetValue(dto);
            }

            return string.Empty;
        }
    }
}
