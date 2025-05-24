using AutoMapper;
using Credit_Management_System.Models;
using Credit_Management_System.Repositories.Interfaces;
using Credit_Management_System.Services.Interfaces;
using Credit_Management_System.ViewModels.Branch;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Credit_Management_System.Services.Implementations
{
    public class BranchService : GenericService<BranchVM, Branch>, IBranchService
    {
        private readonly IBranchRepository _branchRepository;

        public BranchService(IBranchRepository branchRepository, IMapper mapper) : base(branchRepository, mapper)
        {
            _branchRepository = branchRepository;
            
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

            if (branch == null)
                return null;

            var branchDetailVM = _mapper.Map<BranchDetailVM>(branch);
            return branchDetailVM;
        }
    }
}
