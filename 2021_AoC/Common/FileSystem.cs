using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Common
{
    public class FileSystem
    {
        /// <summary>
        /// Returns file path to /2021
        /// </summary>
        /// <returns></returns>
        public static string GetSolutionPath()
        {
            // Hack
            var exePath = Path.GetDirectoryName(AppDomain.CurrentDomain.BaseDirectory);
            var solution = Path.Combine(exePath, @"..\..\..\..");
            return Path.GetFullPath(solution);
        }

        /// <summary>
        /// From \Resources
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public static string GetInputFilePath(string fileName)
        {
            if (!Path.HasExtension(fileName)) fileName += ".txt";
            var path = Path.Combine(GetSolutionPath(), "Resources", fileName);

            if (!File.Exists(path)) throw new ArgumentException($"No file {fileName} exists in {path}");
            return path;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileName">E.g. 01 or 01.txt</param>
        /// <returns></returns>
        public static List<string>? GetInputData(string fileName)
        {
            if (!Path.HasExtension(fileName)) fileName += ".txt";
            var path = Path.Combine(GetSolutionPath(), "Resources", fileName);

            if (!File.Exists(path)) return null;
            var content = File.ReadAllLines(path).ToList();

            if (!content.Any()) return null;
            return content;
        }
    }
}
