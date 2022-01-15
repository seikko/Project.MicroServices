using Course.Services.Catalog.Dtos;
using Course.Shared.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Course.Services.Catalog.Services
{
    public interface ICourseService
    {
        Task<Response<List<CourseDto>>> GetAllAsync();
        Task<Response<CourseDto>> GetByIdCourseAsync(string id);
        Task<Response<List<CourseDto>>> GetAllByUserIdAsync(string id);
        Task<Response<NoContent>> DeleteCourseAsync(string id);
        Task<Response<NoContent>> UpdateCourseAsync(CourseUpdateDto updateDto);
        Task<Response<CourseDto>> CreateCourseAsync(CourseCreateDto course);
    }
}
