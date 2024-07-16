using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VendorManagementSystem.API.Utilities;
using VendorManagementSystem.Application.Dtos.ModelDtos.Item;
using VendorManagementSystem.Application.IServices;

namespace VendorManagementSystem.API.Controllers
{
    [ApiController]
    [Route("/item")]
    public class ItemController : ControllerBase
    {
        private readonly IItemService _itemService;
        public ItemController(IItemService itemService)
        {
            _itemService = itemService;
        }

        [HttpGet]
        [Authorize(Roles ="superadmin,admin")]
        [Route("form-data")]

        public ActionResult GetItemFormDetails()
        {
            var response = _itemService.GetItemFormDetails();
            return StatusCode(ResponseUtility.GetStatusCode(response.Error), response);
        }


        [HttpPost]
        [Authorize(Roles = "superadmin,admin")]
        [Route("create-item")]
        public ActionResult AddItems([FromBody] ItemDto itemDto)
        {
            if(!ModelState.IsValid)
            {
                var errorResponse = ResponseUtility.ModelError(ModelState);
                return StatusCode(StatusCodes.Status400BadRequest, errorResponse);
            }
            var auth = HttpContext.Request.Headers.Authorization.FirstOrDefault()?.Split(" ");
            string jwt = auth == null ? "" : auth[^1];
            var response = _itemService.AddItem(jwt, itemDto);
            return StatusCode(ResponseUtility.GetStatusCode(response.Error), response);

        }
        [HttpGet]
        [Authorize(Roles = "superadmin,admin")]
        [Route("get-items")]

        public ActionResult GetItems()
        {
            var response = _itemService.GetItemService();
            return StatusCode(ResponseUtility.GetStatusCode(response.Error), response);
        }
    }
}
