using System.Collections.Generic;

namespace Myvas.AspNetCore.Weixin.EntityFrameworkCore.InMemory.Test
{
    public interface ITestLogger
    {
        IList<string> LogMessages { get; }
    }
}