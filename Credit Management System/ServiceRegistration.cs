﻿using Credit_Management_System.Infrastructure.Implementations;
using Credit_Management_System.Infrastructure.Interfaces;
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
            services.AddScoped<IMerchantRepository, MerchantRepository>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<IBranchRepository, BranchRepository>();
            services.AddScoped<IProductRepository, ProductRepository>();


            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<IMerchantService, MerchantService>();
            services.AddScoped<IBranchService, BranchService>();
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<IUserService, UserService>();

            services.AddScoped<IImageService, ImageService>();



            services.AddIdentity<AppUser, IdentityRole>(options =>
            {
                options.SignIn.RequireConfirmedAccount = true;
                options.User.RequireUniqueEmail = true;

            }).AddEntityFrameworkStores<CreditManagementSystemDbContext>()
            .AddDefaultTokenProviders();

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddCookie(options =>
            {
                options.LoginPath = "/Account/SignIn";
                options.AccessDeniedPath = "/Account/AccessDenied";
            });

            services.AddSingleton<IEmailService>(sp =>
            {
                var config = sp.GetRequiredService<IConfiguration>().GetSection("SmtpSettings");
                return new EmailService(
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
