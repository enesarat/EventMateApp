﻿using EventMate.Core.DTO.Concrete.User;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventMate.Service.Validator.User
{
    public class UserUpdateAsAdminDtoValidator : AbstractValidator<UserUpdateAsAdminDto>
    {
        public UserUpdateAsAdminDtoValidator()
        {
            RuleFor(x => x.Id).InclusiveBetween(1, int.MaxValue).WithMessage(" {PropertyName} must be greater than 0 ");
            RuleFor(x => x.RoleId).InclusiveBetween(1, 3).WithMessage(" {PropertyName} must be be between 1 and 3 ");
            RuleFor(x => x.Name).NotNull().WithMessage(" {PropertyName} must have any value ").NotEmpty().WithMessage(" {PropertyName} is required ");
            RuleFor(x => x.Surname).NotNull().WithMessage(" {PropertyName} must have any value ").NotEmpty().WithMessage(" {PropertyName} is required ");
            RuleFor(x => x.Email).NotNull().WithMessage(" {PropertyName} must have any value ").NotEmpty().WithMessage(" {PropertyName} is required ").NotEmpty().EmailAddress().WithMessage(" It does not conform to the email format! ");
            RuleFor(x => x.Password).NotNull().WithMessage(" {PropertyName} must have any value ").NotEmpty().WithMessage(" {PropertyName} is required ");
        }
    }
}
