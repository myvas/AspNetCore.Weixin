﻿using System.IO;

namespace Myvas.AspNetCore.Weixin.Api.Tests;

public static class TestFile
{
    public static string GetTestFilePath(string fileName)
    {
        var currentDir = Directory.GetCurrentDirectory();
        return Path.Combine(currentDir, "Data", fileName);
    }
    public static string ReadAllText(string fileName)
    {
        var path = GetTestFilePath(fileName);
        return File.ReadAllText(path);
    }
}
