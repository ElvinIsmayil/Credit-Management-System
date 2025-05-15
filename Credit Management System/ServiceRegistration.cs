using Credit_Management_System.Models;
using Credit_Management_System.Repositories.Implementations;
using Credit_Management_System.Repositories.Interfaces;
using Credit_Management_System.Services.Implementations;
using Credit_Management_System.ViewModels.Merchant;
using Microsoft.EntityFrameworkCore;

namespace Credit_Management_System
{
    public static class ServiceRegistration
    {
        public static void RegisterServices(this IServiceCollection services, IConfiguration configuration) 
        {
            services.AddDbContext<CreditManagementSystemDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddScoped(typeof(IGenericService<,>), typeof(GenericService<,>));
            //services.AddScoped(typeof(GenericService<MerchantVM, Merchant>));
            //services.AddScoped(typeof(GenericService<MerchantCreateVM, Merchant>));
            //services.AddScoped(typeof(GenericService<MerchantUpdateVM, Merchant>));
            //services.AddScoped(typeof(GenericService<MerchantDetailVM, Merchant>));

        }
    }
}
