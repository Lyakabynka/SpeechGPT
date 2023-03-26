﻿using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SpeechGPT.Application.CQRS.Queries;
using SpeechGPT.Application.Services;
using SpeechGPT.WebApi.ActionResults;
using SpeechGPT.WebApi.Controllers.Base;
using SpeechGPT.WebApi.Models.Auth;

namespace SpeechGPT.Presentation.Controllers
{
    [Route("api/auth")]
    public class AuthController : BaseController
    {
        private readonly IMapper _mapper;
        private readonly JwtProvider _jwtProvider;

        public AuthController(IMapper mapper, JwtProvider jwtProvider) =>
            (_mapper,_jwtProvider) = (mapper,jwtProvider);

        /// <summary>
        /// User login
        /// </summary>
        ///
        /// <param name="request">User credentials login dto</param>
        /// <response code="200">Success / user_not_found / user_password_incorrect</response>
        [HttpPost("login")]
        public async Task<ActionResult> Login(
            [FromBody] LoginUserDto request)
        {
            var command = _mapper.Map<GetUserByUsernameAndPasswordCommand>(request);

            var user = await Mediator.Send(command);

            var token = _jwtProvider.CreateToken(user);

            return new WebApiResult()
            {
                Data = token,
            };
        }
    }
}
