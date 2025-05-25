using System.ComponentModel.DataAnnotations;

namespace Credit_Management_System.ViewModels.Category
{
    public class SubCategoryVM
    {
        [Required(ErrorMessage = "Subcategory name is required.")]
        [StringLength(150)]
        public string Name { get; set; }
    }
}
