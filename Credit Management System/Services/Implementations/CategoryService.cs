using AutoMapper;
using Credit_Management_System.Models;
using Credit_Management_System.Repositories.Interfaces;
using Credit_Management_System.Services.Interfaces;
using Credit_Management_System.ViewModels.Category;

namespace Credit_Management_System.Services.Implementations
{
    public class CategoryService : GenericService<CategoryVM, Category>, ICategoryService
    {
        public CategoryService(IGenericRepository<Category> repository, IMapper mapper)
           : base(repository, mapper)
        {
        }

      
    }
}
