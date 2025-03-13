using System;

namespace Myvas.AspNetCore.Weixin.CommonTests.Fundamentals;

#pragma warning disable CS0464

public class Nullable_Tests
{
    static bool IsNullable(Type type) => Nullable.GetUnderlyingType(type) != null;
    static bool IsOfNullableType<T>(T o)
    {
        var type = typeof(T);
        return Nullable.GetUnderlyingType(type) != null;
    }

    [Fact]
    public void NullTests()
    {
        int? a = 10;
        Assert.False(a >= null, "10 >= null");
        Assert.False(a == null, "10 == null");
        Assert.False(a < null, "10 < null");

        int? b = null;
        int? c = null;
        Assert.False(b>=c, "null >= null");
        Assert.True(b==c, "null == null");

        Assert.False(IsNullable(typeof(string)), "string is non-nullable value type");
        // The typeof operator cannot be used on a nullable reference type.
        // Assert.True(IsNullable(typeof(string?)), "string? is nullable value type");

        Assert.True(IsOfNullableType(a), "int? is of nullable type");
        int d = 17;
        Assert.False(IsOfNullableType(d), "int is not of nullable type");
    }
}
