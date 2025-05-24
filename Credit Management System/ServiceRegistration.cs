using Credit_Management_System.Models;
using Credit_Management_System.Repositories.Implementations;
using Credit_Management_System.Repositories.Interfaces;
using Credit_Management_System.Services.Implementations;
using Credit_Management_System.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.Cookies;
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
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<IMerchantService, MerchantService>();
            services.AddScoped<IBranchService, BranchService>();
            services.AddScoped<IBranchRepository, BranchRepository>();


            services.AddIdentity<AppUser, IdentityRole>(options =>
            {
                options.SignIn.RequireConfirmedAccount = true;
                options.User.RequireUniqueEmail = true;

            }).AddEntityFrameworkStores<CreditManagementSystemDbContext>()
    .          AddDefaultTokenProviders();

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddCookie(options =>
            {
                options.LoginPath = "/Account/SignIn";
                options.AccessDeniedPath = "/Account/AccessDenied";
            });

            services.AddSingleton<IEmailSender>(sp =>
            {
                var config = sp.GetRequiredService<IConfiguration>().GetSection("SmtpSettings");
                return new EmailSender(
                    config["Host"],
                    int.Parse(config["Port"]),
                    config["Username"],
                    config["Password"],
                    bool.Parse(config["EnableSsl"])
                );
            });

        }
    }
}
