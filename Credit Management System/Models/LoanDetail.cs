using Credit_Management_System.Models.Common;

namespace Credit_Management_System.Models
{
    public class LoanDetail : BaseEntity
    {
        public decimal TotalAmount { get; set; }
        public decimal AmountPaid { get; set; }
        public decimal RemainingAmount => TotalAmount - AmountPaid;
        public decimal InterestRate { get; set; }
        public DateTime LoanDate { get; set; }
        public DateTime DueDate { get; set; }

        public int LoanId { get; set; }
        public Loan? Loan { get; set; }

    }
}
