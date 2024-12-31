namespace IdentityManager.Models
{
    public class UserModel
    {
        public Guid UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public DateTime AccountCreatedDate { get; set; }
        public DateTime SessionExpiryDate { get; set; }
        public List<string> Roles { get; set; }
    }
}