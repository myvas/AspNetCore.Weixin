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
    public static async Task<TResult> FromResponseAsync<TResult>(Func<CancellationToken, Task<HttpResponseMessage>> GetResponseAction, CancellationToken cancellationToken = default)
        where TResult : IWeixinErrorJson, new()
    {
        var result = new TResult();

        try
        {
            using var response = await GetResponseAction(cancellationToken);

            if (!response.IsSuccessStatusCode)
            {
                result.ErrorCode = (int)response.StatusCode;
                result.ErrorMessage = $"HTTP request failed with status code: {response.StatusCode}";
                return result;
            }

            var responseData = await response.Content.ReadFromJsonAsync<TResult>(cancellationToken: cancellationToken);
            if (responseData == null)
            {
                result.ErrorCode = (int)HttpStatusCode.InternalServerError;
                result.ErrorMessage = "Failed to deserialize response";
                return result;
            }

            return responseData;
        }
        catch (HttpRequestException httpEx)
        {
#if NET5_0_OR_GREATER
            result.ErrorCode = (int)(httpEx.StatusCode ?? HttpStatusCode.InternalServerError);
#else
            var statusCode = httpEx.Data.Contains("StatusCode")
                ? (HttpStatusCode?)httpEx.Data["StatusCode"]
                : null;
            result.ErrorCode = (int)(statusCode??HttpStatusCode.InternalServerError);
#endif
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
        catch (NotSupportedException supportEx)
        {
            result.ErrorCode = (int)HttpStatusCode.BadRequest;
            result.ErrorMessage = "Invalid response format";
            result.Exception = supportEx;
            return result;
        }
        catch (Exception ex)
        {
            result.ErrorCode = (int)HttpStatusCode.InternalServerError;
            result.ErrorMessage = "Request failed for an unexpected error occurred";
            result.Exception = ex;
            return result;
        }
    }
}