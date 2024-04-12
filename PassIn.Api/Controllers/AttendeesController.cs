using Microsoft.AspNetCore.Mvc;
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
        [ProducesResponseType(typeof(RequestRegisterEventJson), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status409Conflict)]
        public IActionResult Register([FromRoute] Guid eventId, [FromBody] RequestRegisterEventJson request)
        {
            var useCase = new RegisterAttendeeOnEventUseCase();
            var response = useCase.Execute(eventId, request);
            return Created(string.Empty, response);
        }

        [HttpGet]
        [Route("{eventId}")]
        [ProducesResponseType(typeof(ResponseAllAttendeesJson), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status404NotFound)]
        public IActionResult GetAll([FromRoute] Guid eventId, [FromQuery] int pageIndex = 0, [FromQuery] int pageSize = 10,[FromQuery] string nameFilter = null)
        {
            var useCase = new GetAllByEventIdUseCase();
            var response = useCase.Execute(eventId, nameFilter ,pageSize, pageIndex);
            return Ok(response);
        }
    }


}
