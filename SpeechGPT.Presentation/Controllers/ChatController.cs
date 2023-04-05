﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SpeechGPT.Application.CQRS.Commands;
using SpeechGPT.Application.CQRS.Queries;
using SpeechGPT.Application.Interfaces;
using SpeechGPT.Domain.Enums;
using SpeechGPT.WebApi.ActionResults;
using SpeechGPT.WebApi.Controllers.Base;
using System.Security.Claims;
using Microsoft.AspNetCore.OutputCaching;
using Microsoft.Extensions.Caching.Memory;
using StackExchange.Redis;

namespace SpeechGPT.WebApi.Controllers
{
    [Route("api/chats")]
    public class ChatController : BaseController
    {
        private readonly IChatGPT _chatGPT;

        public ChatController(IChatGPT chatGPT) =>
            (_chatGPT) = (chatGPT);
        

        /// <summary>
        /// Creates the chat
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// POST /api/chats
        /// </remarks>
        /// 
        /// <response code="200">Success</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="403">Forbidden</response>
        [Authorize]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult> CreateChat()
        {
            var command = new CreateChatCommand()
            {
                UserId = UserId,
            };

            await Mediator.Send(command);

            return new WebApiResult();
        }

        /// <summary>
        /// Gets the chatVm
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// GET /api/chats/1
        /// </remarks>
        /// 
        /// <param name="chatId">Chat id (int) Id of the chat to get info about</param>
        /// <returns>Returns ChatVm</returns>
        /// <response code="200">Success</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="403">Forbidden</response>
        [Authorize]
        [HttpGet("{chatId:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult> GetChat(
            [FromRoute] int chatId)
        {

            var query = new GetChatVmQuery()
            {
                ChatId = chatId,
                UserId = UserId
            };

            var chatVm = await Mediator.Send(query);
            
            //Setting the active current user chat in memoryCache
            //not to get it from database every time
            
            return new WebApiResult()
            {
                Data = chatVm,
            };
        }

        /// <summary>
        /// Get chats
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// GET /api/chats
        /// </remarks>
        /// 
        /// <returns>Returns List of ChatPreviewVm</returns>
        /// <response code="200">Success</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="403">Forbidden</response>
        [Authorize]
        [HttpGet]
        public async Task<ActionResult> GetChats()
        {
            var query = new GetChatPreviewVmsQuery()
            {
                UserId = UserId
            };

            var chatPreviewVms = await Mediator.Send(query);

            return new WebApiResult()
            {
                Data = chatPreviewVms
            };
        }

        /// <summary>
        /// Gets the response from server
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// GET 1/send
        /// </remarks>
        /// 
        /// <returns>Returns the response (string)</returns>
        /// <param name="chatId">Chat id (int) to receive response from</param>
        /// <param name="request">Request/message (string) from the user</param>
        [Authorize]
        [HttpGet("{chatId:int}/send")]
        public async Task<ActionResult> GetResponse(
           [FromRoute] int chatId,
           [FromQuery] string request)
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

            //todo Receive a response based on previous messages in the chat.
            var response = await _chatGPT.GetResponse(request);

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
