using Credit_Management_System.Models.Common;

namespace Credit_Management_System.Models
{
    public class Employee : Person
    {
        public decimal Salary { get; set; }

        public int BranchId { get; set; }
        public Branch Branch { get; set; }

        public ICollection<Loan> ManagedLoans { get; set; } = new List<Loan>();
    }
}
