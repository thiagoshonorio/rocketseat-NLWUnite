using Microsoft.AspNetCore.Mvc;
using PassIn.Application.UseCases.Events.GetById;
using PassIn.Application.UseCases.Events.Register;
using PassIn.Communication.Requests;
using PassIn.Communication.Responses;
using PassIn.Exceptions;

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
        [ProducesResponseType(typeof(ResponseRegisterEventJson), StatusCodes.Status201Created)] //Para devolver o codigo se foi sucesso
        [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status400BadRequest)] //Para devolver o codigo se não foi sucesso
        public IActionResult Register([FromBody] RequestEventJson request)
        {
            try
            {
                var useCase = new RegisterEventUseCase();
                var response =  useCase.Execute(request);
                return Created(string.Empty, response);
            }
            //Garantir as mensagens exception se for tratada, criar sua propria exception.
            catch (PassInException ex)
            {
                return BadRequest(new ResponseErrorJson(ex.Message));
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseErrorJson("Unknown error."));
            }
        }

        [HttpGet]
        [Route("{id}")]
        [ProducesResponseType(typeof(ResponseEventJson), StatusCodes.Status200OK)] //Para devolver o codigo se foi sucesso
        [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status404NotFound)] //Para devolver o codigo se não existe
        public IActionResult GetById([FromRoute] Guid id)
        {
            try
            {
                var useCase = new GetEventByIdUseCase();

                var response = useCase.Execute(id);

                return Ok(response);

            }
            //Garantir as mensagens exception se for tratada, criar sua propria exception.
            catch (PassInException ex)
            {
                return NotFound(new ResponseErrorJson(ex.Message));
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseErrorJson("Unknown error."));
            }
        }
    }
}
