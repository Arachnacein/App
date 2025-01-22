namespace UI.Models
{
    public class CaptchaResult
    {
        public bool Success { get; set; }
        public string ChallengeTs { get; set; }
        public string Hostname { get; set; }
    }
}