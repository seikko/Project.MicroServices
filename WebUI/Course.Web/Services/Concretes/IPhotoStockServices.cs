using Course.Web.Models.PhotoStocks;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Course.Web.Services.Concretes
{
    public interface IPhotoStockServices
    {
        Task<PhotoViewModel> UploadImage(IFormFile photo);
        Task<bool> DeleteImage(string photoUrl);
    }
}
