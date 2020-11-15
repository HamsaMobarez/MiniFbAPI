using FluentValidation;
using MiniFB.Infrastructure.Dtos;

namespace MiniFB.Infrastructure.Validators
{
    public class PostValidator: AbstractValidator<PostDto>
    {
        public PostValidator()
        {
            RuleFor(p => p.Text).NotEmpty().WithMessage("Post text can't be empty");
            RuleFor(p => p.Text).MaximumLength(250);
        }
    }
}
