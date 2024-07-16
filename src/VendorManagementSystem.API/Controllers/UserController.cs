using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using VendorManagementSystem.API.Utilities;
using VendorManagementSystem.Application.Dtos.ModelDtos;
using VendorManagementSystem.Application.Dtos.UtilityDtos;
using VendorManagementSystem.Application.Exceptions;
using VendorManagementSystem.Application.IServices;

namespace VendorManagementSystem.API.Controllers
{
    [ApiController]
    [Route("/user")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        /*[HttpPost]
        [Route("me")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult LoggedIn() {
            var jwtToken = HttpContext.Request.Headers.Authorization.FirstOrDefault()?.Split(" ").Last();
            var response = _userService.ValidateToken(jwtToken!);
            return StatusCode(ResponseUtility.GetStatusCode(response.Error), response);
        }*/

        [HttpPost]
        [Route("login")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult Login([FromBody] LoginDto loginDto)
        {
            if (!ModelState.IsValid)
            {
                var errorResponse = ResponseUtility.ModelError(ModelState);
                return StatusCode(StatusCodes.Status400BadRequest, errorResponse);
            }
            var response = _userService.Login(loginDto);

            return StatusCode(ResponseUtility.GetStatusCode(response.Error), response);
        }

        [HttpPost]
        [Route("create-user")]
        [Authorize(Roles = "superadmin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult CreateUser([FromBody] CreateUserDto userDto)
        {
            if (!ModelState.IsValid)
            {
                var errorResponse = ResponseUtility.ModelError(ModelState);
                return StatusCode(StatusCodes.Status400BadRequest, errorResponse);
            }

            var auth = HttpContext.Request.Headers.Authorization.FirstOrDefault()?.Split(" ");
            string jwt = auth == null ? "" : auth[auth.Length - 1];
            var response = _userService.CreateUser(userDto, jwt);

            return StatusCode(ResponseUtility.GetStatusCode(response.Error), response);
        }

        [HttpGet]
        [Route("get-users")]
        [Authorize(Roles = "superadmin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult GetAllUsers([FromQuery] PaginationDto paginationDto, [FromQuery] string? filter)
        {
            var response = _userService.GetAllUsers(paginationDto, filter);

            return StatusCode(ResponseUtility.GetStatusCode(response.Error), response);
        }

        [HttpPost]
        [Route("update-password/{token}")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult SetUserPassword([FromBody] UpdatePasswordDto updatePasswordDto, string token)
        {
            if (!ModelState.IsValid)
            {
                var errorResponse = ResponseUtility.ModelError(ModelState);
                return StatusCode(StatusCodes.Status400BadRequest, errorResponse);
            }
            if (token.IsNullOrEmpty())
            {
                return StatusCode(StatusCodes.Status400BadRequest, new ApplicationResponseDto<object>
                {
                    Error = new Error
                    {
                        Code = (int)ErrorCodes.InvalidInputFields,
                        Message = new List<string> { $"{nameof(token)} is Empty or null" },
                    }
                });
            }
            var response = _userService.SetUserPassword(updatePasswordDto, token);

            return StatusCode(ResponseUtility.GetStatusCode(response.Error), response);
        }

        [HttpGet]
        [Route("validate-user/{token}")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult ValidateNewUserToken(string token)
        {
            if (token.IsNullOrEmpty())
            {
                return StatusCode(StatusCodes.Status400BadRequest, new ApplicationResponseDto<object>
                {
                    Error = new Error
                    {
                        Code = (int)ErrorCodes.InvalidInputFields,
                        Message = new List<string> { $"{nameof(token)} is Empty or null" },
                    }
                });
            }
            var response = _userService.ValidateToken(token);
            return StatusCode(ResponseUtility.GetStatusCode(response.Error), response);
        }

        [HttpPost]
        [Route("forget-password")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult SendForgetPasswordEmail([FromBody] ForgotPasswordDto forgetPasswordDto)
        {
            if (!ModelState.IsValid)
            {
                var errorResponse = ResponseUtility.ModelError(ModelState);
                return StatusCode(StatusCodes.Status400BadRequest, errorResponse);
            }
            var response = _userService.SendForgetPasswordEmail(forgetPasswordDto.Email, forgetPasswordDto.RedirectUrl);
            return StatusCode(ResponseUtility.GetStatusCode(response.Error), response);
        }

        // Delete this
        [HttpPost]
        [Route("create-superadmin")]
        public ActionResult CreateSuperUser([FromBody] SuperAdminDto superuserDto)
        {
            if (!ModelState.IsValid)
            {
                var errorResponse = ResponseUtility.ModelError(ModelState);
                return StatusCode(StatusCodes.Status400BadRequest, errorResponse);
            }
            var response = _userService.CreateSuperAdmin(superuserDto);
            return Ok(response);
        }
    }
}
