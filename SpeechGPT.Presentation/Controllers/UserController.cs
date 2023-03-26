using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SpeechGPT.Application.CQRS.Commands;
using SpeechGPT.WebApi.Controllers.Base;
using SpeechGPT.WebApi.ActionResults;
using SpeechGPT.WebApi.Models.Auth;
using SpeechGPT.WebApi.Models.User;
using SpeechGPT.Application.CQRS.Queries;

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
        /// <param name="request">Register user credentials dto</param>
        /// <response code="200">Success / user_already_exists</response>
        [HttpPost]
        public async Task<ActionResult> Register(
            [FromBody] RegisterUserDto request)
        {
            var command = _mapper.Map<CreateUserCommand>(request);

            await Mediator.Send(command);

            return new WebApiResult();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Delete()
        {
            throw new NotImplementedException();
            return Ok();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> GetUser(
            [FromQuery] GetUserDto request)
        {
            var query = _mapper.Map<GetUserCommand>(request);

            var userVm = await Mediator.Send(query);

            return new WebApiResult()
            {
                Data = userVm
            };
        }


        //update user data
    }
}
