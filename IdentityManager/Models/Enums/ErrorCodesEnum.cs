namespace IdentityManager.Models.Enums
{
    public enum ErrorCodesEnum
    {
        None = 0,
        GeneralError = 1,
        UsernameAlreadyExists = 1001,
        EmailAlreadyExists = 1002,
        AdminTokenFetchFailed = 2000
    }
}