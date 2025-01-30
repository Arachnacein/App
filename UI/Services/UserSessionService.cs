namespace UI.Services
{
    public class UserSessionService
    {
        public string Token { get; private set; }
        public Guid UserId { get; private set; }
        public string Name { get; private set; }
        public string Surname { get; private set; }
        public string Username { get; private set; }
        public string Email { get; private set; }
        public DateTime AccountCreatedDate { get; set; }
        public DateTime TokenExpiryDate { get; set; }
        public bool EmailVerified { get; set; }
        public bool IsAdmin { get; set; }
        public List<string> Roles { get; private set; } = new List<string>();

        public void SetUserSession(string token, List<string> roles, string name, string surname, string username, string email, Guid userId, DateTime tokenExpiryDate, DateTime accountCreatedDate, bool emailVerified)
        {
            Token = token;
            Roles = roles;
            Name = name;
            Surname = surname;
            Username = username;
            this.Email = email;
            UserId = userId;
            TokenExpiryDate = tokenExpiryDate;
            AccountCreatedDate = accountCreatedDate;
            EmailVerified = emailVerified;
            IsAdmin = roles.Contains("admin") ? true : false;
        }        
        public void SetUserSession(string name, string surname, string username, string email)
        {
            Name = name;
            Surname = surname;
            Username = username;
            this.Email = email;
        }
        public void SetUserSession(bool emailVerified) => EmailVerified = emailVerified;


        public void ClearUserSession()
        {
            Token = null;
            Roles.Clear();
            Name = null;
            Surname = null;
            Username = null;
            Email = null;
            UserId = Guid.Empty;
            TokenExpiryDate = DateTime.MinValue;
            AccountCreatedDate = DateTime.MinValue;
            EmailVerified = false;
            IsAdmin = false;
        }

        public bool IsUserLoggedIn() => !string.IsNullOrEmpty(Token);
        public string GetRemainingTime()
        {
            var remainingTime = TokenExpiryDate - DateTime.Now;
            return remainingTime.ToString(@"hh\:mm\:ss");
        }
    }
}