using System.ComponentModel.DataAnnotations;

public enum EmployeePosition
{
    [Display(Name = "Branch Manager")]
    BranchManager,

    [Display(Name = "Loan Officer")]
    LoanOfficer,

    [Display(Name = "Cashier")]
    Cashier
}
