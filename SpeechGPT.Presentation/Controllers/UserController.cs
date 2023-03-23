using Microsoft.AspNetCore.Mvc;

namespace SpeechGPT.Presentation.Controllers
{
    [Route("api/user")]
    public class UserController : BaseController
    {
        public async IActionResult Register(UserRegisterDto request)
        {

        }
    }
}
