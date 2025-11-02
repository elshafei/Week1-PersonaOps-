using Day1proj.Configs;
using Day1proj.Models;
using Day1proj.PromptTemplte;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Buffers.Text;
using System.Diagnostics;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json.Nodes;
using static System.Net.WebRequestMethods;

namespace Day1proj.Services
{
    public class HuggingFaceService
    {

     

        private readonly HuggingFaceSettings _settings;

        public HuggingFaceService(IOptions<HuggingFaceSettings> settings)
        {
            _settings = settings.Value;
        }


        public async Task<GenResponse> GenerateAsync(GenRequest req)
        {
            //var stopwatch = Stopwatch.StartNew();
            using var http = new HttpClient();
            http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _settings.Token);

            var prompt = RenderTemplate(req.Persona, req.InputText, req.Length);

            var messages = new[] { new { role = "user", content = prompt } };
            var body = new { messages, model = _settings.Model, stream = false };
            var json = JsonConvert.SerializeObject(body);

          var resp = await http.PostAsync(_settings.ApiUrl, new StringContent(json, Encoding.UTF8, "application/json"));
            var content = await resp.Content.ReadAsStringAsync();

            return GenResponse.FromLLM(content);
            

        }

            
        

        private string RenderTemplate(string persona, string userInput, int length)
        {
            persona = persona.ToLowerInvariant();
            if (!PrompetTemp.Templates.ContainsKey(persona))
                persona = "base";

            var template = PrompetTemp.Templates[persona]
                .Replace("{{ include_base }}", PrompetTemp.Templates["base"])
                ;

            template = template
                .Replace("{{ user_prompt }}", userInput)
                .Replace("{{ length }}", length.ToString());

            // simulate basic {% set %} replacement
            template = template
                .Replace("{% set role = '", "")
                .Replace("{% set style = '", "")
                .Replace("{% set tone = '", "")
                .Replace("' %}", "");

            return template;
        }
    }
}

