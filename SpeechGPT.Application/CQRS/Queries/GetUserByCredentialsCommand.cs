using MediatR;
using Microsoft.EntityFrameworkCore;
using SpeechGPT.Application.Common.Exceptions;
using SpeechGPT.Application.Interfaces;
using SpeechGPT.Domain;

namespace SpeechGPT.Application.CQRS.Queries
{
    public class GetUserByCredentialsCommand : IRequest<User>
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }

    public class GetUserByCredentialsCommandHandler : IRequestHandler<GetUserByCredentialsCommand, User>
    {
        private readonly IAppDbContext _context;
        public GetUserByCredentialsCommandHandler(IAppDbContext context) =>
            _context = context;

        public async Task<User> Handle(GetUserByCredentialsCommand request, CancellationToken cancellationToken)
        {
            //get user by given username
            var user = await _context.Users
                .FirstOrDefaultAsync(user => 
                    string.Equals(user.UserName,request.UserName.ToLower()));

            //if user not found, exception
            if (user is null)
            {
                var reasonField = nameof(request.UserName);
                var reasonValue = request.UserName;

                throw new ExpectedApiException()
                {
                    ErrorCode = ErrorCode.UserNotFound,
                    ReasonField =  reasonField,
                    PublicErrorMessage = $"User with given {reasonField} was not found",
                    LogErrorMessage = $"Get user by credentials error. User with username [{reasonValue}] was not found"
                };
            }

            //if password does not match, exception
            if (!BCrypt.Net.BCrypt.EnhancedVerify(request.Password, user.PasswordHash)) 
            {
                var reasonField = nameof(request.Password);
                //var reasonValue = request.Password; ( because its password )

                throw new ExpectedApiException()
                {
                    ErrorCode = ErrorCode.UserPasswordIncorrect,
                    ReasonField = reasonField,
                    PublicErrorMessage = $"User with given {reasonField} was not found",
                    LogErrorMessage = $"Get user by credentials error. Wrong password to user [{user.UserName}]"
                };
            }

            //if password and nickname matches, return user
            return user;
        }
    }
}
