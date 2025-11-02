using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text.Json.Nodes;
using System.Text.RegularExpressions;

namespace Day1proj.Services
{

    public static class JsonExtractor
    {
        /// <summary>
        /// Extracts the actual JSON object produced by the LLM from the full Hugging Face response.
        /// </summary>
        public static Dictionary<string, object> Extract(string responseText)
        {
            try
            {
                // Parse the outer response from Hugging Face
                var root = JObject.Parse(responseText);

                var content = root["choices"]?[0]?["message"]?["content"]?.ToString();
                if (string.IsNullOrWhiteSpace(content))
                    throw new Exception("No message content found in LLM response.");

                // content itself contains the JSON (as string)
                try
                {
                    // Try to parse it directly
                    var inner = JObject.Parse(content);
                    return inner.ToObject<Dictionary<string, object>>()!;
                }
                catch
                {
                    // Fallback: extract first JSON block inside content text
                    var match = Regex.Match(content, @"\{[\s\S]*\}");
                    if (!match.Success)
                        throw new Exception("No valid JSON object found inside LLM content.");

                    var inner = JObject.Parse(match.Value);
                    return inner.ToObject<Dictionary<string, object>>()!;
                }
            }
            catch (Exception ex)
            {
                return new Dictionary<string, object>
                {
                    ["title"] = "Parse Error",
                    ["body"] = $"Failed to parse response: {ex.Message}",
                    ["style"] = "error",
                    ["citations"] = new List<string>()
                };
            }
        }

    }
}
