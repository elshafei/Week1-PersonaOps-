using Newtonsoft.Json;
using System.Text;

namespace Week1CLI
{
    internal class Program
    {
        static void Main(string[] args)
        {

            Console.WriteLine("enter input ");
            string input = Console.ReadLine();
            Console.WriteLine("enter persona ");
            string pers = Console.ReadLine();
            var data = new
            {
                inputText = input,
                persona = pers,
                audience = "students",
                provider = "huggingface",
                model = "openai/gpt-oss-20b"
            };

            using var client = new HttpClient();
            var json = JsonConvert.SerializeObject(data);
            var resp = client.PostAsync("https://localhost:7288/api/generate", new StringContent(json, System.Text.Encoding.UTF8, "application/json")).Result;
            resp.EnsureSuccessStatusCode();
            var result = resp.Content.ReadAsStringAsync().Result;




            string outputDir = Path.Combine("outputs");
            Directory.CreateDirectory(outputDir);
            string outPath = Path.Combine(outputDir, $"{Guid.NewGuid()}.json");
            File.WriteAllText(outPath, result, Encoding.UTF8);
            Console.WriteLine($"Saved → {outPath}");
        }
    }
}
