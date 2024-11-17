namespace UI.Services
{
    public class UserSessionService
    {
        public string Token { get; private set; }
        public List<string> Roles { get; private set; } = new List<string>();
        public string Name { get; private set; }
        public string Surname { get; private set; }
        public string Username { get; private set; }
        public string Email { get; private set; }
        public Guid UserId { get; private set; }

        public void SetUserSession(string token, List<string> roles, string name, string surname, string username, string email, Guid userId)
        {
            Token = token;
            Roles = roles;
            Name = name;
            Surname = surname;
            Username = username;
            Email = email;
            UserId = userId;
        }

        public void ClearUserSession()
        {
            Token = null;
            Roles.Clear();
            Name = null;
            Surname = null;
            Username = null;
            Email = null;
            UserId = Guid.Empty;
        }

        public bool IsUserLoggedIn() => !string.IsNullOrEmpty(Token);
    }
}