namespace Credit_Management_System.Areas.Admin.ViewModels.Role
{
    public class AssignRoleVM
    {
        public string UserId { get; set; }
        public string RoleName { get; set; }
        public List<UserVM> Users { get; set; } = new List<UserVM>();
        public List<RoleVM> Roles { get; set; } = new List<RoleVM>();
    }
}
