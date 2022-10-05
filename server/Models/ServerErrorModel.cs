

public class ServerErrorModel : ResponseModel
{
    public string Error { get; set; }

    public string StackTrace { get; set;}

    public ServerErrorModel(string errorMessage, string message = null, string stackTrace = null) : base(message)
    {
        Error = errorMessage;
        StackTrace = stackTrace;
    }
}