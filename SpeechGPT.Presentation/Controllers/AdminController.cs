using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SpeechGPT.Application.CQRS.Commands;
using SpeechGPT.Application.CQRS.Queries;
using SpeechGPT.WebApi.ActionResults;
using SpeechGPT.WebApi.Controllers.Base;
using SpeechGPT.WebApi.Models.User;

namespace SpeechGPT.WebApi.Controllers
{
    [Route("api/user-management/")]
    public class AdminController : BaseController
    {
        private readonly IMapper _mapper;
        public AdminController(IMapper mapper) =>
            _mapper = mapper;

        /// <summary>
        /// Deletes the user
        /// </summary>
        /// <param name="userId">Delete user id</param>
        /// <response code="200">Success / user_not_found</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="403">Forbidden</response>
        [HttpDelete("{userId:int}")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult> Delete(
            [FromRoute] int userId)
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
            var query = _mapper.Map<GetUserViewModelQuery>(request);

            var userVm = await Mediator.Send(query);

            return new WebApiResult()
            {
                Data = userVm
            };
        }
    }
}
