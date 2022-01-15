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
    public class CoursesController : CustomeBasController
    {
        private readonly ICourseService _coursesService;

        public CoursesController(ICourseService coursesService)
        {
            _coursesService = coursesService;
        }

        [HttpGet("getById")]
        //todo category dto null donuyor bakılcak.
        public async Task<IActionResult> GetById(string id)
        {
            var response = await _coursesService.GetByIdCourseAsync(id);
            return CreateActionResultInstance(response);
        }
        [HttpGet]
        [Route("/api/[controller]/GetAllByUserId/{userId}")]
        public async Task<IActionResult> GetByUserId(string userId)
        {
            var response = await _coursesService.GetAllByUserIdAsync(userId);
            return CreateActionResultInstance(response);
        }

        [HttpGet("getAll")]
        public async Task<IActionResult> GetAll()
        {
            var response = await _coursesService.GetAllAsync();
            return CreateActionResultInstance(response);
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create(CourseCreateDto createDto)
        {
            var response = await _coursesService.CreateCourseAsync(createDto);
            return CreateActionResultInstance(response);
        }

        [HttpPut("update")]
        public async Task<IActionResult> Update(CourseUpdateDto createDto)
        {
            var response = await _coursesService.UpdateCourseAsync(createDto);
            return CreateActionResultInstance(response);
        }
        [HttpDelete("delete")]
        public async Task<IActionResult> Delete(string id)
        {
            var response = await _coursesService.DeleteCourseAsync(id);
            return CreateActionResultInstance(response);
        }

    }
}
