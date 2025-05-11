using Credit_Management_System.Models.Common;

namespace Credit_Management_System.Models
{
    public class Branch : BaseEntity
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }

        public int MerchantId { get; set; }
        public Merchant Merchant { get; set; }

        public ICollection<Employee> Employees { get; set; } = new List<Employee>();
        
    }
}
