using SpeechGPT.Application;
using SpeechGPT.Persistence;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddPersistence(builder.Configuration);
builder.Services.AddApplication();


var app = builder.Build();

app.MapControllers();

app.Run();
