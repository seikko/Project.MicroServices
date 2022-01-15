using AutoMapper;
using Course.Services.Catalog.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Course.Services.Catalog.Models
{
    public class GenaralMapping:Profile
    {
        public GenaralMapping()
        {
            CreateMap<Courses, CourseDto>().ReverseMap();
            CreateMap<Category, CategoryDto>().ReverseMap();
            CreateMap<Feature, FeatureDto>().ReverseMap();

            CreateMap<Courses, CourseCreateDto>().ReverseMap();
            CreateMap<Courses, CourseUpdateDto>().ReverseMap();

            

            //CreateMap<Category, CreateCategoryDto>().ReverseMap();
            //CreateMap<Category, UpdateCategoryDto>().ReverseMap();


        }
    }
}
