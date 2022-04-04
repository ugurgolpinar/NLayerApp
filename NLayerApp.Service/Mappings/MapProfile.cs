using AutoMapper;
using NLayerApp.Core.Dtos;
using NLayerApp.Core.Entities;

namespace NLayerApp.Service.Mappings
{
    public class MapProfile : Profile
    {
        public MapProfile()
        {
            CreateMap<Category, CategoryDto>().ReverseMap();
            CreateMap<Category, CategoryWithProductsDto>();

            CreateMap<Product, ProductDto>().ReverseMap();
            CreateMap<ProductUpdateDto, Product>();
            CreateMap<Product, ProductWithCategoryDto>();

            CreateMap<ProductFeature, ProductFeatureDto>().ReverseMap();
        }
    }
}
