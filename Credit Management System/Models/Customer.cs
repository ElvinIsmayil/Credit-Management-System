using Credit_Management_System.Models.Common;

namespace Credit_Management_System.Models
{
    public class Customer : Person
    {
        public string? PostalCode { get; set; }
        public decimal CreditLimit { get; set; }

        public ICollection<Loan>? Loans { get; set; } = new List<Loan>();
        public ICollection<Payment>? Payments { get; set; } = new List<Payment>();
    }
}
