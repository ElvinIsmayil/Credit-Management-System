using AutoMapper;
using Credit_Management_System.Models;
using Credit_Management_System.Repositories.Interfaces;
using Credit_Management_System.Services.Interfaces;
using Credit_Management_System.ViewModels.Product;

namespace Credit_Management_System.Services.Implementations
{
    public class ProductService : IProductService
    {
        private readonly IGenericRepository<Product> _productRepository;
        private readonly IMapper _mapper;

        public ProductService(IGenericRepository<Product> productRepository, IMapper mapper)
        {
            _productRepository = productRepository;
            _mapper = mapper;
        }


        public async Task<ProductVM> AddAsync(ProductCreateVM viewModel)
        {
            var product = _mapper.Map<Product>(viewModel);
            var result = await _productRepository.AddAsync(product);
            if (result is null) return null;

            var productVM = _mapper.Map<ProductVM>(result);
            return productVM;
        }

        public async Task<bool> DeleteAsync(int id) => await _productRepository.DeleteAsync(id);

        public async Task<IEnumerable<ProductVM>> GetAllAsync()
        {
            var products = await _productRepository.GetAllAsync();
            var productVMs = _mapper.Map<IEnumerable<ProductVM>>(products);
            return productVMs;
        }

        public async Task<ProductVM> GetByIdAsync(int id)
        {
            var product = await _productRepository.GetByIdAsync(id);
            if (product is null) return null;
            var productVM = _mapper.Map<ProductVM>(product);
            return productVM;
        }

        public async Task<ProductDetailVM> GetDetailByIdAsync(int id)
        {
            var product = await _productRepository.GetByIdAsync(id);
            if (product is null) return null;

            var productDetailVM = _mapper.Map<ProductDetailVM>(product);
            return productDetailVM;
        }

        public async Task<ProductUpdateVM> GetUpdateByIdAsync(int id)
        {
            var product = await _productRepository.GetByIdAsync(id);
            if (product is null) return null;

            var productUpdateVM = _mapper.Map<ProductUpdateVM>(product);
            return productUpdateVM;
        }

        public async Task<ProductVM> UpdateAsync(ProductUpdateVM viewModel)
        {
            var product = _mapper.Map<Product>(viewModel);
            var result = await _productRepository.UpdateAsync(product);
            if (result is null) return null;
            var productVM = _mapper.Map<ProductVM>(result);
            return productVM;
        }
    }
}
