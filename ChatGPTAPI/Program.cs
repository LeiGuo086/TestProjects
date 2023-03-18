using Microsoft.AspNetCore.Mvc;
using OpenAI.GPT3.Managers;
using OpenAI.GPT3.ObjectModels.RequestModels;
using OpenAI.GPT3.ObjectModels;
using OpenAI.GPT3;
using System.Text;

const string OPENAPI_TOKEN = "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";

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapGet("/GetChatGPT", (string Prompt) =>
{
    return Task.FromResult(ChatGPTAsync(Prompt));
});

async Task<string> ChatGPTAsync(string prompt)
{
    OpenAIService service = new OpenAIService(new OpenAiOptions() { ApiKey = OPENAPI_TOKEN });
    CompletionCreateRequest createRequest = new CompletionCreateRequest()
    {

        Prompt = prompt,
        Temperature = 1.0f,
        MaxTokens = 1000
    };

    var res = await service.Completions.CreateCompletion(createRequest, Models.TextDavinciV3);

    if (res.Successful)
    {
        return res.Choices.FirstOrDefault().Text;
    }

    return "";
}

app.Run();
