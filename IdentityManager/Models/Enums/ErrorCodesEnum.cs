namespace IdentityManager.Models.Enums
{
    public enum ErrorCodesEnum
    {
        None = 0,
        GeneralError = 1,

        UserNotFound = 1000,
        UsernameAlreadyExists = 1001,
        EmailAlreadyExists = 1002,
        InvalidCredentials = 1003,
        UserNotActive = 1004,
        RegistrationFailed = 1005,

        InvalidRefreshToken = 2001,
    }
}
