using FluentValidation;
using NLayerApp.Core.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NLayerApp.Service.Validations
{
    public class ProductDtoValidator : AbstractValidator<ProductDto>
    {
        public ProductDtoValidator()
        {
            RuleFor(x => x.Name).NotNull().NotEmpty().WithMessage("{PropertyName} boş geçilemez");
            RuleFor(x => x.Price).InclusiveBetween(0, decimal.MaxValue).WithMessage("Geçerli bir {PropertyName} değeri giriniz");
            RuleFor(x => x.Stock).InclusiveBetween(0, int.MaxValue).WithMessage("Geçerli bir {PropertyName} değeri giriniz");
            RuleFor(x => x.CategoryId).InclusiveBetween(1, int.MaxValue).WithMessage("Geçerli bir {PropertyName} değeri giriniz");
        }
    }
}
