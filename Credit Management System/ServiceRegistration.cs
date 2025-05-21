using Credit_Management_System.Models;
using Credit_Management_System.Repositories.Implementations;
using Credit_Management_System.Repositories.Interfaces;
using Credit_Management_System.Services.Implementations;
using Credit_Management_System.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
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

            services.AddIdentity<AppUser, IdentityRole>(options =>
            {
                options.SignIn.RequireConfirmedAccount = false;
                options.User.RequireUniqueEmail = true;

            }).AddEntityFrameworkStores<CreditManagementSystemDbContext>()
    .          AddDefaultTokenProviders();

            services.AddScoped<IEmailSender, EmailSender>();

        }
    }
}
