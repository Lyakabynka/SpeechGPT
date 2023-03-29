using AutoMapper;
using SpeechGPT.Application.Common.Mappings;
using SpeechGPT.Application.CQRS.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpeechGPT.WebApi.Models.Auth
{
    public class LoginUserDto : IMappable<GetUserByUsernameAndPasswordQuery>
    {
        public string UserName { get; set; }
        public string Password { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<LoginUserDto, GetUserByUsernameAndPasswordQuery>();
        }
    }
}
