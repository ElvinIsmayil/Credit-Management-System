using System.ComponentModel.DataAnnotations;

namespace Credit_Management_System.ViewModels.Branch
{
    public class BranchCreateVM
    {
        [Required(ErrorMessage = "Branch name is required.")]
        [StringLength(150, ErrorMessage = "Branch name cannot exceed 150 characters.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Address is required.")]
        [StringLength(250, ErrorMessage = "Address cannot exceed 250 characters.")]
        public string Address { get; set; }

        [Required(ErrorMessage = "Phone number is required.")]
        [Phone(ErrorMessage = "Invalid phone number format.")]
        [StringLength(20, ErrorMessage = "Phone number cannot exceed 20 characters.")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "MerchantId is required.")]
        public int MerchantId { get; set; }
    }
}
