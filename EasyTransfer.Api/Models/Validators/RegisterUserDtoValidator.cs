
namespace EasyTransfer.Api.Models.Validators
{
    public class RegisterUserDtoValidator: AbstractValidator<RegisterUserDto>
    {
        public RegisterUserDtoValidator( EasyTransferDBContext DbContext)
        {
            RuleFor(x => x.Email)
                .NotEmpty()
                .EmailAddress()
                .Custom((value, context) =>
                {
                    var emailInUse = DbContext.Users.Any(u => u.Email == value);
                    if (emailInUse)
                    {
                        context.AddFailure("Email", "That email is taken");
                    }
                });
                

            RuleFor(x => x.Password)
                .MinimumLength(8)
                .Custom((value, context) =>
                {
                    var hadDigit = value.Any(l => char.IsDigit(l));
                    if (!hadDigit)
                    {
                        context.AddFailure("Password", "Your password have to contain at least one digit");
                    }
                    var hadLetter = value.Any(l => char.IsLetter(l));
                    if (!hadLetter)
                    {
                        context.AddFailure("Password", "Your password have to contain at least one letter");
                    }
                    var hadUpper = value.Any(l => char.IsUpper(l));
                    if (!hadUpper)
                    {
                        context.AddFailure("Password", "Your password have to contain at least one uppercase letter");
                    }
                });

            RuleFor(x => x.ConfirmPassword)
                .Equal(e => e.Password);

            RuleFor(x => x.PhoneNumber)
                .MaximumLength(9);
        }
    }
}
