using AutoMapper;
using SpeechGPT.Application.Common.Mappings;
using SpeechGPT.Domain;
using SpeechGPT.Domain.Enums;

namespace SpeechGPT.Application.CQRS.Queries.ViewModels
{
    public class MessageVm : IMappable<Message>
    {
        public int Id { get; set; }
        public string Body { get; set; }
        public DateTime CreatedAt { get; set; }

        public MessageType MessageType { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Message, MessageVm>();
        }
    }
}
