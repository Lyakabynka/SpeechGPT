using MediatR;
using Microsoft.AspNetCore.Mvc;
using SpeechGPT.Application;
using System.Security.Claims;

namespace SpeechGPT.WebApi.Controllers.Base
{
    [ApiController]
    public class BaseController : ControllerBase
    {

        private IMediator _mediator;
        protected IMediator Mediator =>
            _mediator ??= HttpContext.RequestServices.GetRequiredService<IMediator>();

        protected internal int UserId => User.Identity.IsAuthenticated
            ? int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value)
            : throw new Exception(); // todo
    }
}
