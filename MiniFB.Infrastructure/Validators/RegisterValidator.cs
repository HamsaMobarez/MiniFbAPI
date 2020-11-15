using FluentValidation;
using MiniFB.Infrastructure.Dtos;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace MiniFB.Infrastructure.Validators
{
    public class RegisterValidator : AbstractValidator<RegisterDto>
    {
        public RegisterValidator()
        {
            RuleFor(x => x.UserName)
                .NotEmpty()
                .MaximumLength(100);
            
            RuleFor(x => x.PhoneNumber)
                .NotEmpty()
                .MaximumLength(12)
                .WithMessage("Phone number must be 12 digits only");
            RuleFor(x => x.Password)
                .MinimumLength(8)
                .WithMessage("Password can't be less than 8 characters")
                .MaximumLength(30)
                .WithMessage("Password musnt not execeed 30 characters")
                .Must(password => CheckPassword(password))
                .WithMessage("Sorry password didn't satisfy the custom logic");
        }
        public bool CheckPassword(string password)
        {
            string matchPasswordPattern = @"(?=^[^\s]{6,}$)(?=.*\d)(?=.*[a-zA-Z])";

            if (password != null) return Regex.IsMatch(password, matchPasswordPattern);
            else return false;
        }
    }
}
