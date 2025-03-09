using System;

public static class UriExtensions
{
    public static string GetBaseAddress(this Uri uri)
    {
        return uri.GetLeftPart(UriPartial.Authority);
    }

    public static Uri SetBaseAddress(this Uri uri, Uri baseAddressUri)
    {
        var returnUriBuilder = new UriBuilder(uri)
        {
            Scheme = baseAddressUri.Scheme,
            Host = baseAddressUri.Host,
            Port = baseAddressUri.Port
        };
        return returnUriBuilder.Uri;
    }
    
    public static Uri SetBaseAddress(this Uri uri, string absolutiveUrl)
        => SetBaseAddress(uri, new Uri(absolutiveUrl));
}