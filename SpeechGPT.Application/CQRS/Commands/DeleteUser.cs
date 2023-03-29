using MediatR;
using SpeechGPT.Application.Common.Exceptions;
using SpeechGPT.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpeechGPT.Application.CQRS.Commands
{
    public class DeleteUserCommand : IRequest
    {
        public int UserId { get; set; }
    }

    public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand>
    {
        private readonly IAppDbContext _context;
        public DeleteUserCommandHandler(IAppDbContext context) =>
            _context = context;

        public async Task Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _context.Users.FindAsync(request.UserId, cancellationToken);

            if(user == null)
            {
                var reasonField = nameof(request.UserId);
                var reasonValue = request.UserId;

                throw new ExpectedApiException()
                {
                    ErrorCode = ErrorCode.UserNotFound,
                    ReasonField = reasonField,
                    PublicErrorMessage = $"User with given {reasonField} does not exist",
                    LogErrorMessage = $"Delete user error. User with given {reasonField} [{reasonValue}] does not exist"
                };
            }

            _context.Users.Remove(user);

            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
