using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VendorManagementSystem.API.Utilities;
using VendorManagementSystem.Application.Dtos.ModelDtos.Events;
using VendorManagementSystem.Application.Dtos.UtilityDtos;
using VendorManagementSystem.Application.IServices;
using VendorManagementSystem.Application.Services;

namespace VendorManagementSystem.API.Controllers
{
    [ApiController]
    [Route("/event")]
    public class EventController : ControllerBase
    {
        private readonly IEventService _eventService;

        public EventController(IEventService eventService)
        {
            _eventService = eventService;
        }

        [HttpPost]
        [Route("create-event")]
        [Authorize(Roles = "admin, superadmin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult AddEvent([FromBody] EventDTO expenditureDto)
        {
            if (!ModelState.IsValid)
            {
                var errorResponse = ResponseUtility.ModelError(ModelState);
                return StatusCode(StatusCodes.Status400BadRequest, errorResponse);
            }
            var auth = HttpContext.Request.Headers.Authorization.FirstOrDefault()?.Split(" ");
            string jwt = auth == null ? "" : auth[auth.Length - 1];
            var response = _eventService.AddEvent(expenditureDto, jwt);
            return StatusCode(ResponseUtility.GetStatusCode(response.Error), response);
        }


        [HttpGet]
        [Route("get-events")]
        [Authorize(Roles = "admin,superadmin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult GetAllEvents([FromQuery] PaginationDto paginationDto, [FromQuery] string? filter)
        {
            var response = _eventService.GetAllEvents(paginationDto, filter);

            return StatusCode(ResponseUtility.GetStatusCode(response.Error), response);
        }


        [HttpGet]
        [Route("get-top-events/{count}")]
        [Authorize(Roles = "admin,superadmin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult GetTopEvents(int count)
        {
            var response = _eventService.GetTopEvents(count);

            return StatusCode(ResponseUtility.GetStatusCode(response.Error), response);
        }


    }
}
