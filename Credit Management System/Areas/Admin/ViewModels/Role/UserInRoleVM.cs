namespace Credit_Management_System.Areas.Admin.ViewModels.Role
{
    public class UserInRoleVM
    {
        public string Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string? ProfileImageUrl { get; set; }
        public DateTime? JoinedDate { get; set; }
    }
}
