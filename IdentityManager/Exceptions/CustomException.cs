namespace IdentityManager.Exceptions
{
    public class CustomException : Exception
    {
        public int ErrorCode { get; }
        public CustomException(int errorCode, string msg) : base(msg)
        {
            ErrorCode = errorCode;
        }
    }
}