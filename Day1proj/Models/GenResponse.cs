using Newtonsoft.Json.Linq;

namespace Day1proj.Models
{
    public class TokenInfo
    {
        public int Prompt { get; set; }
        public int Completion { get; set; }
    }
    public class GenResponse
    {
        public string Title { get; set; } = "Untitled";
        public string Body { get; set; } = "";
        public string Style { get; set; } = "default";
        public List<string> Citations { get; set; } = new();
        public List<string> Moderation_Flags { get; set; } = new();
        public TokenInfo Tokens { get; set; } = new();
        public int Latency_Ms { get; set; }
        public double Cost_Est { get; set; }
        public string Request_Id { get; set; } = Guid.NewGuid().ToString();

        public static GenResponse FromLLM(string json)
        {
            var root = JObject.Parse(json);
            var message = root["choices"]?[0]?["message"]?["content"]?.ToString();
            var parsed = JObject.Parse(message ?? "{}");

            var usage = root["usage"] as JObject;
            int promptTokens = usage?["prompt_tokens"]?.Value<int>() ?? 0;
            int completionTokens = usage?["completion_tokens"]?.Value<int>() ?? 0;
            double totalTime = usage?["total_time"]?.Value<double>() ?? 0;

            return new GenResponse
            {
                Title = parsed["title"]?.ToString() ?? "Untitled",
                Body = parsed["body"]?.ToString() ?? "",
                Style = parsed["style"]?.ToString() ?? "default",
                Citations = (parsed["citations"] is JArray arr)
                    ? arr.Select(x => x.ToString()).ToList()
                    : new List<string>(),
                Moderation_Flags = new(),
                Tokens = new TokenInfo
                {
                    Prompt = promptTokens,
                    Completion = completionTokens
                },
                Latency_Ms = (int)(totalTime * 1000),
                Cost_Est = Math.Round((promptTokens + completionTokens) * 0.0000015, 4),
                Request_Id = root["id"]?.ToString() ?? Guid.NewGuid().ToString()
            };
        }
    }
}
