namespace Credit_Management_System.ViewModels.Branch
{
    public class BranchDetailVM
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Address { get; set; }

        public string PhoneNumber { get; set; }

        public int MerchantId { get; set; }

        public string MerchantName { get; set; }  // for displaying related Merchant info

        // Optional collections summaries (just counts or basic info)
        public int? EmployeeCount { get; set; }
        public int? ProductCount { get; set; }
    }
}
