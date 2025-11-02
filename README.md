# Week1-PersonaOps-
Build and ship a production-ready content generation microservice and CLI that leverage prompt patterns to produce persona-controlled, structured JSON outputs — with built-in safety, observability, and lightweight evaluation harness.

# 🧠 Day1proj"LLM microservice" — PersonaOps (C# .NET 9)

A practical lab project demonstrating how to build an **LLM-powered microservice and CLI tool** with persona-based content generation.  
The system integrates a REST API (ASP.NET Web API) and a CLI client (C# console app) to generate structured responses from large language models such as **Hugging Face’s GPT-OSS-20B**.

---

## 🚀 Project Overview

**Day1proj** simulates a small-scale *PersonaOps* system — where each persona (e.g., `witty_marketer`, `serious_academic`) has a unique writing tone and structure.  
The app sends user prompts to an LLM provider (Hugging Face endpoint by default) and formats the model output into a structured JSON response.

---

## 🧩 Architecture



Day1proj/
├── Day1proj.API/ → ASP.NET Web API project
│ ├── Controllers/GenerateController.cs
│ ├── Services/HuggingFaceService.cs  /JsonExtractor.cs
│ ├── PromptTemplte/PrompetTemp.cs  ->TemplateFactory
│ ├── Models/GenRequest.cs / GenResponse.cs
│ └── appsettings.json
│
├── Day1proj.Week1CLI/ → CLI client using System.CommandLine
  ├── CLIService.cs
  └── Program.cs



---

## ⚙️ Setup Instructions

### 1️⃣ Prerequisites
- [.NET 9 SDK](https://dotnet.microsoft.com/download)
- A [Hugging Face](https://huggingface.co/) account and access token
- Internet connection to reach the API endpoint

### 2️⃣ Configure API
Open `appsettings.json` inside `Day1proj.API` and set your Hugging Face token:
```json
{
  "HuggingFace": {
    "Endpoint": "https://router.huggingface.co/v1/chat/completions",
    "Model": "openai/gpt-oss-20b",
    "Token": "your_hf_token_here"
  }
}

3️⃣ Run the API
cd Day1proj
dotnet run


API will start (default: [https://localhost:7288])

🧰 CLI Usage

You can generate structured content using:

dotnet run --project Day1proj.CLI -- "write about AI for students" witty_marketer

Example Output
{
  "title": "AI for Students",
  "body": "Artificial Intelligence is reshaping how students learn and create...",
  "style": "witty_marketer",
  "citations": ["kb://brand_guide#messaging"],
  "moderation_flags": [],
  "tokens": { "prompt": 321, "completion": 187 },
  "latency_ms": 642,
  "cost_est": 0.0031,
  "request_id": "b9a12e42-1f33-43c8-98b0-61a7c08b3af6"
}

🧱 Personas & Templates

Each persona has a different tone:

Persona	Description
witty_marketer	funny, clever, persuasive tone
serious_academic	formal, structured, intellectual tone
helpful_teacher	Clear, simple explanations


All templates inherit from a base layout but apply distinct tone and structure in C# via TemplateFactory.

🧾 API Endpoint

POST /api/generate
Body:

{
  "input_text": "Explain AI for beginners",
  "persona": "helpful_teacher",
  "audience": "students",
  "model": "openai/gpt-oss-20b"
}


Response:

{
  "title": "Introduction to AI",
  "body": "AI stands for Artificial Intelligence...",
  "style": "helpful_teacher",
  "citations": [],
  "moderation_flags": [],
  "tokens": {"prompt": 150, "completion": 120},
  "latency_ms": 430,
  "cost_est": 0.0019,
  "request_id": "uuid"
}

📦 Output

All CLI outputs are saved under:

outputs/{guid}.json

🧩 Tech Stack

.NET 9 (C#)

ASP.NET Web API

System.CommandLine (CLI)

Newtonsoft.Json

HttpClient

Hugging Face Inference API

