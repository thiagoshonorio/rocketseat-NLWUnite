using Microsoft.AspNetCore.Mvc;
using PassIn.Application.UseCases.Events.GetById;
using PassIn.Application.UseCases.Events.Register;
using PassIn.Application.UseCases.Events.RegisterAttendee;
using PassIn.Communication.Requests;
using PassIn.Communication.Responses;

//Regra API, receber a requisição e passar para o processamento da regra de negócio
namespace PassIn.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventController : ControllerBase
    {
        //Para registrar um evento, precisa receber as informações no corpo da requisição
        //Esperando receber do body da requisição o requesteventjson
        [HttpPost]
        [ProducesResponseType(typeof(ResponseRegisterJson), StatusCodes.Status201Created)] //Para devolver o codigo se foi sucesso
        [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status400BadRequest)] //Para devolver o codigo se não foi sucesso
        public IActionResult Register([FromBody] RequestEventJson request)
        {
            var useCase = new RegisterEventUseCase();
            var response =  useCase.Execute(request);
            return Created(string.Empty, response);

        }

        [HttpGet]
        [Route("{id}")]
        [ProducesResponseType(typeof(ResponseEventJson), StatusCodes.Status200OK)] //Para devolver o codigo se foi sucesso
        [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status404NotFound)] //Para devolver o codigo se não existe
        public IActionResult GetById([FromRoute] Guid id)
        {
            var useCase = new GetEventByIdUseCase();
            var response = useCase.Execute(id);
            return Ok(response);
        }

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
    }
}
