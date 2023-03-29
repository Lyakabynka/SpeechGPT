
using AutoMapper;
using SpeechGPT.Application.Common.Mappings;
using SpeechGPT.Domain;

namespace SpeechGPT.Application.CQRS.Queries.ViewModels
{
    public class ChatPreviewVm : IMappable<Chat>
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Chat, ChatPreviewVm>();
        }
    }
}
