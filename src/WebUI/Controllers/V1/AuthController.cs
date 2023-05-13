

namespace ShedulingReminders.WebUI.Controllers.V1
{
    /// <summary>
    /// Make it Authorization operations
    /// </summary>
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class AuthController : BaseApiController
    {
        /// <summary>
        /// Make it User Login operations
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [Consumes("application/json")]
        [Produces("application/json", "text/plain")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(string))]
        [HttpPost("login")]
        public async Task<IActionResult> Login()
        {
            return Ok();
        }

        /// <summary>
        ///  Make it User Register operations
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [Consumes("application/json")]
        [Produces("application/json", "text/plain")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IResult))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(IResult))]
        [HttpPost("register")]
        public async Task<IActionResult> Register()
        {
            return Ok();
        }
    }
}
