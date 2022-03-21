using Course.Web.Models;
using Course.Web.Models.Catalogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Course.Web.Services.Interfaces
{
    public interface ICatalogServices
    {
        Task<List<CourseViewModel>> GetAllCoursesAsync();
        Task<List<CategoryViewModel>> GetAllCategoryAsync();
        Task<List<CourseViewModel>> GetAllCourseByUserIdAsync(string userId);
        Task<CourseViewModel> GetByCourseIdAsync(string courseId);
        Task<bool> DeleteCourseAsync(string courseId);
        Task<bool> CreateCourseAsync(CourseCreateModel model);
        Task<bool> UpdateCourseAsync(CourseUpdateModel model);

    }
}
