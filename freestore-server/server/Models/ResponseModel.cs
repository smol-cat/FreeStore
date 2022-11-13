
public class ResponseModel
{
    public string Message { get; set; }

    public ResponseModel(string message = "Success")
    {
        Message = message;
    }
}