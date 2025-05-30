using Credit_Management_System.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

public class CreditManagementSystemDbContext : IdentityDbContext<AppUser>
{
    public CreditManagementSystemDbContext(DbContextOptions<CreditManagementSystemDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(CreditManagementSystemDbContext).Assembly);
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
