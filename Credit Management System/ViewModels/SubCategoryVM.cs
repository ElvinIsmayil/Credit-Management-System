using System.ComponentModel.DataAnnotations;

namespace Credit_Management_System.ViewModels
{
    public class SubCategoryVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int ProductCount { get; set; }
    }
}
