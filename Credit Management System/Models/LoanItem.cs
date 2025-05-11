using Credit_Management_System.Models.Common;

namespace Credit_Management_System.Models
{
    public class LoanItem : BaseEntity
    {
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalPrice => Quantity * UnitPrice;

        public int LoanId { get; set; }
        public Loan? Loan { get; set; }

        public int ProductId { get; set; }
        public Product? Product { get; set; }

    }
}
