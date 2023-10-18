using FluentValidation;

namespace Lime.Application.Authentication.Commands.Register;

public class RegisterCommandValidator : AbstractValidator<RegisterCommand>
{
    public RegisterCommandValidator()
    {
        this.RuleFor(x => x.Email)
            .EmailAddress();
    }
}