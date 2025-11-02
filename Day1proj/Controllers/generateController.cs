using Day1proj.Configs;
using Day1proj.Models;
using Day1proj.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace Day1proj.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class generateController : ControllerBase
    {
        private readonly HuggingFaceSettings _settings;
        private readonly HuggingFaceService _hf;

        public generateController(IOptions<HuggingFaceSettings> settings)
        {
            _settings = settings.Value;
            _hf = new(settings);
        }


        [HttpPost]
        public async Task<ActionResult<GenResponse>> Post([FromBody] GenRequest req)
        {
            var clean = PiiRedactor.Redact(req.InputText);
            req.InputText = clean;

            var result = await _hf.GenerateAsync(req);

            Directory.CreateDirectory("Day1Lab.API/logs");
            await System.IO.File.AppendAllTextAsync("Day1Lab.API/logs/runs.jsonl",
                JsonConvert.SerializeObject(new
                {
                    req.Provider,
                    req.Model,
                    result.Latency_Ms,
                    result.Cost_Est,
                    result.Request_Id
                }) + Environment.NewLine);

            return Ok(result);
        }
    }
}
