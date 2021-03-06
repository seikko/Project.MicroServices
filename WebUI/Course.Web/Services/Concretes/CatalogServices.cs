using Course.Shared.Dtos;
using Course.Web.Helpers;
using Course.Web.Models;
using Course.Web.Models.Catalogs;
using Course.Web.Services.Concretes;
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
        private readonly IPhotoStockServices _photoStockServices;
        private readonly PhotoHelper _photoHelper;

        public CatalogServices(HttpClient client, IPhotoStockServices photoStockServices,PhotoHelper photoHelper)
        {
            _client = client;
            _photoStockServices = photoStockServices;
            _photoHelper = photoHelper;
        }


        public async Task<bool> CreateCourseAsync(CourseCreateModel model)
        {
            var resultPhoto = await _photoStockServices.UploadImage(model.PhotoFormFile);
            if(resultPhoto != null)
            {
                model.Picture = resultPhoto.Url;
            }
          

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
            responseSucces.Data.ForEach(x =>
            {
                x.StockPictureUrl = _photoHelper.GetPhotoStockUrl(x.Picture);

            });
            return responseSucces.Data;
        }

        public async Task<List<CourseViewModel>> GetAllCoursesAsync()
        {
            //base url'i startupdan geliyor. http://localhost:5000/services/catalog/courses
            var response = await _client.GetAsync("courses/getall");
            if (!response.IsSuccessStatusCode) return null;
            var responseSucces = await  response.Content.ReadFromJsonAsync<Response<List<CourseViewModel>>>();
            responseSucces.Data.ForEach(x =>
            {
                x.Picture = _photoHelper.GetPhotoStockUrl(x.Picture);
            });
            return responseSucces.Data;
        }

        public async Task<CourseViewModel> GetByCourseIdAsync(string courseId)
        {
            var response = await _client.GetAsync($"courses/{courseId}");
            if (!response.IsSuccessStatusCode) return null;
            var responseSucces = await response.Content.ReadFromJsonAsync<Response<CourseViewModel>>();
            responseSucces.Data.StockPictureUrl = _photoHelper.GetPhotoStockUrl(responseSucces.Data.Picture);
            return responseSucces.Data;
        }
         
        public async Task<bool> UpdateCourseAsync(CourseUpdateModel model)
        {
            var resultPhoto = await _photoStockServices.UploadImage(model.PhotoFormFile);
            if (resultPhoto != null)
            {
                await _photoStockServices.DeleteImage(model.Picture);
                model.Picture = resultPhoto.Url;
            }

            var response = await _client.PutAsJsonAsync<CourseUpdateModel>("courses/update", model);
            return response.IsSuccessStatusCode;
        }
    }
}
