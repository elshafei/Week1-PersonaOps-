namespace Day1proj.Models
{
    public class GenRequest
    {
        public string InputText { get; set; } = "";
        public string Persona { get; set; } = "scholarship";
        public string Audience { get; set; } = "students";
        public int Length { get; set; } = 120;
        public string Provider { get; set; } = "huggingface";
        public string Model { get; set; } = "openai/gpt-oss-20b";
        public List<string> Context { get; set; } = new();
    }
}
