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
        /// <param name="request">Register user dto (username email password)</param>
        /// <response code="200">Success / user_already_exists</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> Register(
            [FromBody] RegisterUserDto request)
        {
            var command = _mapper.Map<CreateUserCommand>(request);

            await Mediator.Send(command);

            return new WebApiResult();
        }


        /// <summary>
        /// Deletes the user
        /// </summary>
        /// <param name="userId">Delete user id</param>
        /// <response code="200">Success / user_not_found</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="403">Forbidden</response>
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult> Delete(
            [FromQuery] int userId)
        {
            var command = new DeleteUserCommand() { UserId = userId };

            await Mediator.Send(command);

            return new WebApiResult();
        }

        /// <summary>
        /// Gets the user
        /// </summary>
        /// 
        /// <param name="request">Get user dto (id/username/password)</param>
        /// 
        /// <response code="200">Success / user_not_found</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="403">Forbidden</response>
        [HttpGet]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
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


        /*/// <summary>
        /// Updates the user
        /// </summary>
        /// <param name="userId">Update user id</param>
        /// <response code="200">Success / user_not_found</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="403">Forbidden</response>
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult> Update(
            [FromQuery] int userId)
        {
            var command = new DeleteUserCommand() { UserId = userId };

            await Mediator.Send(command);

            return new WebApiResult();
        }*/
    }
}
