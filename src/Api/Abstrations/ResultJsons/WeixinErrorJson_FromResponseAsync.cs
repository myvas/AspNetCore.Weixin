using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Myvas.AspNetCore.Weixin;

public partial class WeixinErrorJson
{
    /// <summary>
    /// Parse the response message as <see cref="IWeixinErrorJson"/> without exception.
    /// </summary>
    /// <typeparam name="TResult"></typeparam>
    /// <param name="response"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    /// <noexception/>
    public static async Task<TResult> FromResponseAsync<TResult>(Func<CancellationToken, Task<HttpResponseMessage>> getResponseAsync, CancellationToken cancellationToken = default)
        where TResult : IWeixinErrorJson, new()
    {
        var result = new TResult();

        try
        {
            // ConfigureAwait(false) tells the runtime:
            // "You don’t need to return to the original context. Resume on any available thread from the thread pool."
            using var response = await getResponseAsync(cancellationToken).ConfigureAwait(false);

            if (!response.IsSuccessStatusCode)
            {
                result.ErrorCode = (int)response.StatusCode;
                result.ErrorMessage = $"HTTP request failed with status: {response.StatusCode}";
                return result;
            }

            var responseData = await response.Content.ReadFromJsonAsync<TResult>(cancellationToken: cancellationToken).ConfigureAwait(false);
            return responseData ?? throw new JsonException("Null response received");
        }
        catch (OperationCanceledException)
        {
            result.ErrorCode = (int)HttpStatusCode.RequestTimeout;
            result.ErrorMessage = "Request was canceled";
            return result;
        }
        catch (HttpRequestException httpEx)
        {
            result.ErrorCode = GetStatusCode(httpEx);
            result.ErrorMessage = httpEx.Message;
            result.Exception = httpEx;
            return result;
        }
        catch (JsonException jsonEx)
        {
            result.ErrorCode = (int)HttpStatusCode.BadRequest;
            result.ErrorMessage = "Invalid JSON response";
            result.Exception = jsonEx;
            return result;
        }
        catch (NotSupportedException notSuppEx)
        {
            result.ErrorCode = (int)HttpStatusCode.UnsupportedMediaType;
            result.ErrorMessage = "Unsupported response format";
            result.Exception = notSuppEx;
            return result;
        }
        catch (Exception ex)
        {
            result.ErrorCode = (int)HttpStatusCode.InternalServerError;
            result.ErrorMessage = "An unexpected error occurred";
            result.Exception = ex;
            return result;
        }
    }

    private static int GetStatusCode(HttpRequestException httpEx)
    {

#if NET5_0_OR_GREATER
        return (int)(httpEx.StatusCode ?? HttpStatusCode.InternalServerError);
#else
        return (int)(httpEx.Data.Contains("StatusCode")
            ? (HttpStatusCode?)httpEx.Data["StatusCode"]
            : HttpStatusCode.InternalServerError);
#endif
    }
}