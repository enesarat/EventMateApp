using EventMate.Core.DTO.Concrete.Event;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventMate.Service.Validator.Event
{
    public class EventUpdateDtoValidator : AbstractValidator<EventUpdateDto>
    {
        public EventUpdateDtoValidator()
        {
            RuleFor(x => x.Id).InclusiveBetween(1, int.MaxValue).WithMessage(" {PropertyName} must be greater than 0 ");
            RuleFor(x => x.Name).NotNull().WithMessage(" {PropertyName} must have any value ").NotEmpty().WithMessage(" {PropertyName} is required ");
            /*RuleFor(x => x.StartDate)
            .Must(date => date != default(DateTime))
            .WithMessage(" Please enter a valid date! ")
            .GreaterThan(DateTime.Today)
            .WithMessage(" The start date cannot be earlier than today. ");
            RuleFor(x => x.EndDate)
            .Must(date => date != default(DateTime))
            .WithMessage(" Please enter a valid date! ")
            .GreaterThan(DateTime.Today)
            .WithMessage(" The start date cannot be earlier than today. ");*/
            RuleFor(x => x.Description).NotNull().WithMessage(" {PropertyName} must have any value ").NotEmpty().WithMessage(" {PropertyName} is required ");
            RuleFor(x => x.CityId).InclusiveBetween(1, int.MaxValue).WithMessage(" {PropertyName} must be greater than 0 ");
            RuleFor(x => x.Address).NotNull().WithMessage(" {PropertyName} must have any value ").NotEmpty().WithMessage(" {PropertyName} is required ");
            RuleFor(x => x.CategoryId).InclusiveBetween(1, int.MaxValue).WithMessage(" {PropertyName} must be greater than 0 ");
            RuleFor(x => x.Quota).InclusiveBetween(1, int.MaxValue).WithMessage(" {PropertyName} must be greater than 0 ");
        }
    }
}
