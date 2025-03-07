using Microsoft.AspNetCore.Mvc;

namespace Myvas.AspNetCore.WeixinSiteHost;

[Route("localApi")]
public class LocalApiController : ControllerBase
{
    public IActionResult Get()
    {
        var claims = from c in User.Claims select new { c.Type, c.Value };
        return new JsonResult(claims);
    }
}

