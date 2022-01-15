using AutoMapper;
using Course.Services.Catalog.Configurations;
using Course.Services.Catalog.Dtos;
using Course.Services.Catalog.Models;
using Course.Shared.Dtos;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Course.Services.Catalog.Services
{
    public class CourseService:ICourseService
    {
        private readonly IMongoCollection<Courses> _courseCollection;
        private readonly IMongoCollection<Category> _categoryCollection;
        private readonly IMapper _mapper;


        public CourseService(IMapper mapper, IDatabaseSettings databaseSettings)
        {
            var client = new MongoClient(databaseSettings.ConnectionString);
            var database = client.GetDatabase(databaseSettings.DatabaseName);
            _courseCollection = database.GetCollection<Courses>(databaseSettings.CourseCollectionName);
            _categoryCollection = database.GetCollection<Category>(databaseSettings.CategoryCollectionName);
            _mapper = mapper;
        }

        public async Task<Response<List<CourseDto>>> GetAllAsync()
        {
            var course = await _courseCollection.Find(course => true).ToListAsync();//true =  hepsini ver 
            if (course.Any())//içerde bir kayıt varsa 
            {
                foreach (var item in course) // coursenin icindeki categoryleri bulup getiriyorum.
                {
                    item.Categories = await _categoryCollection.Find<Category>(x => x.Id == item.CategoryId).FirstAsync();
                }
            }
            else course = new List<Courses>();

            

            return Response<List<CourseDto>>.Success(_mapper.Map<List<CourseDto>>(course), 200);
        }
        public async Task<Response<CourseDto>> GetByIdCourseAsync(string id)
        {
            var course = await _courseCollection.Find<Courses>(x => x.Id == id).FirstOrDefaultAsync();
            if (course == null)
            {
                return Response<CourseDto>.Fail("Course not found", 400);
            }
            course.Categories = await _categoryCollection.Find<Category>(x => x.Id == course.CategoryId).FirstAsync();
            return Response<CourseDto>.Success(_mapper.Map<CourseDto>(course), 200);
        }

        public async Task<Response<List<CourseDto>>> GetAllByUserIdAsync(string id)
        {
            var course = await _courseCollection.Find<Courses>(x => x.UserId == id).ToListAsync();
            if (course.Any())
            {
                foreach (var item in course)
                {
                    item.Categories = await _categoryCollection.Find<Category>(x => x.Id == item.CategoryId).FirstAsync();
                }
            }
            else course = new List<Courses>();
            return Response<List<CourseDto>>.Success(_mapper.Map<List<CourseDto>>(course), 200);
        }

        public async Task<Response<CourseDto>> CreateCourseAsync(CourseCreateDto course)
        {
            var newCourse = _mapper.Map<Courses>(course);
            newCourse.CreatedDate = DateTime.UtcNow;
            await _courseCollection.InsertOneAsync(newCourse);
            return Response<CourseDto>.Success(_mapper.Map<CourseDto>(newCourse), 200);
        }

        public async Task<Response<NoContent>> UpdateCourseAsync(CourseUpdateDto updateDto)
        {
            var updateCourse = _mapper.Map<Courses>(updateDto);
            await _courseCollection.FindOneAndReplaceAsync(x=> x.Id == updateCourse.Id,updateCourse);//bul ve update et 
            if (updateCourse == null)
            {
                return Response<NoContent>.Fail("Course not found", 404);
            }
            return Response<NoContent>.Success(204);// body'si olmayan basirılı durum kodu
        }

        public async Task<Response<NoContent>> DeleteCourseAsync(string id)
        {
            var deletedCourse = await _courseCollection.DeleteOneAsync(x => x.Id == id);
            if (deletedCourse.DeletedCount > 0)
            {
                return Response<NoContent>.Success(204);// body'si olmayan basirılı durum kodu


            }
            return Response<NoContent>.Fail("Course not found", 404);

        }

    }
}
