using AutoMapper;
using SpeechGPT.Application.Common.Mappings;
using SpeechGPT.Application.CQRS.Queries;

namespace SpeechGPT.WebApi.Models.User
{
    public class GetUserDto : IMappable<GetUserViewModelQuery>
    {
        public int? Id { get; set; }
        public string? Username { get; set; }
        public string? Email { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<GetUserDto, GetUserViewModelQuery>();
        }
    }
}
