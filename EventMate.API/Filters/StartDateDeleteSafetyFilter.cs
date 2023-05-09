using EventMate.Core.DTO.Concrete.Response;
using EventMate.Core.Model.Abstract;
using EventMate.Core.Model.Concrete;
using EventMate.Core.Repository;
using EventMate.Repository.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Net;

namespace EventMate.API.Filters
{
    public class EventStartDateFilter : IAsyncActionFilter
    {
        private readonly IServiceProvider _serviceProvider;

        public EventStartDateFilter(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (!context.ActionArguments.ContainsKey("id"))
            {
                context.Result = new BadRequestObjectResult("id parameter is missing.");
                return;
            }

            int id = (int)context.ActionArguments["id"];

            using var scope = _serviceProvider.CreateScope();
            var dbContext = scope.ServiceProvider.GetService<ApplicationDbContext>();
            var eventRepository = scope.ServiceProvider.GetService<IGenericRepository<Event>>();

            var entity = await eventRepository.GetByIdAsync(id);
            if (entity == null)
            {
                throw new Exception($" The entity with id '{id}' was not found. ");
            }

            if (entity.StartDate.Date > DateTime.Now.Date && (entity.StartDate.Date - DateTime.Now.Date).TotalDays <= 5)
            {
                throw new Exception(" The entity cannot be deleted as the StartDate is within 5 days. ");
            }

            await next();
        }
    }
}
