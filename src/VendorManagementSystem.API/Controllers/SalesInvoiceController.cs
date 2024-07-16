using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VendorManagementSystem.API.Utilities;
using VendorManagementSystem.Application.Dtos.ModelDtos.SalesInvoice;
using VendorManagementSystem.Application.Dtos.UtilityDtos;
using VendorManagementSystem.Application.IServices;

namespace VendorManagementSystem.API.Controllers
{
    [ApiController]
    [Route("sales-invoice")]
    public class SalesInvoiceController : ControllerBase
    {
        private readonly ISalesInvoiceService _salesInvoiceService;
        public SalesInvoiceController(ISalesInvoiceService salesInvoiceService)
        {
            _salesInvoiceService = salesInvoiceService;
        }
        [HttpGet]
        [Route("form-details")]
        [Authorize(Roles = "admin, superadmin")]
        public ActionResult GetFormDetails()
        {
            var auth = HttpContext.Request.Headers.Authorization.FirstOrDefault()?.Split(" ");
            string jwt = auth == null ? "" : auth[auth.Length - 1];
            var response = _salesInvoiceService.GetInvoiceFormData(jwt);
            return StatusCode(ResponseUtility.GetStatusCode(response.Error), response);
        }

        [HttpPost]
        [Route("add-invoices")]
        [Authorize(Roles = "admin, superadmin")]
        public async Task<ActionResult> AddSalesInvoices([FromBody] SalesInvoiceDto salesInvoiceDto)
        {
            if (!ModelState.IsValid)
            {
                var errorResponse = ResponseUtility.ModelError(ModelState);
                return StatusCode(StatusCodes.Status400BadRequest, errorResponse);
            }
            var auth = HttpContext.Request.Headers.Authorization.FirstOrDefault()?.Split(" ");
            string jwt = auth == null ? "" : auth[auth.Length - 1];
            var response = await _salesInvoiceService.AddSalesInvoice(salesInvoiceDto,jwt);
            return StatusCode(ResponseUtility.GetStatusCode(response.Error), response);

        }
        [HttpGet]
        [Route("get-invoices")]
        [Authorize(Roles = "admin, superadmin")]
        public ActionResult GetSalesInvoices([FromQuery] PaginationDto paginationDto, string? filter)
        {
            var response = _salesInvoiceService.GetSalesInvoice(paginationDto, filter);
            return StatusCode(ResponseUtility.GetStatusCode(response.Error), response);
        }

        [HttpDelete]
        [Route("delete-invoiec/{id}")]
        [Authorize(Roles = "admin, superadmin")]
        public ActionResult DeleteSalesInvoice(int id)
        {
            if (!ModelState.IsValid)
            {
                var errorResponse = ResponseUtility.ModelError(ModelState);
                return StatusCode(StatusCodes.Status400BadRequest, errorResponse);
            }
            var response = _salesInvoiceService.DeleteSalesInvoice(id);
            return StatusCode(ResponseUtility.GetStatusCode(response.Error), response);
        }

        [HttpGet]
        [Route("download/{id}")]
        [Authorize(Roles = "admin, superadmin")]
        public async Task<ActionResult> DownloadSalesInvoice(int id)
        {
            var auth = HttpContext.Request.Headers.Authorization.FirstOrDefault()?.Split(" ");
            string jwt = auth == null ? "" : auth[auth.Length - 1];
            var response = await _salesInvoiceService.DownloadSalesInvoiceAsync(id, jwt);
            if (response.Error == null || response.Data != null && response.Data.Content != null)
            {
                return File(response.Data!.Content!, response.Data.ContentType, response.Data.Name);
            }
            return StatusCode(ResponseUtility.GetStatusCode(response.Error), new { response.Error, response.Message });
        }
        [HttpPost]
        [Route("send-email")]
        [Authorize(Roles="admin, superadmin")]
        public async Task<ActionResult> SendSalesInvoiceEmail([FromBody] SalesInvoiceEmailDto salesInvoiceEmailDto)
        {
            var auth = HttpContext.Request.Headers.Authorization.FirstOrDefault()?.Split(" ");
            string jwt = auth == null ? "" : auth[auth.Length - 1];
            var response = await _salesInvoiceService.SendSalesInvoiceMailAsync(salesInvoiceEmailDto,jwt);
            return StatusCode(ResponseUtility.GetStatusCode(response.Error), response);
        }
    }
}
