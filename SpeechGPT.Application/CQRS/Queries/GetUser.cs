using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SpeechGPT.Application.Common.Exceptions;
using SpeechGPT.Application.CQRS.Queries.ViewModels;
using SpeechGPT.Application.Interfaces;
using SpeechGPT.Domain;

namespace SpeechGPT.Application.CQRS.Queries
{
    public class GetUserCommand : IRequest<UserVm>
    {
        public int? Id { get; set; }
        public string? UserName { get; set; }
        public string? Email { get; set; }
    }

    public class GetUserCommandHandler : IRequestHandler<GetUserCommand, UserVm>
    {
        private readonly IAppDbContext _context;
        private readonly IMapper _mapper;
        public GetUserCommandHandler(IAppDbContext context, IMapper mapper) =>
            (_context,_mapper) = (context,mapper);

        public async Task<UserVm> Handle(GetUserCommand request, CancellationToken cancellationToken)
        {
            var user = 
                request.Id != null
                    ? await _context.Users.FindAsync(request.Id,cancellationToken)
                : string.IsNullOrWhiteSpace(request.UserName)
                    ? await _context.Users.FirstOrDefaultAsync(u=>u.UserName == request.UserName, cancellationToken)
                : string.IsNullOrWhiteSpace(request.Email) 
                    ? await _context.Users.FirstOrDefaultAsync(u => u.Email == request.Email, cancellationToken)
                : null;

            if (user == null)
            {
                throw new ExpectedApiException()
                {
                    ErrorCode = ErrorCode.UserNotFound,
                    PublicErrorMessage = $"User with given parameter was not found.",
                    LogErrorMessage = $"User with given parameter was not found."
                };
            }

            var userVm = _mapper.Map<UserVm>(user);

            return userVm;
        }
    }
}
