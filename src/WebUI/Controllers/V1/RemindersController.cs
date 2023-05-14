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
        /// Creates a new reminder.
        /// </summary>
        /// <param name="createReminderCommand">The command containing reminder creation data.</param>
        /// <returns>The created reminder data if successful, or an error message otherwise.</returns>
        [AllowAnonymous]
        [Consumes("application/json")]
        [Produces("application/json", "text/plain")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CreateReminderCommand))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CreateReminderCommand createReminderCommand)
        {
            return GetResponseOnlyResultData(await Mediator.Send(createReminderCommand));
        }

        /// <summary>
        /// Deletes reminders.
        /// </summary>
        /// <param name="deleteRemindersCommand">The command containing reminder deletion data.</param>
        /// <returns>The result of the deletion operation.</returns>
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
        [HttpDelete]
        public async Task<IActionResult> Delete([FromForm] DeleteRemindersCommand deleteRemindersCommand)
        {
            return GetResponseOnlyResultMessage(await Mediator.Send(deleteRemindersCommand));
        }

        /// <summary>
        /// Updates a reminder.
        /// </summary>
        /// <param name="updateReminderCommand">The command containing reminder update data.</param>
        /// <returns>The updated reminder data if successful, or an error message otherwise.</returns>
        [Consumes("application/json")]
        [Produces("application/json", "text/plain")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UpdateReminderCommand))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        [HttpPut]
        public async Task<IActionResult> Put([FromBody] UpdateReminderCommand updateReminderCommand)
        {
            return GetResponseOnlyResultData(await Mediator.Send(updateReminderCommand));
        }

        /// <summary>
        /// Retrieves a list of reminders.
        /// </summary>
        /// <returns>The list of reminders if successful, or an error message otherwise.</returns>
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
