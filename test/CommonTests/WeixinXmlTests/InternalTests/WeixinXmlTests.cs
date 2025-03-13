using Myvas.AspNetCore.Weixin;

namespace Myvas.AspNetCore.Weixin.CommonTests.WeixinXmlTests.InternalTests;

public class WeixinXmlTests
{
    [Fact]
    public void WeixinXml_Serialize()
    {
        string s = @"<xml></xml>";

        var o = new WeixinXml();
        var result = MyvasXmlConvert.SerializeObject(o);
        Assert.Equal(s, result);
    }

    [Theory]
    [InlineData(@"<xml></xml>")]
    [InlineData(@"<xml>\n</xml>")]
    [InlineData(@"<xml>\r</xml>")]
    [InlineData(@"<xml>\t</xml>")]
    [InlineData(@"<xml> </xml>")]
    public void WeixinXml_Deserialize(string s)
    {
        var result = MyvasXmlConvert.DeserializeObject<WeixinXml>(s);
        Assert.NotNull(result);
    }
}