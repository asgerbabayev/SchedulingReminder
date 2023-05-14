using MediatR;
using Microsoft.AspNetCore.RateLimiting;

namespace ShedulingReminders.WebUI.Controllers.Base
{
    /// <summary>
    /// Base controller
    /// </summary>
    [Authorize]
    [Route("[controller]")]
    [ApiController]
    [EnableRateLimiting("Sliding")]
    public class BaseApiController : ControllerBase
    {
        private IMediator _mediator;

        /// <summary>
        /// It is for getting the Mediator instance creation process from the base controller.
        /// </summary>
        protected IMediator Mediator => _mediator ??= HttpContext.RequestServices.GetService<IMediator>();

        /// <summary>
        /// Generates an API response with only the result message.
        /// </summary>
        /// <param name="result">The result object.</param>
        /// <returns>An <see cref="IActionResult"/> representing the API response.</returns>
        [ApiExplorerSettings(IgnoreApi = true)]
        public static IActionResult GetResponseOnlyResultMessage(IResult result)
        {
            return result.Success ? new OkObjectResult(result.Message) : new BadRequestObjectResult(result.Message);
        }

        /// <summary>
        /// Generates an API response with only the result data.
        /// </summary>
        /// <typeparam name="T">The type of the result data.</typeparam>
        /// <param name="result">The result object.</param>
        /// <returns>An <see cref="IActionResult"/> representing the API response.</returns>
        [ApiExplorerSettings(IgnoreApi = true)]
        public static IActionResult GetResponseOnlyResultData<T>(IDataResult<T> result)
        {
            return result.Success ? new OkObjectResult(result.Data) : new BadRequestObjectResult(result.Message);
        }
    }
}
