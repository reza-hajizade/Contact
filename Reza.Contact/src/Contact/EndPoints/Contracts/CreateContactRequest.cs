using FluentValidation;

namespace Contact.EndPoints.Contracts;

public sealed record CreateContactRequest(string phoneNumber, string email);


public sealed class CreateContactRequestValidator:AbstractValidator<CreateContactRequest>
{

    public CreateContactRequestValidator()
    {
        RuleFor(x=>x.phoneNumber)
            .NotEmpty()
            .WithMessage("PhoneNumber is required")
            .Matches(@"^[0-9]{11}$")
            .WithMessage("Phone number must be a valid format.");
        RuleFor(x => x.email)
            .NotEmpty()
            .WithMessage("Email is required.")
            .EmailAddress()
            .WithMessage("Invalid email format.");

    }


}



