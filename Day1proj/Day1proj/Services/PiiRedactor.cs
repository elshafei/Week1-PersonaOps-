using System.Text.RegularExpressions;

namespace Day1proj.Services
{
    public static  class PiiRedactor
    {
        private static readonly List<Regex> PiiPatterns = new()
    {
        new(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,}"),
        new(@"\b\+?\d[\d\s().-]{7,}\b"),
        new(@"sk-[A-Za-z0-9]{20,}")
    };

        public static string Redact(string text)
        {
            foreach (var pat in PiiPatterns)
                text = pat.Replace(text, "***");
            return text;
        }
    }
}
