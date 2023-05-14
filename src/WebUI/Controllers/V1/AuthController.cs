namespace ShedulingReminders.WebUI.Controllers.V1
{

    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class AuthController : BaseApiController
    {

        [AllowAnonymous]
        [Consumes("application/json")]
        [Produces("application/json", "text/plain")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(string))]
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginUserCommand loginUserCommand)
        {
            var result = await Mediator.Send(loginUserCommand);
            return result.Success ? Ok(result) : Unauthorized(result.Message);
        }


        /// <summary>
        /// Registers a new user.
        /// </summary>
        /// <param name="registerUserCommand">The command containing user registration data.</param>
        /// <returns>The result of the registration operation.</returns>
        [AllowAnonymous]
        [Consumes("application/json")]
        [Produces("application/json", "text/plain")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IResult))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(IResult))]
        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterUserCommand registerUserCommand)
        {
            return GetResponseOnlyResultMessage(await Mediator.Send(registerUserCommand));
        }

    }
}
