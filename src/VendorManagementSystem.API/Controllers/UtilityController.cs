using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VendorManagementSystem.API.Utilities;
using VendorManagementSystem.Application.Dtos.ModelDtos.PurchaseOrder;
using VendorManagementSystem.Application.Dtos.UtilityDtos;
using VendorManagementSystem.Application.IServices;
using VendorManagementSystem.Infrastructure.Services;

namespace VendorManagementSystem.API.Controllers
{

    [ApiController]
    [Route("/utility")]

    public class UtilityController : ControllerBase
    {
        
        private readonly IUtilityService _utilityService;
        private readonly IFileStorageService _fileStorageService;

        private readonly IDigitalSign _digitalSign;

        public UtilityController(IDigitalSign digitalSign, IUtilityService utilityService,IFileStorageService fileStorageService)
        {
            _utilityService = utilityService;
            _fileStorageService = fileStorageService;
            _digitalSign = digitalSign;
        }


        [HttpGet]
        [Route("check")]
        public ActionResult Health()
        {
            var response = new ApplicationResponseDto<object>();
            response.Data = "server is running";
            return StatusCode(ResponseUtility.GetStatusCode(response.Error), response);
        }


        [HttpGet]
        [Route("get-count/{tab}")]
        [Authorize(Roles = "admin,superadmin")]
        [Authorize(Roles = "admin,superadmin")]
        public ActionResult GetCount(string tab)
        {
            var response = new ApplicationResponseDto<CountDto>();
            if (tab == "vendor") { response = _utilityService.VendorCount(); }
            else if (tab == "invoice") { response = _utilityService.InvoiceCount(); }
            return StatusCode(ResponseUtility.GetStatusCode(response.Error), response);
        }


        [HttpPost]
        [Route("pdf-verification")]
        public async  Task<ActionResult> VerifyPdf([FromForm] IFormFile file)
        {
            Console.WriteLine("ruinning");

            Console.WriteLine(file.FileName);
            var response = await _digitalSign.VerifySignature(file);
            return StatusCode(ResponseUtility.GetStatusCode(response.Error), response);
        }


        [HttpGet]
        [Route("get-propertyNames")]
        [Authorize(Roles = "admin,superadmin")]
        [Authorize(Roles = "admin,superadmin")]
        public ActionResult GetPropertyNames([FromQuery] string type)
        {
           var response = _utilityService.ExtractPropertyNames(type);
            return StatusCode(ResponseUtility.GetStatusCode(response.Error), response);
        }

        [HttpPost]
        [Route("generate-pdf")]
        [Authorize(Roles = "admin,superadmin")]
        [Authorize(Roles = "admin,superadmin")]
        public ActionResult GeneratePDF([FromBody] PdfGenerationDto poPdfGeneration)
        {
            return Ok(poPdfGeneration);
        }

        [HttpPost]
        [Route("save-signature")]
        [Authorize(Roles = "superadmin")]
        public ActionResult SaveSignature([FromBody] string signature)
        {
            var response = _utilityService.SaveCanvasSignature(signature);
            return StatusCode(ResponseUtility.GetStatusCode(response.Error), response);
        }

        [HttpGet]
        [Route("dashboard")]
        [Authorize(Roles = "superadmin,admin")]
        public ActionResult DashBoardData()
        {
            var response = _utilityService.DashBoardData();
            return StatusCode(ResponseUtility.GetStatusCode(response.Error), response);
        }

        [HttpGet]
        [Route("containers")]
        public async  Task<ActionResult> GetAllCOntainers()
        {
            var response = await _fileStorageService.GetAllContainers();
            if(response != null)
            return StatusCode(200,response);
            else return NotFound();
        }
    }
}
