using Microsoft.AspNetCore.Mvc;
using VendorManagementSystem.Application.Dtos.UtilityDtos;
using VendorManagementSystem.Application.Dtos.ModelDtos.Invoice;
using VendorManagementSystem.Infrastructure.Services;
using VendorManagementSystem.Application.IServices;
using Microsoft.AspNetCore.Authorization;
using VendorManagementSystem.API.Utilities;
using VendorManagementSystem.Application.Services;
namespace VendorManagementSystem.API.Controllers
{
    [ApiController]
    [Route("/invoice")]


    public class InvoiceController : ControllerBase
    {
        private readonly IInvoiceService _invoiceService;
        public InvoiceController(IInvoiceService invoiceService,IVendorService vendorService)
        {
            _invoiceService = invoiceService;
        }


        [HttpPost]
        [Route("create-invoice")]
        [Authorize(Roles = "admin,superadmin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        //public ActionResult CreateInvoice([FromForm] CreateInvoiceDto createInvoiceDto)
        public async Task<ActionResult> CreateInvoice([FromForm] CreateInvoiceDto createInvoiceDto)
        {
            if (!ModelState.IsValid)
            {
                var errorResponse = ResponseUtility.ModelError(ModelState);
                return StatusCode(StatusCodes.Status400BadRequest, errorResponse);
            }
             var auth = HttpContext.Request.Headers.Authorization.FirstOrDefault()?.Split(" ");
            string jwt = auth == null ? "" : auth[auth.Length - 1];

           
            var response =  await _invoiceService.CreateInvoice(createInvoiceDto,jwt);
            return StatusCode(ResponseUtility.GetStatusCode(response.Error), response);
        }

        [HttpGet]
        [Route("get-invoices")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult GetInvoices([FromQuery] PaginationDto paginationDto, [FromQuery] string? filter)
        {
            if (!ModelState.IsValid)
            {
                var errorResponse = ResponseUtility.ModelError(ModelState);
                return StatusCode(StatusCodes.Status400BadRequest, errorResponse);
            }
            if (filter == null) filter = string.Empty;
            var response = _invoiceService.GetInvoices(filter, paginationDto.Cursor, paginationDto.Size, paginationDto.Next);
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
            var response = await _invoiceService.GetFile(fileName);
            if (response.Error == null)
            {

                return File(response.Data.Content, response.Data.ContentType, response.Data.Name);
            }
            return StatusCode(ResponseUtility.GetStatusCode(response.Error), new { response.Error, response.Message });
        }

    }
}
