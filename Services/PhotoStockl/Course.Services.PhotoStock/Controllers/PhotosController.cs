using Course.Services.PhotoStock.Dtos;
using Course.Shared.ControllerBases;
using Course.Shared.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Course.Services.PhotoStock.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PhotosController : CustomeBasController
    {
        [HttpPost]  
        public async Task<IActionResult> PhotoSave(IFormFile file, CancellationToken cancellationToken)
        {
            if (file != null && file.Length > 0)
            {
                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/photos", file.FileName);
                using var stream = new FileStream(path, FileMode.Create);
                await file.CopyToAsync(stream, cancellationToken);
                var returnPath =  file.FileName;
                    
                PhotoDto photoDto = new() { Url = returnPath };
                return CreateActionResultInstance(Response<PhotoDto>.Success(photoDto, 200));
            }
            return CreateActionResultInstance(Response<PhotoDto>.Fail("photo is empt", 400));
        }

        public IActionResult PhotoDelete(string photoUrl)
        {
            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/photos", photoUrl);
            if (!System.IO.File.Exists(path))
            {

                return CreateActionResultInstance(Response<NoContent>.Fail("Photo Not Found", 404));
            }
            System.IO.File.Delete(path);
            return CreateActionResultInstance(Response<NoContent>.Success(204));

        }
    }
}
