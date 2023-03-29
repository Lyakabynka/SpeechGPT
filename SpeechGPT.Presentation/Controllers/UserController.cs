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

        [Authorize]
        [HttpPost("chats")]
        public async Task<ActionResult> CreateChat()
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            var command = new CreateChatCommand()
            {
                UserId = userId,
            };

            await Mediator.Send(command);

            return new WebApiResult();
        }

        [Authorize]
        [HttpGet("chats/{chatId:int}")]
        public async Task<ActionResult> GetChat(
            [FromRoute] int chatId)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            var query = new GetChatVmQuery()
            {
                ChatId = chatId,
                UserId = userId
            };

            var chat = await Mediator.Send(query);

            return new WebApiResult()
            {
                Data = chat,
            };
        }

        [Authorize]
        [HttpGet("chats")]
        public async Task<ActionResult> GetChats()
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            var query = new GetChatPreviewVmsQuery()
            {
                UserId = userId
            };

            var chatPreviewVms = await Mediator.Send(query);

            return new WebApiResult()
            {
                Data = chatPreviewVms
            };
        }

        
        [Authorize]
        [HttpPost("chats/{chatId:int}/send")]
        public async Task<ActionResult> GetResponse(
           [FromRoute] int chatId,
           [FromBody] string request)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            
            var commandRequest = new CreateMessageCommand()
            {
                UserId = userId,
                ChatId = chatId,
                Body = request,
                MessageType = MessageType.Request
            };

            await Mediator.Send(commandRequest);

            //receive a response from OpenAI Api
            var response = "Vse okay";

            var commandResponse = new CreateMessageCommand()
            {
                UserId = userId,
                ChatId = chatId,
                Body = response,
                MessageType = MessageType.Response
            };

            await Mediator.Send(commandResponse);

            return new WebApiResult()
            {
                Data = response,
            };
        }

    }
}
