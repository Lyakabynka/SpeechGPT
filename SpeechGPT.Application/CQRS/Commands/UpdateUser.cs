
using MediatR;

namespace SpeechGPT.Application.CQRS.Commands
{
    public class UpdateUserCommand : IRequest
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }

    }
}
