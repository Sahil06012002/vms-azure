using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VendorManagementSystem.API.Utilities;
using VendorManagementSystem.Application.Dtos.ModelDtos.Contract;
using VendorManagementSystem.Application.Dtos.UtilityDtos;
using VendorManagementSystem.Application.IServices;

namespace VendorManagementSystem.API.Controllers
{
    [ApiController]
    [Route("/contracts")]
    public class ContractController : ControllerBase
    {
        private readonly IContractService _contractService;

        public ContractController(IContractService contractService)
        {
            _contractService = contractService;
        }

        [HttpPost]
        [Route("add-contract")]
        [Authorize(Roles = "admin,superadmin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> AddContract([FromForm] ContractDto contractDto)
        {
            if (!ModelState.IsValid)
            {
                var errorResponse = ResponseUtility.ModelError(ModelState);
                return StatusCode(StatusCodes.Status400BadRequest, errorResponse);
            }
            var auth = HttpContext.Request.Headers.Authorization.FirstOrDefault()?.Split(" ");
            string jwt = auth == null ? "" : auth[auth.Length - 1];
            var response = await _contractService.AddContract(contractDto, jwt);
            return StatusCode(ResponseUtility.GetStatusCode(response.Error), response);
        }

        [HttpGet]
        [Route("form/getdata")]
        [Authorize(Roles = "admin,superadmin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult GetFormData()
        {
            var response = _contractService.GetContractFormCreationData();
            return StatusCode(ResponseUtility.GetStatusCode(response.Error), response);
        }
        [HttpGet]
        [Route("form/categories/{id}")]
        [Authorize(Roles = "admin,superadmin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult GetVendorCategories(int id)
        {
            var response = _contractService.GetVendorCategories(id);
            return StatusCode(ResponseUtility.GetStatusCode(response.Error), response);
        }

        [HttpGet]
        [Route("download")]
        [Authorize(Roles = "admin,superadmin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> GetFile([FromQuery] string fileName) 
        {
            var response = await _contractService.GetFile(fileName);
            if (response.Error == null || response.Data!=null && response.Data.Content!=null)
            {
                return File(response.Data!.Content!, response.Data.ContentType, response.Data.Name);
            }
            return StatusCode(ResponseUtility.GetStatusCode(response.Error), new { response.Error, response.Message });
        }

        [HttpGet]
        [Route("get-contracts")]
        [Authorize(Roles = "admin,superadmin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult GetAllContracts([FromQuery] PaginationDto paginationDto, string? filter )
        {
            var response = _contractService.GetAllContracts(paginationDto, filter);
            return StatusCode(ResponseUtility.GetStatusCode(response.Error), response);
        }
    }
}
