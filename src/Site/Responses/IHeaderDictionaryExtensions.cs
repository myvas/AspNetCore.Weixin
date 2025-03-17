namespace Microsoft.AspNetCore.Http;

/// <summary>
/// For netstandard2.0, We need these extensions to support for setting Cache-Control and Pragma headers in IHeaderDictionary.
/// </summary>
public static class IHeaderDictionaryExtensions
{
    public static void SetCacheControl(this IHeaderDictionary headers, string value)
    {
        //headers.CacheControl = value;
        headers["Cache-Control"] = value;
    }

    public static void SetPragma(this IHeaderDictionary headers, string value)
    {
        //headers.Pragma = value;
        headers["Pragma"] = value;
    }
}