using MediatR;
using Microsoft.EntityFrameworkCore;
using SpeechGPT.Application.Common.Exceptions;
using SpeechGPT.Application.Interfaces;
using SpeechGPT.Domain;
using SpeechGPT.Domain.Enums;

namespace SpeechGPT.Application.CQRS.Commands
{
    public class RegisterUserCommand : IRequest
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }

    public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand>
    {
        private readonly IAppDbContext _context;
        public RegisterUserCommandHandler(IAppDbContext context, IEmailService emailService) =>
            _context = context;
        public async Task Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {
            var existingUser = await _context.Users
                .FirstOrDefaultAsync(u => 
                                  string.Equals(u.UserName.ToLower(),request.UserName.ToLower()) 
                                     || string.Equals(u.Email.ToLower(),request.Email.ToLower()),
                                  cancellationToken);

            if (existingUser is not null)
            {
                //also will be Username if both Username and Email exist
                var existingPropertyName = existingUser.UserName == request.UserName
                    ? nameof(request.UserName)
                    : nameof(request.Email);
                var existingPropertyValue = existingUser.UserName == request.UserName
                    ? request.UserName
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
                UserName = request.UserName,
                Email = request.Email,
                PasswordHash = BCrypt.Net.BCrypt.EnhancedHashPassword(request.Password),
                UserRole = UserRole.User,
                EmailConfirmed = false,
            };

            _context.Users.Add(User);
            await _context.SaveChangesAsync(cancellationToken);

        }
    }
}
