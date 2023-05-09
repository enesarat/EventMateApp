using EventMate.Core.Model.Abstract;
using EventMate.Core.Model.Concrete;
using EventMate.Core.Repository;
using Microsoft.AspNetCore.Mvc.Filters;

namespace EventMate.API.Filters
{
    public class UpdateEventNameSafetyFilter<T, Dto> : IAsyncActionFilter
    where T : Event
    where Dto : class
    {
        private readonly IGenericRepository<T> _service;
        public UpdateEventNameSafetyFilter(IGenericRepository<T> service)
        {
            _service = service;
        }
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {

            Dto dto = context.ActionArguments.Values.FirstOrDefault(x => x is Dto) as Dto;

            var dtoInfo = (dto?.GetType().GetProperty("Id")?.GetValue(dto), dto?.GetType().GetProperty("Name")?.GetValue(dto));
            var id = (int)dtoInfo.Item1;
            var name = (string?)dtoInfo.Item2;


            var model = _service.GetByIdAsNoTrackingAsync(id).Result;


            var anyEntity = await _service.AnyAsync(x => x.Id == id && x.IsActive == true);
            if (model != null && dto != null && model.Name != GetName(dto))
            {
                dto.GetType().GetProperty("Name")?.SetValue(dto, model.Name);

            }
            await next();

        }

        private string GetName(Dto dto)
        {
            var nameProperty = dto.GetType().GetProperty("Name");
            if (nameProperty != null && nameProperty.PropertyType == typeof(string))
            {
                return (string)nameProperty.GetValue(dto);
            }

            return string.Empty;
        }
    }
}
