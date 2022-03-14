using Course.Shared.Dtos;
using Course.Web.Models;
using Course.Web.Models.Catalogs;
using Course.Web.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Course.Web.Services
{
    public class CatalogServices : ICatalogServices
    {
        private readonly HttpClient _client;

        public CatalogServices(HttpClient client)
        {
            _client = client;
        }

        public async Task<bool> CreateCourseAsync(CourseCreateModel model)
        {
            var response = await _client.PostAsJsonAsync<CourseCreateModel>("courses", model);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteCourseAsync(string courseId)
        {
            var response = await _client.DeleteAsync($"courses/{courseId}");
            return response.IsSuccessStatusCode;
        }

        public async Task<List<CategoryViewModel>> GetAllCategoryAsync()
        {
            var response = await _client.GetAsync("categories");
            if (!response.IsSuccessStatusCode) return null;
            var responseSucces = await response.Content.ReadFromJsonAsync<Response<List<CategoryViewModel>>>();
            return responseSucces.Data;
        }

        public async Task<List<CourseViewModel>> GetAllCourseByUserIdAsync(string userId)
        {
            var response = await _client.GetAsync($"courses/GetAllByUserId/{userId}");
            if (!response.IsSuccessStatusCode) return null;
            var responseSucces = await response.Content.ReadFromJsonAsync<Response<List<CourseViewModel>>>();
            return responseSucces.Data;
        }

        public async Task<List<CourseViewModel>> GetAllCoursesAsync()
        {
            //base url'i startupdan geliyor. http://localhost:5000/services/catalog/courses
            var response = await _client.GetAsync("courses");
            if (!response.IsSuccessStatusCode) return null;
            var responseSucces = await  response.Content.ReadFromJsonAsync<Response<List<CourseViewModel>>>();
            return responseSucces.Data;
        }

        public async Task<CourseViewModel> GetByCourseIdAsync(string courseId)
        {
            var response = await _client.GetAsync($"courses/{courseId}");
            if (!response.IsSuccessStatusCode) return null;
            var responseSucces = await response.Content.ReadFromJsonAsync<Response<CourseViewModel>>();
            return responseSucces.Data;
        }

        public async Task<bool> UpdateCourseAsync(CourseUpdateModel model)
        {
            var response = await _client.PutAsJsonAsync<CourseUpdateModel>("courses", model);
            return response.IsSuccessStatusCode;
        }
    }
}
