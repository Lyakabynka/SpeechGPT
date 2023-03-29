
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SpeechGPT.Application.CQRS.Queries.ViewModels;
using SpeechGPT.Application.Interfaces;

namespace SpeechGPT.Application.CQRS.Queries
{
    public class GetChatPreviewVmsQuery : IRequest<List<ChatPreviewVm>>
    {
        public int UserId { get; set; }
    }

    public class GetChatPreviewVmsQueryHandler : IRequestHandler<GetChatPreviewVmsQuery, List<ChatPreviewVm>>
    {
        private readonly IAppDbContext _context;
        private readonly IMapper _mapper;
        public GetChatPreviewVmsQueryHandler(IMapper mapper, IAppDbContext context) =>
            (_context,_mapper) = (context,mapper);

        public async Task<List<ChatPreviewVm>> Handle(GetChatPreviewVmsQuery request, CancellationToken cancellationToken)
        {
            var chatPreviewVms = await _context.Chats
                .Where(c => c.UserId == request.UserId)
                .ProjectTo<ChatPreviewVm>(_mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);

            return chatPreviewVms;
        }
    }
}
