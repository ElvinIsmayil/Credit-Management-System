namespace Credit_Management_System.ViewModels.Merchant
{
    public class MerchantCreateVM
    {
        public string Name { get; set; }
        public string Adress { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string? ImageUrl { get; set; }
        public IFormFile? Image { get; set; }
    }
}
