using AutoMapper;
using Credit_Management_System.Models;
using Credit_Management_System.Repositories.Interfaces;
using Credit_Management_System.Services.Interfaces;
using Credit_Management_System.ViewModels.Branch;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Credit_Management_System.Services.Implementations
{
    public class BranchService : IBranchService
    {
        private readonly IBranchRepository _branchRepository;
        private readonly IMapper _mapper;

        public BranchService(IBranchRepository branchRepository, IMapper mapper)
        {
            _branchRepository = branchRepository;
            _mapper = mapper;
        }

        public async Task<BranchVM> AddAsync(BranchCreateVM viewModel)
        {
            var branch = _mapper.Map<Branch>(viewModel);
            var result = await _branchRepository.AddAsync(branch);

            if (result is null) return null;

            var branchVM = _mapper.Map<BranchVM>(result);
            return branchVM;
        }

        public Task<bool> DeleteAsync(int id) => _branchRepository.DeleteAsync(id);

        public async Task<IEnumerable<BranchVM>> GetAllAsync()
        {
            var branches = await _branchRepository.GetAllAsync();

            var branchVMs = _mapper.Map<IEnumerable<BranchVM>>(branches);
            return branchVMs;
        }

        public async Task<List<BranchVM>> GetAllBranchesAsync()
        {
            var branches = await _branchRepository.GetBranchesWithMerchantsAsync();
            var branchVMs = _mapper.Map<List<BranchVM>>(branches);
            return branchVMs; 
        }

        public async Task<BranchDetailVM> GetBranchDetailByIdAsync(int id)
        {
            var branch = await _branchRepository.GetBranchWithDetailsByIdAsync(id);

            if (branch == null) return null;

            var branchDetailVM = _mapper.Map<BranchDetailVM>(branch);
            return branchDetailVM;
        }

        public async Task<BranchVM> GetByIdAsync(int id)
        {
            var branch = await  _branchRepository.GetByIdAsync(id);
            if (branch is null) return null;

            var branchVM = _mapper.Map<BranchVM>(branch);
            return branchVM;
        }

        public async Task<BranchDetailVM> GetDetailByIdAsync(int id)
        {
            var branch = await _branchRepository.GetBranchWithDetailsByIdAsync(id);
            if (branch is null) return null;

            var branchDetailVM = _mapper.Map<BranchDetailVM>(branch);
            return branchDetailVM;
        }

        public async Task<BranchUpdateVM> GetUpdateByIdAsync(int id)
        {
            var branch = await _branchRepository.GetByIdAsync(id);
            if (branch is null) return null;
            var branchUpdateVM = _mapper.Map<BranchUpdateVM>(branch);
            return branchUpdateVM;
        }

        public async Task<BranchVM> UpdateAsync(BranchUpdateVM viewModel)
        {
            var branch = _mapper.Map<Branch>(viewModel);
            var result = await _branchRepository.UpdateAsync(branch);

            if (result is null) return null;
            var branchVM = _mapper.Map<BranchVM>(result);
            return branchVM;
        }
    }
}
