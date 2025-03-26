namespace WeixinSiteSample.Models;

public class ReturnableViewModel
{
    public string? ReturnUrl { get; set; }
}

public class ReturnableViewModel<TModel>
    where TModel : class
{
    public string? ReturnUrl { get; set; }
    public TModel? Item { get; set; }
}