using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vergiCommon;

namespace _2022_AoC
{
    internal static class Files
    {
        /// <summary>
        /// </summary>
        /// <param name="fileName">E.g. 01 or 01.txt</param>
        /// <returns></returns>
        public static List<string>? GetInputData(string fileName)
        {
            if (!Path.HasExtension(fileName)) fileName += ".txt";
            var path = Path.Combine(GetPath.ThisProject(), "Input", fileName);

            if (!File.Exists(path)) return null;
            var content = File.ReadAllLines(path).ToList();

            if (!content.Any()) return null;
            return content;
        }
    }
}
