using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace SpeechGPT.Presentation.Controllers
{
    [ApiController]
    public class BaseController : ControllerBase
    {
        private IMediator _mediator;

        protected IMediator Mediator =>
            _mediator ??= HttpContext.RequestServices.GetRequiredService<IMediator>();
    }
}
