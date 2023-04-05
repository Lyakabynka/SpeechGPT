using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SpeechGPT.Application.CQRS.Commands;
using SpeechGPT.WebApi.Controllers.Base;
using SpeechGPT.WebApi.ActionResults;
using SpeechGPT.WebApi.Models.Auth;
using SpeechGPT.WebApi.Models.User;
using SpeechGPT.Application.CQRS.Queries;
using System.Net;
using System.Security.Claims;
using SpeechGPT.Domain.Enums;

namespace SpeechGPT.WebApi.Controllers
{
    [Route("api/users")]
    public class UserController : BaseController
    {
        private readonly IMapper _mapper;

        public UserController(IMapper mapper) =>
            _mapper = mapper;

        /// <summary>
        /// Creates the user
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// POST api/users
        /// </remarks>
        /// 
        /// <param name="request">Register user dto (username email password)</param>
        /// <response code="200">Success / user_already_exists</response>
        
        
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> Register(
            [FromBody] RegisterUserDto request)
        {
            var command = _mapper.Map<RegisterUserCommand>(request);

            await Mediator.Send(command);

            return new WebApiResult();
        }
    }
}
