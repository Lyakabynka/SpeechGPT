using SpeechGPT.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpeechGPT.Application.Interfaces
{
    public interface IEmailService
    {
        Task SendRegistrationEmailAsync(User user);
    }
}
