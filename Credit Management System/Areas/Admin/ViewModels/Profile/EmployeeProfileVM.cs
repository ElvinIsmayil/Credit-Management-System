using System.ComponentModel.DataAnnotations;

namespace Credit_Management_System.Areas.Admin.ViewModels.Profile
{
    public class EmployeeProfileVM : BaseProfileVM
    {
        [Display(Name = "Branch")]
        public string Branch { get; set; } = null!;
        [Display(Name = "Merchant")]
        public string Merchant { get; set; } = null!;

        [Display(Name = "Salary")]
        public decimal Salary { get; set; }
    }
}
