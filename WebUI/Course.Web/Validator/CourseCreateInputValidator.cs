using Course.Web.Models;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Course.Web.Validator
{
    public class CourseCreateInputValidator:AbstractValidator<CourseCreateModel >
    { 
        public CourseCreateInputValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Isim alan boş olamaz");
            RuleFor(x => x.Description).NotEmpty().WithMessage("Açıklama alanı boş olamaz");
            RuleFor(x => x.Feature.Duration).InclusiveBetween(1,int.MaxValue).WithMessage("süre boş olamaz");
            RuleFor(x => x.Price).NotEmpty().WithMessage("fiyat alanı boş olamaz").ScalePrecision(2,6).WithMessage("hatalı para formatı");
            //scalePercision toplamı 6 karakter olacak virgülden once 2 karakter virgulden sonra 4 karakter
            RuleFor(x => x.CategoryId).NotEmpty().WithMessage("category alanı seçilmelidir");

        }
    }
}
