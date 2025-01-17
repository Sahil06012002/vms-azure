﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VendorManagementSystem.API.Utilities;
using VendorManagementSystem.Application.Dtos.ModelDtos.Category;
using VendorManagementSystem.Application.Dtos.UtilityDtos;
using VendorManagementSystem.Application.IServices;

namespace VendorManagementSystem.API.Controllers
{
    [ApiController]
    [Route("/category")]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpPost]
        [Route("create-category")]
        [Authorize(Roles = "admin, superadmin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult CreateCategory([FromBody] CategoryDto categoryDto)
        {
            if (!ModelState.IsValid)
            {
                var errorResponse = ResponseUtility.ModelError(ModelState);
                return StatusCode(StatusCodes.Status400BadRequest, errorResponse);
            }
            var auth = HttpContext.Request.Headers.Authorization.FirstOrDefault()?.Split(" ");
            string jwt = auth==null?"":auth[auth.Length - 1];
            var response = _categoryService.CreateCategory(categoryDto, jwt);
            return StatusCode(ResponseUtility.GetStatusCode(response.Error), response);
        }

        [HttpGet]
        [Route("get-category/{id}")]
        [Authorize(Roles = "admin,superadmin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult GetCategory(int id)
        {
            var response = _categoryService.GetCategoryById(id);
            return StatusCode(ResponseUtility.GetStatusCode(response.Error), response);
        }

        [HttpGet]
        [Route("get-categories")]
        [Authorize(Roles = "admin,superadmin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult GetAllCategories([FromQuery] PaginationDto paginationDto, [FromQuery] string?filter)
        {
            var response = _categoryService.GetAllCategories(paginationDto, filter);
            
            return StatusCode(ResponseUtility.GetStatusCode(response.Error), response);
        }

        [HttpPut]
        [Route("update-category/{id}")]
        [Authorize(Roles = "admin, superadmin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult UpdateCategory(int id, [FromBody] CategoryDto category)
        {
            var auth = HttpContext.Request.Headers.Authorization.FirstOrDefault()?.Split(" ");
            string jwt = auth == null ? "" : auth[auth.Length - 1];
            var response = _categoryService.UpdateCategoryById(id, category, jwt);
            return StatusCode(ResponseUtility.GetStatusCode(response.Error), response);
        }

        [HttpDelete]
        [Route("delete/{id}")]
        [Authorize(Roles = "admin, superadmin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult DeleteCategory(int id)
        {
            var response = _categoryService.DeleteCategory(id);
            if (response.Error != null)
            {
                return StatusCode(ResponseUtility.GetStatusCode(response.Error), response);
            }
            return StatusCode(StatusCodes.Status200OK, response);
        }
    }
}
