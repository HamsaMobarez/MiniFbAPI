using FluentValidation;
using MiniFB.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace MiniFB.Infrastructure.Validators
{
    public class VerifyMobileValidator : AbstractValidator<VerifyMobileModel>
    {
        public VerifyMobileValidator()
        {
            RuleFor(x => x.OTP)
                .NotEmpty()
                .Length(4)
                .WithMessage("OTP can't be empty or exceed 4 digits");
            RuleFor(x => x.PhoneNumber)
                .NotEmpty()
                .MaximumLength(12)
                .WithMessage("Phone number must be 12 digits only");
        }
    }
}
