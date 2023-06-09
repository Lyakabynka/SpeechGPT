﻿using AutoMapper;
using SpeechGPT.Application.Common.Mappings;
using SpeechGPT.Domain;
using SpeechGPT.Domain.Enums;

namespace SpeechGPT.Application.CQRS.Queries.ViewModels
{
    public class UserVm : IMappable<User>
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }

        public UserRole UserRole { get; set; }

        public bool EmailConfirmed { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<User, UserVm>();
        }
    }
}
