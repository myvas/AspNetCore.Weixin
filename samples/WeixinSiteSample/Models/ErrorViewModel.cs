namespace WeixinSiteSample.Models;

public class ErrorViewModel
{
    public string? RequestId { get; set; }

    public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);

    public string Message { get => Error?.Message ?? "An error occurred while processing your request."; }

    public Exception? Error { get; set; }
}
