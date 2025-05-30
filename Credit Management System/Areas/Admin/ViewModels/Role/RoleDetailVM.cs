namespace Credit_Management_System.Areas.Admin.ViewModels.Role
{
    public class RoleDetailVM
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public int UserCount { get; set; }

        public List<UserInRoleVM> Users { get; set; } = new List<UserInRoleVM>();
    }
}
