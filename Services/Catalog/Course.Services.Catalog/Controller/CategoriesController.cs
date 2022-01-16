using Course.Services.Catalog.Dtos;
using Course.Services.Catalog.Services;
using Course.Shared.ControllerBases;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Course.Services.Catalog.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : CustomeBasController
    {
        private readonly ICategoryService _categoryService;

        public CategoriesController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }
        
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _categoryService.GetAllAsync();
            return CreateActionResultInstance(result);
        }

        [HttpGet("getById")]
        public async Task<IActionResult> GetById(string id)
        {
            var result = await _categoryService.GetByIdCategoryAsync(id);
            return CreateActionResultInstance(result);
        }
        [HttpPost("addCategory")]
        public async Task<IActionResult> Create(CategoryDto createCategoryDto)
        {
            var result = await _categoryService.CreateCategoryAsync(createCategoryDto);

            return CreateActionResultInstance(result);

        }
    }
}
