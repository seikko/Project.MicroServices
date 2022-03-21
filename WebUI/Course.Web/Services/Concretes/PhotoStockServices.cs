using Course.Shared.Dtos;
using Course.Web.Models.PhotoStocks;
using Course.Web.Services.Concretes;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Course.Web.Services.Abstract
{
    public class PhotoStockServices : IPhotoStockServices
    {
        private readonly HttpClient _httpClient;

        public PhotoStockServices(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<bool> DeleteImage(string photoUrl)
        {
            var response = await _httpClient.DeleteAsync($"photos?photoUrl={photoUrl}");
            return response.IsSuccessStatusCode;
        }

        public async Task<PhotoViewModel> UploadImage(IFormFile photo)
        {
            if (photo == null || photo.Length <= 0) return null;
            // örnek dosya ismi 543764352543543534.jpg
            var randonFileName = $"{Guid.NewGuid().ToString()}{Path.GetExtension(photo.FileName)}";//path.extension .jpg veriyor
            using var ms = new MemoryStream();
            await photo.CopyToAsync(ms);
            var multiPartContent = new MultipartFormDataContent();
            multiPartContent.Add(new ByteArrayContent(ms.ToArray()), "file", randonFileName);
            var response = await _httpClient.PostAsync("photos", multiPartContent);
            if (!response.IsSuccessStatusCode) return null;
            var result =  await response.Content.ReadFromJsonAsync<Response<PhotoViewModel>>();
            return result.Data;

        }
    }
}
