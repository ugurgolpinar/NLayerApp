using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using NLayerApp.Core.Dtos;
using NLayerApp.Core.Entities;
using NLayerApp.Core.Repositories;
using NLayerApp.Core.Services;
using NLayerApp.Core.UnitOfWorks;
using NLayerApp.Service.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace NLayerApp.Caching
{
    public class ProductServiceCaching : IProductService
    {
        private const string ProductsCacheKey = "productsCache";
        private readonly IMapper _mapper;
        private readonly IMemoryCache _memoryCache;
        private readonly IProductRepository _productRepository;
        private readonly IUnitOfWork _unitOfWork;

        public ProductServiceCaching(IUnitOfWork unitOfWork, IProductRepository productRepository, IMemoryCache memoryCache, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _productRepository = productRepository;
            _memoryCache = memoryCache;
            _mapper = mapper;

            if (!_memoryCache.TryGetValue(ProductsCacheKey, out _))
            {
                _memoryCache.Set(ProductsCacheKey, _productRepository.GetProductsWithCategoryAsync().Result);
            }
        }

        public async Task<Product> AddAsync(Product entity)
        {
            await _productRepository.AddAsync(entity);
            await _unitOfWork.CommitAsync();
            await CacheProducts();

            return entity;
        }

        public async Task<IEnumerable<Product>> AddRangeAsync(IEnumerable<Product> entities)
        {
            await _productRepository.AddRangeAsync(entities);
            await _unitOfWork.CommitAsync();
            await CacheProducts();

            return entities;
        }

        public Task<bool> AnyAsync(Expression<Func<Product, bool>> expression)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Product>> GetAllAsync()
        {
            return Task.FromResult(_memoryCache.Get<List<Product>>(ProductsCacheKey).AsEnumerable());
        }

        public Task<Product> GetAsync(Expression<Func<Product, bool>> expression)
        {
            var product = _memoryCache.Get<List<Product>>(ProductsCacheKey).FirstOrDefault(expression.Compile());

            if (product == null)
            {
                throw new NotFoundException($"{typeof(Product).Name} bulunamadı");
            }

            return Task.FromResult(product);
        }

        public Task<Product> GetByIdAsync(int id)
        {
            var product = _memoryCache.Get<List<Product>>(ProductsCacheKey).FirstOrDefault(x => x.Id == id);

            if (product == null)
            {
                throw new NotFoundException($"{typeof(Product).Name} ({id}) bulunamadı");
            }

            return Task.FromResult(product);
        }

        public Task<CustomResponseDto<List<ProductWithCategoryDto>>> GetProductsWithCategory()
        {
            var products = _memoryCache.Get<List<Product>>(ProductsCacheKey);
            var productWithCategoryDto = _mapper.Map<List<ProductWithCategoryDto>>(products);

            return Task.FromResult(CustomResponseDto<List<ProductWithCategoryDto>>.Success(200, productWithCategoryDto));
        }

        public async Task RemoveAsync(Product entity)
        {
            _productRepository.Remove(entity);
            await _unitOfWork.CommitAsync();
            await CacheProducts();
        }

        public async Task RemoveRangeAsync(IEnumerable<Product> entities)
        {
            _productRepository.RemoveRange(entities);
            await _unitOfWork.CommitAsync();
            await CacheProducts();
        }

        public async Task UpdateAsync(Product entity)
        {
            _productRepository.Update(entity);
            await _unitOfWork.CommitAsync();
            await CacheProducts();
        }

        public IQueryable<Product> Where(Expression<Func<Product, bool>> expression)
        {
            return _memoryCache.Get<List<Product>>(ProductsCacheKey).Where(expression.Compile()).AsQueryable();
        }

        public async Task CacheProducts()
        {
            _memoryCache.Set(ProductsCacheKey, await _productRepository.GetAll().ToListAsync());
        }
    }
}
