using ShedulingReminders.Application.Handlers.Reminders.Commands;
using ShedulingReminders.Application.Handlers.Reminders.Queries;
using System.Net;

namespace ShedulingReminders.WebUI.Controllers.V1
{
    /// <summary>
    /// Make it Reminders operations
    /// </summary>
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class RemindersController : BaseApiController
    {
        /// <summary>
        /// Added Reminder
        /// </summary>
        /// <param name="createReminderCommand"></param>
        /// <remarks>
        /// Post
        ///     success: true
        ///     message: this a message box
        /// </remarks>
        /// <returns></returns>
        [Consumes("application/json")]
        [Produces("application/json", "text/plain")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CreateReminderCommand))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        [HttpPost]
        public async Task<IActionResult> Post(
        [FromBody] CreateReminderCommand createReminderCommand)
        {
            return GetResponseOnlyResultData(await Mediator.Send(createReminderCommand));
        }


        [ProducesResponseType(typeof(string), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
        [HttpDelete]
        public async Task<IActionResult> Delete(
        [FromForm] DeleteRemindersCommand deleteRemindersCommand)
        {
            return GetResponseOnlyResultMessage(await Mediator.Send(deleteRemindersCommand));
        }

        [Consumes("application/json")]
        [Produces("application/json", "text/plain")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UpdateReminderCommand))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        [HttpPut]
        public async Task<IActionResult> Put(
        [FromBody] UpdateReminderCommand updateReminderCommand)
        {
            return GetResponseOnlyResultData(await Mediator.Send(updateReminderCommand));
        }

        /// <summary>
        /// List Reminders
        /// </summary>
        /// <remarks>Get Reminders</remarks>
        /// <return>Languages List</return>
        /// <response code="200"></response>
        [Produces("application/json", "text/plain")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<GetRemindersQuery>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        [HttpGet]
        public async Task<IActionResult> GetList()
        {
            return GetResponseOnlyResultData(await Mediator.Send(new GetRemindersQuery()));
        }

    }
}
