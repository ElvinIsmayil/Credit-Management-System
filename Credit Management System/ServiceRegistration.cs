using Credit_Management_System.Infrastructure.Implementations;
using Credit_Management_System.Infrastructure.Interfaces;
using Credit_Management_System.Infrastructure.Configurations;
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
            // Database Context
            services.AddDbContext<CreditManagementSystemDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            // Http Context Accessor
            services.AddHttpContextAccessor();

            // Repository Layer
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddScoped<IMerchantRepository, MerchantRepository>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<IBranchRepository, BranchRepository>();
            services.AddScoped<IProductRepository, ProductRepository>();

            // Service Layer
            services.AddTransient<IAuthenticationService, AuthenticationService>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<IMerchantService, MerchantService>();
            services.AddScoped<IBranchService, BranchService>();
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IImageService, ImageService>();

            // Identity Configuration
            services.AddIdentity<AppUser, IdentityRole>(options =>
            {
                options.SignIn.RequireConfirmedAccount = true;
                options.User.RequireUniqueEmail = true;
                options.Password.RequiredLength = 8;
                options.Password.RequireDigit = true;
                options.Password.RequireUppercase = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireNonAlphanumeric = true;
            })
            .AddEntityFrameworkStores<CreditManagementSystemDbContext>()
            .AddDefaultTokenProviders();

            // Authentication - Cookie Configurations
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddCookie(options =>
            {
                options.LoginPath = "/Account/SignIn";
                options.AccessDeniedPath = "/Account/AccessDenied";
                options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
                options.SlidingExpiration = true;
                options.Cookie.HttpOnly = true;
                options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
            });

            // Email Service Registration
            services.Configure<SmtpSettings>(configuration.GetSection("SmtpSettings"));
            services.AddSingleton<IEmailService, EmailService>();
        }
    }
}