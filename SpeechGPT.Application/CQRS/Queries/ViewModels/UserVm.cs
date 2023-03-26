using AutoMapper;
using SpeechGPT.Application.Common.Mappings;
using SpeechGPT.Domain;

namespace SpeechGPT.Application.CQRS.Queries.ViewModels
{
    public class UserVm : IMappable<User>
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }

        public Role Role { get; set; }

        public bool EmailConfirmed { get; set; }

        public List<Request> Requests { get; set; }
        public List<Response> Responses { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<User, UserVm>();
        }
    }
}
