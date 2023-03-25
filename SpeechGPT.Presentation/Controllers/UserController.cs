using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SpeechGPT.Application.CQRS.Commands;
using SpeechGPT.Presentation.Controllers;
using SpeechGPT.WebApi.ActionResults;
using SpeechGPT.WebApi.Models.User;

namespace SpeechGPT.WebApi.Controllers
{
    [Route("api/user")]
    public class UserController : BaseController
    {
        private readonly IMapper _mapper;

        public UserController(IMapper mapper) =>
            _mapper = mapper;

        /// <summary>
        /// Creates the user
        /// </summary>
        /// <param name="request">Register user credentials dto</param>
        /// <response code="200">Success / user_already_exists</response>
        [HttpPost("register")]
        public async Task<ActionResult> Register(
            [FromBody] RegisterUserDto request)
        {
            var command = _mapper.Map<RegisterUserCommand>(request);

            await Mediator.Send(command);

            return new WebApiResult();
        }

        // ( for admins )

        //delete user
        
        //get user data

        //update user data
    }
}
