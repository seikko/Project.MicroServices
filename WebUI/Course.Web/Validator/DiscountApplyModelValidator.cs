using Course.Web.Models.Discount;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Course.Web.Validator
{
    public class DiscountApplyModelValidator:AbstractValidator<DiscountApplyModel>
    {
        public DiscountApplyModelValidator()
        {
            RuleFor(x => x.Code).NotEmpty().WithMessage("code boş olamaz");
        }
    }
}
