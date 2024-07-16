using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VendorManagementSystem.API.Utilities;
using VendorManagementSystem.Application.Dtos.ModelDtos.Expenditure;
using VendorManagementSystem.Application.Dtos.UtilityDtos;
using VendorManagementSystem.Application.IServices;
using VendorManagementSystem.Application.Services;


namespace VendorManagementSystem.API.Controllers
{

    [ApiController]
    [Route("/expenditure")]
    public class ExpenditureController : ControllerBase
    {
        private readonly IExpenditureService _expenditureService;

        public ExpenditureController(IExpenditureService expenditureService)
        {
            _expenditureService = expenditureService;
        }

        [HttpPost]
        [Route("create-expenditure/{id}")]
        [Authorize(Roles = "admin, superadmin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult AddExpenditure([FromBody] List<ExpenditureDTO> expenditureDto,int id)
        {
            if (!ModelState.IsValid)
            {
                var errorResponse = ResponseUtility.ModelError(ModelState);
                return StatusCode(StatusCodes.Status400BadRequest, errorResponse);
            }
            var auth = HttpContext.Request.Headers.Authorization.FirstOrDefault()?.Split(" ");
            string jwt = auth == null ? "" : auth[auth.Length - 1];
            var response = _expenditureService.AddExpenditure(expenditureDto, jwt,id);
            return StatusCode(ResponseUtility.GetStatusCode(response.Error), response);
        }


        [HttpGet]
        [Route("get-expenditures/{id}")]
        [Authorize(Roles = "admin,superadmin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult GetAllExpenditure(int id)
        {
            var response = _expenditureService.GetAllExpenditure(id);

            return StatusCode(ResponseUtility.GetStatusCode(response.Error), response);
        }


        
        [HttpGet]
        [Route("get-top-expenditures/{count}")]
        [Authorize(Roles = "admin,superadmin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult GetTopExpenditure(int count)
        {
            var response = _expenditureService.GetTopExpenditure(count);

            return StatusCode(ResponseUtility.GetStatusCode(response.Error), response);
        }
        

    }

}
