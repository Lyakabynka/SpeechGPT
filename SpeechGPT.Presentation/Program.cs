using SpeechGPT.Application;
using SpeechGPT.Application.Common.Mappings;
using SpeechGPT.Persistence;
using SpeechGPT.WebApi.Middleware;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddPersistence(builder.Configuration);
builder.Services.AddApplication();

builder.Services.AddAutoMapper(config =>
{
    config.AddProfile(new AssemblyMappingProfile(Assembly.GetExecutingAssembly()));

    config.AddProfile(new AssemblyMappingProfile(typeof(IMappable<>).Assembly));
});



var app = builder.Build();

app.MapControllers();

app.UseCustomExceptionHandler();

app.Run();
