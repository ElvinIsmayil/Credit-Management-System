using Credit_Management_System.Models.Common;

namespace Credit_Management_System.Models
{
    public class Payment : BaseEntity
    {
        public decimal Amount { get; set; }
        public DateTime PaymentDate { get; set; }
        public PaymentMethod PaymentMethod { get; set; } = PaymentMethod.Cash;
        public Guid TransactionId { get; set; } = Guid.NewGuid();

        public int LoanId { get; set; }
        public Loan Loan { get; set; }
    }
}
