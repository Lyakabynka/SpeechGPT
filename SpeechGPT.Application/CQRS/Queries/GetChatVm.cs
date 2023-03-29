
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SpeechGPT.Application.Common.Exceptions;
using SpeechGPT.Application.CQRS.Queries.ViewModels;
using SpeechGPT.Application.Interfaces;
using SpeechGPT.Domain;

namespace SpeechGPT.Application.CQRS.Queries
{
    public class GetChatVmQuery : IRequest<ChatVm>
    {
        public int ChatId { get; set; }
        public int UserId { get; set; }
    }

    public class GetChatVmQueryHandler : IRequestHandler<GetChatVmQuery, ChatVm>
    {
        private readonly IAppDbContext _context;
        private readonly IMapper _mapper;
        public GetChatVmQueryHandler(IAppDbContext context, IMapper mapper) =>
            (_context,_mapper) = (context,mapper);
        public async Task<ChatVm> Handle(GetChatVmQuery request, CancellationToken cancellationToken)
        {
            var chat = await _context.Chats.FindAsync(request.ChatId, cancellationToken);

            if(chat == null)
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

            if(chat.UserId != request.UserId)
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

            await _context.Entry(chat).Collection(c => c.Messages).LoadAsync(cancellationToken);

            var chatVm = _mapper.Map<ChatVm>(chat);

            return chatVm;
        }
    }
}
