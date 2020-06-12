using Microsoft.Extensions.Logging;
using System.Linq;
using Xunit;

namespace Myvas.AspNetCore.Weixin.EntityFrameworkCore.InMemory.Test
{
    public static class WeixinResultAssert
    {
        public static void IsSuccess(WeixinResult result)
        {
            Assert.NotNull(result);
            Assert.True(result.Succeeded);
        }

        public static void IsFailure(WeixinResult result)
        {
            Assert.NotNull(result);
            Assert.False(result.Succeeded);
        }

        public static void IsFailure(WeixinResult result, string error)
        {
            Assert.NotNull(result);
            Assert.False(result.Succeeded);
            Assert.Equal(error, result.Errors.First().Description);
        }

        public static void IsFailure(WeixinResult result, WeixinError error = null)
        {
            Assert.NotNull(result);
            Assert.False(result.Succeeded);
            if (error != null)
            {
                Assert.Equal(error.Description, result.Errors.FirstOrDefault()?.Description);
                Assert.Equal(error.Code, result.Errors.FirstOrDefault()?.Code);
            }
        }

        public static void VerifyLogMessage(ILogger logger, string expectedLog)
        {
            var testLogger = logger as ITestLogger;
            if (testLogger != null)
            {
                Assert.Contains(expectedLog, testLogger.LogMessages);
            }
            else
            {
                Assert.False(true, "No logger registered");
            }
        }
    }
}