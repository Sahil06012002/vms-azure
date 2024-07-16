using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using VendorManagementSystem.API.Utilities;
using VendorManagementSystem.Application.Dtos.UtilityDtos;
using VendorManagementSystem.Application.IServices;
using VendorManagementSystem.Application.Dtos.ModelDtos.PurchaseOrderDTO;
using Newtonsoft.Json.Linq;
using VendorManagementSystem.Application.Dtos.ModelDtos.PurchaseOrder;

namespace VendorManagementSystem.API.Controllers
{
    [ApiController]
    [Route("/purchase-order")]
    public class PurchaseOrderController : ControllerBase
    {
        private readonly IPurchaseOrderService _purchaseOrderService;
        private readonly IAddressService _addressService;
        public PurchaseOrderController(IPurchaseOrderService purchaseOrderService, IAddressService addressService)
        {
            _purchaseOrderService = purchaseOrderService;
            _addressService = addressService;
        }
        [HttpGet]
        [Route("form-details")]
        [Authorize(Roles = "admin, superadmin")]
        public ActionResult GetFormDetails()
        {
            var auth = HttpContext.Request.Headers.Authorization.FirstOrDefault()?.Split(" ");
            string jwt = auth == null ? "" : auth[auth.Length - 1];
            var response = _purchaseOrderService.GetPurchaseOrderFormDetails(jwt);
            return StatusCode(ResponseUtility.GetStatusCode(response.Error), response);
        }
        [HttpGet]
        [Route("address/{id}")]
        [Authorize(Roles = "admin, superadmin")]
        public ActionResult GetVendorAddress(int id)
        {
            var response = _addressService.GetVendorAddress(id);
            return StatusCode(ResponseUtility.GetStatusCode(response.Error), response);
        }


        [HttpPost]
        [Route("add-pos")]
        [Authorize(Roles = "admin, superadmin")]
        public ActionResult AddPurchaseOrders([FromBody] PurchaseOrderDto purchaseOrderDto)
        {

            if (!ModelState.IsValid)
            {
                var errorResponse = ResponseUtility.ModelError(ModelState);
                return StatusCode(StatusCodes.Status400BadRequest, errorResponse);
            }
            var auth = HttpContext.Request.Headers.Authorization.FirstOrDefault()?.Split(" ");
            string jwt = auth == null ? "" : auth[auth.Length - 1];
            var response = _purchaseOrderService.AddPurchaseOrder(purchaseOrderDto, jwt);
            return StatusCode(ResponseUtility.GetStatusCode(response.Error), response);

        }

        [HttpGet]
        [Route("get-pos")]
        [Authorize(Roles = "admin, superadmin")]
        public ActionResult GetPurchaseOrders([FromQuery] PaginationDto paginationDto,string? filter)
        {
            if (!ModelState.IsValid)
            {
                var errorResponse = ResponseUtility.ModelError(ModelState);
                return StatusCode(StatusCodes.Status400BadRequest, errorResponse);
            }
            var response = _purchaseOrderService.GetPurchaseOrder(paginationDto, filter);
            return StatusCode(ResponseUtility.GetStatusCode(response.Error),response);
        }

        [HttpDelete]
        [Route("delete-po/{id}")]
        [Authorize(Roles =("admin,superadmin"))]
        public ActionResult DeletePurchaseOrders(int id)
        {
            if (!ModelState.IsValid)
            {
                var errorResponse = ResponseUtility.ModelError(ModelState);
                return StatusCode(StatusCodes.Status400BadRequest, errorResponse);
            }
            var response = _purchaseOrderService.DeletePurchaseOrder(id);
            return StatusCode(ResponseUtility.GetStatusCode(response.Error), response);
        }
        [HttpPost]
        [Route("send-purchaseorder-email")]
        [Authorize(Roles = ("admin,superadmin"))]
        public async Task<ActionResult> GeneratePDF([FromBody] PurchaseEmailDto pdfGenerationDto)
        {
            var auth = HttpContext.Request.Headers.Authorization.FirstOrDefault()?.Split(" ");
            string jwt = auth == null ? "" : auth[auth.Length - 1];
            var response = await _purchaseOrderService.GeneratePDFHTML(pdfGenerationDto, jwt, true);
            return StatusCode(ResponseUtility.GetStatusCode(response.Error), response);
        }

        [HttpGet]
        [Route("download/{id}")]
        [Authorize(Roles = ("admin,superadmin"))]
        public async Task<ActionResult> GeneratePDF(int id)
        {
            var auth = HttpContext.Request.Headers.Authorization.FirstOrDefault()?.Split(" ");
            string jwt = auth == null ? "" : auth[auth.Length - 1];
            var response = await _purchaseOrderService.DownloadPurchaseOrder(id, jwt);
            if (response.Error == null || response.Data != null && response.Data.Content != null)
            {
                return File(response.Data!.Content!, response.Data.ContentType, response.Data.Name);
            }
            return StatusCode(ResponseUtility.GetStatusCode(response.Error), new { response.Error, response.Message });
        }
    }
}


