using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PassIn.Application.UseCases.Events.Attendees.GetAllByEventId;
using PassIn.Application.UseCases.Events.RegisterAttendee;
using PassIn.Communication.Requests;
using PassIn.Communication.Responses;

namespace PassIn.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AttendeesController : ControllerBase
    { 
        [HttpPost]
        [Route("{eventId}/register")]
        [ProducesResponseType(typeof(ResponseRegisterJson), StatusCodes.Status201Created)] //Para devolver o codigo se foi sucesso
        [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status404NotFound)] //Para devolver o codigo se não existe
        [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status409Conflict)]
        [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status403Forbidden)]
        public IActionResult Register([FromRoute] Guid eventId, [FromBody] RequestRegisterEventJson request)
        {
            var useCase = new RegisterAttedeeOnEventUseCase();
            var response = useCase.Execute(eventId, request);
            return Created(string.Empty, response);
        }

        [HttpGet]
        [Route("{eventId}")]
        [ProducesResponseType(typeof(ResponseRegisterJson), StatusCodes.Status200OK)] //Para devolver o codigo se foi sucesso
        [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status404NotFound)] //Para devolver o codigo se não existe
        public IActionResult GetAll([FromRoute] Guid eventId)
        {
            var useCase = new GetAllAttendeesByEventIdUseCase();
            var response = useCase.Execute(eventId);
            return Ok(response);
        }
    }
}
