using AutoMapper;
using Credit_Management_System.Models;
using Credit_Management_System.Repositories.Interfaces;
using Credit_Management_System.Services.Interfaces;
using Credit_Management_System.ViewModels.Merchant;

namespace Credit_Management_System.Services.Implementations
{
    public class MerchantService : GenericService<MerchantVM, Merchant>, IMerchantService
    {
        public MerchantService(IGenericRepository<Merchant> repository, IMapper mapper) : base(repository, mapper)
        {
        }
    }
}
