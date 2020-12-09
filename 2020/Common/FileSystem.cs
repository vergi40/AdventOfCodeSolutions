using System;
using System.IO;
using System.Reflection;

namespace Common
{
    public class FileSystem
    {
        public static string GetSolutionPath()
        {
            // Hack
            var exePath = Path.GetDirectoryName(AppDomain.CurrentDomain.BaseDirectory);
            var solution = Path.Combine(exePath, @"..\..\..\..");
            return Path.GetFullPath(solution);
        }
    }
}
