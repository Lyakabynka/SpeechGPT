using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpeechGPT.Application.CQRS.Commands
{
    public class RegisterUserCommand : IRequest
    {
        public string Nickname { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
