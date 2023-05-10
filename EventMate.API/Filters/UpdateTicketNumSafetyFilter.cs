using EventMate.Core.Model.Concrete;
using EventMate.Core.Repository;
using Microsoft.AspNetCore.Mvc.Filters;

namespace EventMate.API.Filters
{
    public class UpdateTicketNumSafetyFilter<T, Dto> : IAsyncActionFilter
    where T : Ticket
    where Dto : class
    {
        private readonly IGenericRepository<T> _service;
        public UpdateTicketNumSafetyFilter(IGenericRepository<T> service)
        {
            _service = service;
        }
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {

            Dto dto = context.ActionArguments.Values.FirstOrDefault(x => x is Dto) as Dto;

            var dtoInfo = (dto?.GetType().GetProperty("Id")?.GetValue(dto), dto?.GetType().GetProperty("IdentifiedTicketNumber")?.GetValue(dto));
            var id = (int)dtoInfo.Item1;
            var identifiedTicketNumber = (string?)dtoInfo.Item2;


            var model = _service.GetByIdAsNoTrackingAsync(id).Result;


            var anyEntity = await _service.AnyAsync(x => x.Id == id && x.IsActive == true);
            if (model != null && dto != null && model.IdentifiedTicketNumber != GetIdentifiedTicketNumber(dto))
            {
                dto.GetType().GetProperty("IdentifiedTicketNumber")?.SetValue(dto, model.IdentifiedTicketNumber);

            }
            await next();

        }

        private string GetIdentifiedTicketNumber(Dto dto)
        {
            var identifiedTicketNumberProperty = dto.GetType().GetProperty("IdentifiedTicketNumber");
            if (identifiedTicketNumberProperty != null && identifiedTicketNumberProperty.PropertyType == typeof(string))
            {
                return (string)identifiedTicketNumberProperty.GetValue(dto);
            }

            return string.Empty;
        }
    }
}
