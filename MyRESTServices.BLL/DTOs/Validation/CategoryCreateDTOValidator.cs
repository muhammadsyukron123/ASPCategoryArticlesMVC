using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyRESTServices.BLL.DTOs.Validation
{
    public class CategoryCreateDTOValidator : AbstractValidator<CategoryCreateDTO>
    {
        public CategoryCreateDTOValidator()
        {
            RuleFor(c => c.CategoryName).NotEmpty().WithMessage("Name is required");
            RuleFor(c => c.CategoryName).MaximumLength(50).WithMessage("Name can not be more than 50 characters");
        }
    }
}
