using System.Linq;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace Myvas.AspNetCore.Weixin;

public static class WeixinXmlStringNormalizer
{
    /// <summary>
    /// Format the input XML string by cleaning up spaces, unwraping unecessary CDATA.
    /// </summary>
    /// <param name="xmlString"></param>
    /// <param name="saveOptions">Default is SaveOptions.DisableFormatting, which means no CrLf in the ouputs.</param>
    /// <returns></returns>
    public static string Normalize(string xmlString, SaveOptions saveOptions = SaveOptions.DisableFormatting)
    {
        var doc = CleanAndUnwrapCData(xmlString);

        // Return the cleaned and unwrapped XML string
        return doc.ToString(saveOptions);
    }

    private static XDocument CleanAndUnwrapCData(string xmlString)
    {
        // Parse the XML string
        var doc = XDocument.Parse(xmlString);

        var cdataSections = doc.DescendantNodes().OfType<XCData>().ToList();
        foreach (var cdata in cdataSections)
        {
            var content = cdata.Value;

            // Check if the content contains XML markup characters
            if (!content.Contains("<") && !content.Contains(">") && !content.Contains("&"))
            {
                // Unwrap the CDATA section
                cdata.ReplaceWith(new XText(content));
            }
        }

        return doc;
    }
}
