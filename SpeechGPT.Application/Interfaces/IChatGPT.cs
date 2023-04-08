using SpeechGPT.Application.CQRS.Queries.ViewModels;

namespace SpeechGPT.Application.Interfaces
{
    public interface IChatGPT
    {
        Task<string> Handle(string requestBody, ChatVm chatVm);
    }
}
