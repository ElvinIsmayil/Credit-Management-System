namespace Credit_Management_System.Areas.Admin.ViewModels
{
    public class BreadCrumbItem
    {
        public string Text { get; set; }
        public string Url { get; set; } // Null for the active item
        public bool IsActive { get; set; }
    }
}
