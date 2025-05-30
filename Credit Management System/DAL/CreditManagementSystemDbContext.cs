using Credit_Management_System.Models;
using Credit_Management_System.Models.Common;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

public class CreditManagementSystemDbContext : IdentityDbContext<AppUser>
{
    public CreditManagementSystemDbContext(DbContextOptions<CreditManagementSystemDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(CreditManagementSystemDbContext).Assembly);

        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            // Check if the entity has an 'IsDeleted' property and inherits from BaseEntity
            // You can refine this check based on your specific BaseEntity setup.
            // Option A: Check if it inherits from BaseEntity
            if (typeof(BaseEntity).IsAssignableFrom(entityType.ClrType))
            {
                // Create the lambda expression: e => !e.IsDeleted
                var parameter = Expression.Parameter(entityType.ClrType, "e");
                var propertyMethod = entityType.ClrType.GetProperty("IsDeleted");
                if (propertyMethod == null) continue; // Should not happen if BaseEntity is used

                var isDeletedProperty = Expression.Property(parameter, propertyMethod);
                var filter = Expression.Lambda(Expression.Not(isDeletedProperty), parameter);

                // Apply the filter
                modelBuilder.Entity(entityType.ClrType).HasQueryFilter(filter);
            }
        }



        }

    public DbSet<Merchant> Merchants { get; set; }
    public DbSet<Branch> Branches { get; set; }

    public DbSet<Employee> Employees { get; set; }
    public DbSet<Customer> Customers { get; set; }

    public DbSet<Category> Categories { get; set; }
    public DbSet<Product> Products { get; set; }

    public DbSet<Loan> Loans { get; set; }
    public DbSet<LoanDetail> LoanDetails { get; set; }
    public DbSet<LoanItem> LoanItems { get; set; }
    public DbSet<Payment> Payments { get; set; }


}
