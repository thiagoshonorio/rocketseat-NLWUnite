using Microsoft.AspNetCore.Mvc;
using PassIn.Application.UseCases.Checkins.DoCheckIn;
using PassIn.Application.UseCases.Events.Register;
using PassIn.Communication.Responses;

namespace PassIn.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CheckInController : ControllerBase
    {
        [HttpPost]
        [Route("{attendeeId}")]
        [ProducesResponseType(typeof(ResponseRegisterJson), StatusCodes.Status201Created)] //Para devolver o codigo se foi sucesso
        [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status404NotFound)] //Para devolver o codigo se não foi sucesso
        [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status409Conflict)]
        public IActionResult CheckIn([FromRoute] Guid attendeeId)
        {
            var useCase = new DoAttendeeCheckInUseCase();
            var response = useCase.Execute(attendeeId);
            return Created(string.Empty, response);

        }

    }
}
