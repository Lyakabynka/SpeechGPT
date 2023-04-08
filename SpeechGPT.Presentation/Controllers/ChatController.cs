using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OpenAI.GPT3.ObjectModels.RequestModels;
using SpeechGPT.Application.CQRS.Commands;
using SpeechGPT.Application.CQRS.Queries;
using SpeechGPT.Application.Interfaces;
using SpeechGPT.Domain.Enums;
using SpeechGPT.WebApi.ActionResults;
using SpeechGPT.WebApi.Controllers.Base;
using SpeechGPT.Application.CQRS.Queries.ViewModels;

namespace SpeechGPT.WebApi.Controllers
{
    [Route("api/chats")]
    public class ChatController : BaseController
    {
        private readonly IChatGPT _chatGPT;
        private readonly IRedisCache _cache;

        public ChatController(IChatGPT chatGPT, IRedisCache cache) =>
            (_chatGPT,_cache) = (chatGPT,cache);
        

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
    
            //creates the chat
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
            var chatVm = await _cache.GetCacheData<ChatVm>($"chat:{chatId}");

            if (chatVm is not null)
                return new WebApiResult()
                {
                    Data = chatVm
                };

            var query = new GetChatVmQuery()
            {
                ChatId = chatId,
                UserId = UserId
            };

            chatVm = await Mediator.Send(query);

            await _cache.SetCacheData($"chat:{chatId}", chatVm);
            
            return new WebApiResult()
            {
                Data = chatVm
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
            var commandCreateRequestMessage = new CreateMessageCommand()
            {
                UserId = UserId,
                ChatId = chatId,
                Body = request,
                MessageType = MessageType.Request
            };

            //creating new message from user
            await Mediator.Send(commandCreateRequestMessage);

            //try getting the cached Chat
            var chatVm = await _cache.GetCacheData<ChatVm>($"chat:{chatId}");
            
            //if data in cache was found, add new message there and 
            if (chatVm is not null)
            {
                chatVm.MessageVms.Add(new MessageVm()
                {
                    Body = request,
                    CreatedAt = DateTime.UtcNow,
                    MessageType = MessageType.Request
                });
            }
            //if data in cache was not found, get chat from database and set it to the cache
            else
            {
                var query = new GetChatVmQuery()
                {
                    ChatId = chatId,
                    UserId = UserId
                };
                
                chatVm = await Mediator.Send(query);
            }
            
            //getting response from chatGPT with new message (request) and previous Messages
            var response = await _chatGPT.Handle(request, chatVm);
            
            chatVm.MessageVms.Add(new MessageVm()
            {
                Body = response,
                CreatedAt = DateTime.UtcNow,
                MessageType = MessageType.Response
            });
            
            var commandCreateResponseMessage = new CreateMessageCommand()
            {
                UserId = UserId,
                ChatId = chatId,
                Body = response,
                MessageType = MessageType.Response
            };
            
            //creating new message to user
            await Mediator.Send(commandCreateResponseMessage);
            
            //putting updated data to the cache
            await _cache.SetCacheData($"chat:{chatId}", chatVm);
            
            return new WebApiResult()
            {
                Data = response,
            };
        }
    }
}
