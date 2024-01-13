namespace MonTraApi.Common;

public class ConstantValue
{
    #region env key
    public const string UserNameEnvKey = "USERNAME";
    public const string PasswordEnvKey = "PASSWORD";
    public const string BaseUrlEnvKey = "BASE_URL";
    public const string DatabaseNameEnvKey = "DATABASE_NAME";
    #endregion

    #region error messsage code
    // common error code
    public const string Err0001 = "err0001";
    public const string Err0002 = "err0002";
    public const string Err0002Message = "Parameter does not valid";

    // error message
    public const string Err1001 = "err1001";
    public const string Err1001Message = "Email or password incorrect";
    public const string Err1002 = "err1002";
    public const string Err1002Message = "User not found";
    #endregion

    #region database collection name
    public const string AccountCollection = "Account";
    public const string UserCollection = "User";
    #endregion

    #region another value
    public const char PasswordHashDelimiter = ';';
    #endregion
}