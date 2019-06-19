using System.IO;
using System.Text.RegularExpressions;

namespace Fate.Utilities
{
    public static class StringExtensions
    {
        public static bool IsValidFileName(this string fileName)
        {
            Regex containsInvalidCharacter = new Regex(
                $"[{Regex.Escape(new string(Path.GetInvalidFileNameChars()))}]");
            return !containsInvalidCharacter.IsMatch(fileName);
        }
    }
}
