using Microsoft.AspNetCore.Mvc.ModelBinding;
using VendorManagementSystem.Application.Dtos.UtilityDtos;
using VendorManagementSystem.Application.Exceptions;

namespace VendorManagementSystem.API.Utilities
{
    public static class ResponseUtility
    {
        public static int GetStatusCode(Error? error)
        {
            if (error != null)
            {
                if (error.Code == (int)ErrorCodes.DatabaseError || error.Code == (int)ErrorCodes.InternalError)
                {
                    return StatusCodes.Status500InternalServerError;
                }
                else if (error.Code == (int)ErrorCodes.NotFound)
                {
                    return StatusCodes.Status404NotFound;
                }
                return StatusCodes.Status400BadRequest;
            }
            else
            {
                return StatusCodes.Status200OK;
            }
        }


        public static ApplicationResponseDto<object> ModelError(ModelStateDictionary modelState)
        {
            var errors = modelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
            var errorResponse = new ApplicationResponseDto<object>
            {
                Error = new Error
                {
                    Code = (int)ErrorCodes.InvalidInputFields,
                    Message = errors,
                }
            };
            return errorResponse;
        }
    }
}
