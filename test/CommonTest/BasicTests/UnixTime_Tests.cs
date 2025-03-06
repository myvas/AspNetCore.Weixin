using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace test;

public class UnixTime_Tests
{
    [Fact]
    public void DateTimeUtcTests()
    {
        Assert.True(-DateTime.UtcNow.Subtract(DateTime.Now.AddHours(-8)).TotalSeconds < 10, "UtcTime = LocalTime - timezone");
        Assert.True(-DateTime.UtcNow.Subtract(DateTime.Now.ToUniversalTime()).TotalSeconds < 10, "UtcTime = LocalTime.ToUniversalTime()");
        Assert.True(-DateTime.UtcNow.ToUnixTimeSeconds()+DateTime.Now.ToUnixTimeSeconds() < 10, "ToUnixTimeSeconds should ignore the timezone");
    }

}
