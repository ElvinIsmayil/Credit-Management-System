using AutoMapper;
using Credit_Management_System.Models;
using Credit_Management_System.Repositories.Interfaces;
using Credit_Management_System.Services.Interfaces;
using Credit_Management_System.ViewModels.Merchant;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Credit_Management_System.Services.Implementations
{
    public class MerchantService : IMerchantService
    {
        private readonly IMerchantRepository _repository;
        private readonly IMapper _mapper;
        public MerchantService(IMerchantRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }
        public async Task<MerchantVM> AddAsync(MerchantCreateVM viewModel)
        {
            var merchant = _mapper.Map<Merchant>(viewModel);
            var result = await _repository.AddAsync(merchant);
            if (result is null) return null;

            var merchantVM = _mapper.Map<MerchantVM>(result);
            return merchantVM;
        }
        public async Task<bool> DeleteAsync(int id) => await _repository.DeleteAsync(id);
        public async Task<IEnumerable<MerchantVM>> GetAllAsync()
        {
            var merchants = await _repository.GetAllAsync();
            var merchantVMs = _mapper.Map<IEnumerable<MerchantVM>>(merchants);
            return merchantVMs;
        }
        public async Task<MerchantVM> GetByIdAsync(int id)
        {
            var merchant = await _repository.GetByIdAsync(id);
            if (merchant is null) return null;

            var merchantVM = _mapper.Map<MerchantVM>(merchant);
            return merchantVM;
        }
        public async Task<MerchantVM> UpdateAsync(MerchantUpdateVM viewModel)
        {
            var merchant = _mapper.Map<Merchant>(viewModel);
            var result = await _repository.UpdateAsync(merchant);
            if (result is null) return null;

            var merchantVM = _mapper.Map<MerchantVM>(result);

            return merchantVM;
        }

        public async Task<MerchantDetailVM> GetDetailByIdAsync(int id)
        {
            var merchant = await _repository.GetByIdAsync(id);
            if (merchant is null) return null;

            var merchantDetailVM = _mapper.Map<MerchantDetailVM>(merchant);
            return merchantDetailVM;
        }

        public async Task<MerchantUpdateVM> GetUpdateByIdAsync(int id)
        {
            var merchant = await _repository.GetByIdAsync(id);
            if (merchant is null) return null;

            var merchantUpdateVM = _mapper.Map<MerchantUpdateVM>(merchant);
            return merchantUpdateVM;
        }

        public async Task<IEnumerable<MerchantVM>> SearchMerchantAsync(string searchTerm)
        {
            var merchants = await _repository.SearchMerchantAsync(searchTerm);
            var merchantVMs = _mapper.Map<IEnumerable<MerchantVM>>(merchants);
            return merchantVMs;
        }
    }
}
