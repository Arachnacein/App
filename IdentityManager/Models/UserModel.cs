namespace IdentityManager.Models
{
    public class UserModel
    {
        public Guid UserId { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public DateTime AccountCreatedDate { get; set; }
        public List<string> Roles { get; set; } = new();
        public bool Enabled { get; set; }
        public bool EmailVerified { get; set; }
    }
}
