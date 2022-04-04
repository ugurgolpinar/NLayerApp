using NLayerApp.Core.Dtos;
using NLayerApp.Core.Entities;

namespace NLayerApp.Core.Services
{
    public interface ICategoryService : IService<Category>
    {
        Task<CustomResponseDto<CategoryWithProductsDto>> GetCategoryByIdWithProductsAsync(int categoryId);
    }
}
