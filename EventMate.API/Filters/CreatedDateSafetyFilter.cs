using EventMate.Core.DTO.Concrete.Response;
using EventMate.Core.Model.Abstract;
using EventMate.Core.Repository;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System;

namespace EventMate.API.Filters
{
    public class CreateDateSafetyFilter<T, Dto> : IAsyncActionFilter
    where T : BaseModel
    where Dto : class
    {
        private readonly IGenericRepository<T> _service;
        public CreateDateSafetyFilter(IGenericRepository<T> service)
        {
            _service = service;
        }
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {

            Dto dto = context.ActionArguments.Values.FirstOrDefault(x => x is Dto) as Dto;

            var dtoInfo = (dto?.GetType().GetProperty("Id")?.GetValue(dto), dto?.GetType().GetProperty("CreatedDate")?.GetValue(dto));
            var id = (int)dtoInfo.Item1;
            var createdDate = (DateTime?)dtoInfo.Item2;


            var model = _service.GetByIdAsNoTrackingAsync(id).Result;


            var anyEntity = await _service.AnyAsync(x => x.Id == id && x.IsActive == true);
            if (model != null && dto != null && model.CreatedDate != GetCreatedDate(dto))
            {
                dto.GetType().GetProperty("CreatedDate")?.SetValue(dto, model.CreatedDate);
                
                await next();
            }
            
            
        }

        private DateTime GetCreatedDate(Dto dto)
        {
            var createdDateProperty = dto.GetType().GetProperty("CreatedDate");
            if (createdDateProperty != null && createdDateProperty.PropertyType == typeof(DateTime))
            {
                return (DateTime)createdDateProperty.GetValue(dto);
            }

            return DateTime.MinValue;
        }
    }
}
