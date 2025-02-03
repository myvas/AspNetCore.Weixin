using System;
using Xunit;

namespace Myvas.AspNetCore.Weixin.EntityFrameworkCore.InMemory.Test
{
    public class InMemoryWeixinSubscriberStoreTest : WeixinSpecificationTestBase<WeixinSubscriber, string>, IClassFixture<InMemoryDatabaseFixture>
    {
        private readonly InMemoryDatabaseFixture _fixture;

        public InMemoryWeixinSubscriberStoreTest(InMemoryDatabaseFixture fixture)
        {
            _fixture = fixture;
        }

        protected override object CreateTestContext()
            => InMemoryContext.Create(_fixture.Connection);

        [Fact]
        public void Test1()
        {

        }
    }
}
