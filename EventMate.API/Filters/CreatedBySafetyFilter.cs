using EventMate.Core.Model.Abstract;
using EventMate.Core.Repository;
using Microsoft.AspNetCore.Mvc.Filters;

namespace EventMate.API.Filters
{
    public class CreatedBySafetyFilter<T, Dto> : IAsyncActionFilter
    where T : BaseModel
    where Dto : class
    {
        private readonly IGenericRepository<T> _service;
        public CreatedBySafetyFilter(IGenericRepository<T> service)
        {
            _service = service;
        }
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {

            Dto dto = context.ActionArguments.Values.FirstOrDefault(x => x is Dto) as Dto;

            var dtoInfo = (dto?.GetType().GetProperty("Id")?.GetValue(dto), dto?.GetType().GetProperty("CreatedBy")?.GetValue(dto));
            var id = (int)dtoInfo.Item1;
            var createdBy = (string?)dtoInfo.Item2;


            var model = _service.GetByIdAsNoTrackingAsync(id).Result;


            var anyEntity = await _service.AnyAsync(x => x.Id == id && x.IsActive == true);
            if (model != null && dto != null && model.CreatedBy != GetCreatedBy(dto))
            {
                dto.GetType().GetProperty("CreatedBy")?.SetValue(dto, model.CreatedBy);

            }
            await next();

        }

        private string GetCreatedBy(Dto dto)
        {
            var createdByProperty = dto.GetType().GetProperty("CreatedBy");
            if (createdByProperty != null && createdByProperty.PropertyType == typeof(string))
            {
                return (string)createdByProperty.GetValue(dto);
            }

            return string.Empty;
        }
    }
}
