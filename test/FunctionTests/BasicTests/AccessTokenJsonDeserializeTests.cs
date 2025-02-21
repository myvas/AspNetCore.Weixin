using Myvas.AspNetCore.Weixin;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using Xunit;

namespace Weixin.Tests.FunctionTests.BasicTests
{
    public class JsonDeserializeTests
    {
        [Fact]
        public void AccessTokenJsonDeserialize_ShouldSuccess()
        {
            var json = @"{""access_token"":""89_RnWF3ynfikijP6gaqt_XlOtYCvV6188JYiMQcFvAEu94Ksih8Z3qML-Vio7ZDthRtZDaVqtbF8-W0LTD9Etenkso_nwmhmlEicOXnA7rQpu1ezENvNU6JhFHSpAVUBfAHAYXE"",""expires_in"":7200}";
            var options = new JsonSerializerOptions();
            options.AllowTrailingCommas = true;
            options.IgnoreNullValues = true;
            var result = JsonSerializer.Deserialize<AccessTokenJson>(json, options);

            Assert.True(result.Succeeded);
            Assert.Equal(7200, result.expires_in);
            Assert.Equal("89_RnWF3ynfikijP6gaqt_XlOtYCvV6188JYiMQcFvAEu94Ksih8Z3qML-Vio7ZDthRtZDaVqtbF8-W0LTD9Etenkso_nwmhmlEicOXnA7rQpu1ezENvNU6JhFHSpAVUBfAHAYXE", result.access_token);
        }
    }
}
