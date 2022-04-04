using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NLayerApp.Api.Filters;
using NLayerApp.Core.Entities;
using NLayerApp.Core.Services;

namespace NLayerApp.Api.Controllers
{
    public class CategoriesController : CustomBaseController
    {
        private readonly ICategoryService _categoryService;

        public CategoriesController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [ServiceFilter(typeof(NotFoundFilter<Category>))]
        [HttpGet("[action]/{categoryId}")]
        public async Task<IActionResult> GetCategoryByIdWithProducts(int categoryId)
        {
            return CreateActionResult(await _categoryService.GetCategoryByIdWithProductsAsync(categoryId));
        }
    }
}
