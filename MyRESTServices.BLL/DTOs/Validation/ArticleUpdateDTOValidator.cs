using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyRESTServices.BLL.DTOs.Validation
{
    public class ArticleUpdateDTOValidator : AbstractValidator<ArticleUpdateDTO>
    {
        public ArticleUpdateDTOValidator()
        {
            RuleFor(x => x.CategoryID).NotEmpty().WithMessage("Category is required");
            RuleFor(x => x.Title).NotEmpty().WithMessage("Title is required");
        }
    }
}
