using Credit_Management_System.Enums;
using Credit_Management_System.Models.Common;

namespace Credit_Management_System.Models
{
    public class Loan : BaseEntity
    {
        public LoanStatus LoanStatus { get; set; } = LoanStatus.Pending;

        public int CustomerId { get; set; }
        public Customer Customer { get; set; }

        public int? EmployeeId { get; set; }
        public Employee? Employee { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public decimal TotalAmount { get; set; }

        public LoanDetail LoanDetail { get; set; }

        public ICollection<Payment> Payments { get; set; } = new List<Payment>();
        public ICollection<LoanItem> LoanItems { get; set; } = new List<LoanItem>();

    }
}
