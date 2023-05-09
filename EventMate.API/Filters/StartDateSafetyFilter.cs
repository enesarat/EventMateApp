using EventMate.Core.DTO.Abstract;
using EventMate.Core.Model.Abstract;
using EventMate.Core.Model.Concrete;
using EventMate.Core.Repository;
using Microsoft.AspNetCore.Mvc.Filters;

namespace EventMate.API.Filters
{
    public class StartDateSafetyFilter<T, Dto> :  IAsyncActionFilter 
    where T : Event
    where Dto : class
    {
        private readonly IGenericRepository<T> _service;
        public StartDateSafetyFilter(IGenericRepository<T> service)
        {
            _service = service;
        }
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {

            Dto dto = context.ActionArguments.Values.FirstOrDefault(x => x is Dto) as Dto;

            var dtoInfo = (dto?.GetType().GetProperty("Id")?.GetValue(dto), dto?.GetType().GetProperty("StartDate")?.GetValue(dto));
            var id = (int)dtoInfo.Item1;
            var startDate = (DateTime?)dtoInfo.Item2;


            var model = _service.GetByIdAsNoTrackingAsync(id).Result;


            var anyEntity = await _service.AnyAsync(x => x.Id == id && x.IsActive == true);
            if (model != null && dto != null && model.StartDate != GetStartDate(dto))
            {
                var startDateDifference = GetStartDateDifference(model);
                if (startDateDifference <= 5)
                {
                    throw new Exception(" The entity cannot be updated as the StartDate is within 5 days. ");
                }

                dto.GetType().GetProperty("StartDate")?.SetValue(dto, model.StartDate);

            }
            await next();

        }

        private DateTime GetStartDate(Dto dto)
        {
            var startDateProperty = dto.GetType().GetProperty("StartDate");
            if (startDateProperty != null && startDateProperty.PropertyType == typeof(DateTime))
            {
                return (DateTime)startDateProperty.GetValue(dto);
            }

            return DateTime.MinValue;
        }
        private DateTime GetStartDate(T model)
        {
            var startDateProperty = model.GetType().GetProperty("StartDate");
            if (startDateProperty != null && startDateProperty.PropertyType == typeof(DateTime))
            {
                return (DateTime)startDateProperty.GetValue(model);
            }

            return DateTime.MinValue;
        }
        private int GetStartDateDifference(T model)
        {
            var startDate = GetStartDate(model);
            var today = DateTime.Today;

            return (startDate - today).Days;
        }
    }
}
