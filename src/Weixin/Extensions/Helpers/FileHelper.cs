using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Myvas.AspNetCore.Weixin.Helpers;

public static class FileHelper
{
    /// <summary>
    /// 根据完整文件路径获取FileStream
    /// </summary>
    /// <param name="fileName"></param>
    /// <returns></returns>
    public static FileStream GetFileStream(string fileName)
    {
        FileStream fileStream = null;
        if (!string.IsNullOrEmpty(fileName) && File.Exists(fileName))
        {
            fileStream = new FileStream(fileName, FileMode.Open);
        }
        return fileStream;
    }
}
