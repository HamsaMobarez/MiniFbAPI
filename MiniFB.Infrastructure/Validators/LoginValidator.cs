using FluentValidation;
using MiniFB.Infrastructure.Dtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace MiniFB.Infrastructure.Validators
{
    public class LoginValidator: AbstractValidator<LoginDto>
    {
        public LoginValidator()
        {
            RuleFor(x => x.UserName)
                 .NotEmpty()
                 .MaximumLength(100);
            RuleFor(x => x.Password)
                .NotEmpty();
        }
    }
}
