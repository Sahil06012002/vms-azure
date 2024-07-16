using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VendorManagementSystem.API.Utilities;
using VendorManagementSystem.Application.Dtos.ModelDtos.VendorDtos;
using VendorManagementSystem.Application.Dtos.UtilityDtos;
using VendorManagementSystem.Application.IServices;



namespace VendorManagementSystem.API.Controllers
{
    [ApiController]
    [Route("/vendor")]
    public class VendorController : ControllerBase
    {

        private readonly IVendorService _vendorService;

        public VendorController(IVendorService vendorService)
        {
            _vendorService = vendorService;
        }


        [HttpPost]
        [Route("create-vendor")]
        [Authorize(Roles = "admin, superadmin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult CreateVendor([FromBody] CreateVendorNewDto vendornewDto)
        {
            //Console.WriteLine("***********", vendornewDto);
            //Console.Write(vendornewDto.AddressType);
            if (!ModelState.IsValid)
            {
                var errorResponse = ResponseUtility.ModelError(ModelState);
                return StatusCode(StatusCodes.Status400BadRequest, errorResponse);
            }
            var auth = HttpContext.Request.Headers.Authorization.FirstOrDefault()?.Split(" ");
            string jwt = auth == null ? "" : auth[auth.Length - 1];
            var response = _vendorService.CreateVendor(vendornewDto, jwt);

            return StatusCode(ResponseUtility.GetStatusCode(response.Error), response);

        }


        [HttpGet]
        [Route("get-vendors")]
        [Authorize(Roles = "admin, superadmin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult GetAllVendors([FromQuery] PaginationDto paginationDto, [FromQuery] string? filter)
        {

            var response = _vendorService.GetAllVendors(filter, paginationDto.Cursor, paginationDto.Size, paginationDto.Next);
            return StatusCode(ResponseUtility.GetStatusCode(response.Error), response);
        }


        [HttpGet]
        [Route("get-vendor/{id}")]
        [Authorize(Roles = "admin, superadmin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult GetVendorById(int id)
        {
            var response = _vendorService.GetVendorById(id);
            return StatusCode(ResponseUtility.GetStatusCode(response.Error), response);
        }



        [HttpPatch]
        [Route("update-vendor/{id}")]
        [Authorize(Roles = "admin, superadmin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult UpdateVendors(int id,[FromBody] UpdateVendorNewDto updateDto) {
            if(!ModelState.IsValid)
            {
                var errorResponse = ResponseUtility.ModelError(ModelState);
                return StatusCode(StatusCodes.Status400BadRequest, errorResponse);
            }
            var auth = HttpContext.Request.Headers.Authorization.FirstOrDefault()?.Split(" ");
            string jwt = auth == null ? "" : auth[auth.Length - 1];
            var response = _vendorService.UpdateVendor(updateDto, jwt);
            return StatusCode(ResponseUtility.GetStatusCode(response.Error), response);
        }


        [HttpPost]
        [Route("update-vendor-status/{id}")]
        [Authorize(Roles = "admin, superadmin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult DeactivateVendor(int id)
        {
            if(!ModelState.IsValid)
            {
                var errorResponse = ResponseUtility.ModelError(ModelState);
                return StatusCode(StatusCodes.Status400BadRequest, errorResponse);
            }
            var auth = HttpContext.Request.Headers.Authorization.FirstOrDefault()?.Split(" ");
            string jwt = auth == null ? "" : auth[auth.Length - 1];
            var response = _vendorService.ToogleVendorStatus(id,jwt);
            return StatusCode(ResponseUtility.GetStatusCode(response.Error), response);
        }


        [HttpGet]
        [Route("get-form-creation-data")]
        [Authorize(Roles = "admin, superadmin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult GetVendorFormCreationData()
        {
            var response = _vendorService.GetFormData();
            return StatusCode(ResponseUtility.GetStatusCode(response.Error), response);
        }

    }
}




