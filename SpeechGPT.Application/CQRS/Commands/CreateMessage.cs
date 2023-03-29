using MediatR;
using Microsoft.EntityFrameworkCore;
using SpeechGPT.Application.Common.Exceptions;
using SpeechGPT.Application.CQRS.Queries.ViewModels;
using SpeechGPT.Application.Interfaces;
using SpeechGPT.Domain;
using SpeechGPT.Domain.Enums;

namespace SpeechGPT.Application.CQRS.Commands
{
    public class CreateMessageCommand : IRequest
    {
        public int UserId { get; set; }
        public int ChatId { get; set; }
        public string Body { get; set; }

        public MessageType MessageType { get; set; }
    }

    public class CreateMessageCommandHandler : IRequestHandler<CreateMessageCommand>
    {
        private readonly IAppDbContext _context;
        public CreateMessageCommandHandler(IAppDbContext context) =>
            _context = context;

        public async Task Handle(CreateMessageCommand request, CancellationToken cancellationToken)
        {
            var chat = await _context.Chats.FindAsync(request.ChatId, cancellationToken);

            if (chat == null)
            {
                var reasonField = nameof(request.ChatId);
                var reasonValue = request.ChatId;
                throw new ExpectedApiException()
                {
                    ErrorCode = ErrorCode.ChatNotFound,
                    ReasonField = reasonField,
                    PublicErrorMessage = $"Chat with given {reasonField} does not exist",
                    LogErrorMessage = $"Get chat error. Chat with given {reasonField} [{reasonValue}] does not exist"
                };
            }

            if (chat.UserId != request.UserId)
            {
                var reasonField = nameof(request.UserId);
                var reasonValue = request.UserId;
                throw new ExpectedApiException()
                {
                    ErrorCode = ErrorCode.ChatNotFound,
                    ReasonField = reasonField,
                    PublicErrorMessage = $"Chat {reasonField} and given {reasonField} do not match",
                    LogErrorMessage = $"Get chat error. Chat {reasonField} and given {reasonField} [{reasonValue}] do not match"
                };
            }

            var message = new Message()
            {
                Body = request.Body,
                CreatedAt = DateTime.UtcNow,

                MessageType = request.MessageType,

                ChatId = request.ChatId
            };

            _context.Entry(message).State = EntityState.Added;

            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
