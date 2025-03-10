using System;
using Xunit;

namespace Myvas.AspNetCore.Weixin.CommonTests.Fundamentals;

public class UnixTime_Tests
{
    [Fact]
    public void DateTimeUtcTests()
    {
        Assert.True(-DateTime.UtcNow.Subtract(DateTime.Now.AddHours(-8)).TotalSeconds < 10, "UtcTime = LocalTime - timezone");
        Assert.True(-DateTime.UtcNow.Subtract(DateTime.Now.ToUniversalTime()).TotalSeconds < 10, "UtcTime = LocalTime.ToUniversalTime()");
        Assert.True(-DateTime.UtcNow.ToUnixTime() + DateTime.Now.ToUnixTime() < 10, "ToUnixTime should ignore the timezone");
    }

}
