using System.IO;

namespace Myvas.AspNetCore.Weixin.CommonTests;

public static class TestFile
{
    public static string GetTestFilePath(string fileName)
    {
        var currentDir = Directory.GetCurrentDirectory();
        return Path.Combine(currentDir, "Data", fileName);
    }

    public static string ReadTestFile(string fileName)
    {
        var path = GetTestFilePath(fileName);
        return File.ReadAllText(path);
    }
}
