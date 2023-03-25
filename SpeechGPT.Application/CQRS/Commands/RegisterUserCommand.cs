using MediatR;
using Microsoft.EntityFrameworkCore;
using SpeechGPT.Application.Common.Exceptions;
using SpeechGPT.Application.Interfaces;
using SpeechGPT.Domain;

namespace SpeechGPT.Application.CQRS.Commands
{
    public class RegisterUserCommand : IRequest
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }


    public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand>
    {
        private readonly IAppDbContext _context;
        private readonly IEmailService _emailService;
        public RegisterUserCommandHandler(IAppDbContext context, IEmailService emailService) =>
            (_context, _emailService) = (context, emailService);
        public async Task Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {
            var existingUser = await _context.Users
                .FirstOrDefaultAsync(u => 
                                  string.Equals(u.UserName.ToLower(),request.Username.ToLower()) 
                                     || string.Equals(u.Email.ToLower(),request.Email.ToLower()),
                                  cancellationToken);

            if (existingUser is not null)
            {
                //also will be Username if both Username and Email exist
                var existingPropertyName = existingUser.UserName == request.Username
                    ? nameof(request.Username)
                    : nameof(request.Email);
                var existingPropertyValue = existingUser.UserName == request.Username
                    ? request.Username
                    : request.Email;

                throw new ExpectedApiException()
                {
                    ErrorCode = ErrorCode.UserAlreadyExists,
                    ReasonField = existingPropertyName,
                    PublicErrorMessage = $"User with given {existingPropertyName} already exists",
                    LogErrorMessage = $"Create User error. User with given {existingPropertyName} [{existingPropertyValue}] already exists",
                };
            }

            var User = new User()
            {
                UserName = request.Username,
                Email = request.Email,
                PasswordHash = BCrypt.Net.BCrypt.EnhancedHashPassword(request.Password),
                Role = Role.User,
                EmailConfirmed = false,
                ConfirmEmailCode = new ConfirmEmailCode(),
            };

            //todo
            //await _emailService.SendRegistrationEmailAsync(User);

            _context.Users.Add(User);
            await _context.SaveChangesAsync(cancellationToken);

        }
    }
}
