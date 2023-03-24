using AutoMapper;
using SpeechGPT.Application.Common.Mappings;
using SpeechGPT.Application.CQRS.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpeechGPT.WebApi.Models.Auth
{
    public class RegisterUserDto : IMappable<RegisterUserCommand>
    {
        public string Nickname { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }


        public void Mapping(Profile profile)
        {
            profile.CreateMap<RegisterUserDto,RegisterUserCommand>();
        }
    }
}
