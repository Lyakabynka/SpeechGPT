using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SpeechGPT.Application.CQRS.Commands;
using SpeechGPT.WebApi.Models.Auth;

namespace SpeechGPT.Presentation.Controllers
{
    [Route("api/user")]
    public class AuthController : BaseController
    {
        private readonly IMapper _mapper;

        public AuthController(IMapper mapper) =>
            _mapper = mapper;

        [Route("register")]
        public async Task<IActionResult> Register(
            [FromBody] RegisterUserDto request)
        {
            var command = _mapper.Map<RegisterUserCommand>(request);

            await Mediator.Send(command);

            return Ok();
        }
    }
}
