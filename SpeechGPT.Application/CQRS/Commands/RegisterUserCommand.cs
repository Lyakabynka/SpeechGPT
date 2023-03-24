using MediatR;
using Microsoft.EntityFrameworkCore;
using SpeechGPT.Application.Common.Exceptions;
using SpeechGPT.Application.Interfaces;
using SpeechGPT.Domain;

namespace SpeechGPT.Application.CQRS.Commands
{
    public class RegisterUserCommand : IRequest
    {
        public string Nickname { get; set; }
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
                                    u.Nickname == request.Nickname 
                                    || string.Equals(u.Email.ToLower(),request.Email.ToLower()),
                                  cancellationToken);

            if (existingUser is not null)
            {
                var existingPropertyName = existingUser.Nickname == request.Nickname
                    ? nameof(request.Nickname)
                    : nameof(request.Email);
                var existingPropertyValue = existingUser.Nickname == request.Nickname
                    ? request.Nickname
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
                Nickname = request.Nickname,
                Email = request.Email,
                PasswordHash = BCrypt.Net.BCrypt.EnhancedHashPassword(request.Password, BCrypt.Net.HashType.SHA512),
                Role = Role.User,
            };

            _context.Users.Add(User);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
