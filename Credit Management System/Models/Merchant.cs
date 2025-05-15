using Credit_Management_System.Models.Common;

namespace Credit_Management_System.Models
{
    public class Merchant : BaseEntity
    {
        public string Name { get; set; }
        public string Adress { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string? ImageUrl { get; set; }
        public ICollection<Branch> Branches { get; set; } = new List<Branch>();
    }
}
