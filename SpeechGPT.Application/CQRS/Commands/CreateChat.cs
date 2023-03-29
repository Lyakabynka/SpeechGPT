
using MediatR;
using Microsoft.EntityFrameworkCore;
using SpeechGPT.Application.Interfaces;
using SpeechGPT.Domain;

namespace SpeechGPT.Application.CQRS.Commands
{
    public class CreateChatCommand : IRequest
    {
        public int UserId { get; set; }
    }

    public class CreateChatCommandHandler : IRequestHandler<CreateChatCommand>
    {
        private readonly IAppDbContext _context;
        public CreateChatCommandHandler(IAppDbContext context) =>
            _context = context;

        public async Task Handle(CreateChatCommand request, CancellationToken cancellationToken)
        {
            var chat = new Chat()
            {
                UserId = request.UserId
            };

            _context.Entry(chat).State = EntityState.Added;

            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
