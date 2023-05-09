using EventMate.Core.Model.Concrete;
using EventMate.Core.Repository;
using Microsoft.AspNetCore.Mvc.Filters;

namespace EventMate.API.Filters
{
    public class UpdateIsApprSafetyFiltercs<T, Dto> : IAsyncActionFilter
    where T : Event
    where Dto : class
    {
        private readonly IGenericRepository<T> _service;
        public UpdateIsApprSafetyFiltercs(IGenericRepository<T> service)
        {
            _service = service;
        }
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {

            Dto dto = context.ActionArguments.Values.FirstOrDefault(x => x is Dto) as Dto;

            var dtoInfo = (dto?.GetType().GetProperty("Id")?.GetValue(dto), dto?.GetType().GetProperty("IsApproved")?.GetValue(dto));
            var id = (int)dtoInfo.Item1;
            var isApproved = (bool?)dtoInfo.Item2;


            var model = _service.GetByIdAsNoTrackingAsync(id).Result;


            var anyEntity = await _service.AnyAsync(x => x.Id == id && x.IsActive == true);
            if (model != null && dto != null && model.IsApproved != GetIsApproved(dto))
            {
                dto.GetType().GetProperty("IsApproved")?.SetValue(dto, model.IsApproved);

            }
            await next();

        }

        private bool GetIsApproved(Dto dto)
        {
            var isApprovedProperty = dto.GetType().GetProperty("IsApproved");
            if (isApprovedProperty != null && isApprovedProperty.PropertyType == typeof(bool))
            {
                return (bool)isApprovedProperty.GetValue(dto);
            }

            return false;
        }
    }
}
