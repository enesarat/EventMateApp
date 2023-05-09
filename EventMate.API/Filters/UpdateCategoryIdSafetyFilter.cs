using EventMate.Core.Model.Concrete;
using EventMate.Core.Repository;
using Microsoft.AspNetCore.Mvc.Filters;

namespace EventMate.API.Filters
{
    public class UpdateCategoryIdSafetyFilter<T, Dto> : IAsyncActionFilter
    where T : Event
    where Dto : class
    {
        private readonly IGenericRepository<T> _service;
        public UpdateCategoryIdSafetyFilter(IGenericRepository<T> service)
        {
            _service = service;
        }
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {

            Dto dto = context.ActionArguments.Values.FirstOrDefault(x => x is Dto) as Dto;

            var dtoInfo = (dto?.GetType().GetProperty("Id")?.GetValue(dto), dto?.GetType().GetProperty("CategoryId")?.GetValue(dto));
            var id = (int)dtoInfo.Item1;
            var categoryId = (int?)dtoInfo.Item2;


            var model = _service.GetByIdAsNoTrackingAsync(id).Result;


            var anyEntity = await _service.AnyAsync(x => x.Id == id && x.IsActive == true);
            if (model != null && dto != null && model.CategoryId != GetCategoryId(dto))
            {
                dto.GetType().GetProperty("CategoryId")?.SetValue(dto, model.CategoryId);

            }
            await next();

        }

        private int GetCategoryId(Dto dto)
        {
            var categoryIdProperty = dto.GetType().GetProperty("CategoryId");
            if (categoryIdProperty != null && categoryIdProperty.PropertyType == typeof(int))
            {
                return (int)categoryIdProperty.GetValue(dto);
            }

            return 0;
        }
    }
}
