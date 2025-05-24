using Credit_Management_System.Models;
using Credit_Management_System.Services.Implementations;
using Credit_Management_System.ViewModels.Branch;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Credit_Management_System.Services.Interfaces
{
    public interface IBranchService : IGenericService<BranchVM, Branch>
    {
        Task<List<BranchVM>> GetAllBranchesAsync();
        Task<BranchDetailVM> GetBranchDetailByIdAsync(int id);


    }
}
