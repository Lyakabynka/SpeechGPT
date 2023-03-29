using AutoMapper;
using SpeechGPT.Application.Common.Mappings;
using SpeechGPT.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpeechGPT.Application.CQRS.Queries.ViewModels
{
    public class ChatVm : IMappable<Chat>
    {
        public int Id { get; set; }
        public string Name { get; set; } = "New Chat";

        public List<MessageVm> MessageVms { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Chat, ChatVm>();
        }
    }
}
