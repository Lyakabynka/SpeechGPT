using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.OpenApi.Models;
using SpeechGPT.Application;
using SpeechGPT.Application.Common.Mappings;
using SpeechGPT.Persistence;
using SpeechGPT.WebApi;
using SpeechGPT.WebApi.Middleware;
using Swashbuckle.AspNetCore.Filters;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

//builder.Configuration.AddEnvironmentVariables();

// TO Inject the JwtProviderConfiguration and
// Assign the values from section 'Jwt' to its properties
builder.Services.AddCustomConfigurations(builder.Configuration);

builder.Services.AddControllers();

builder.Services.AddAuthentication()
    .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme);

builder.Services.AddPersistence(builder.Configuration);
builder.Services.AddApplication();

builder.Services.AddAutoMapper(config =>
{
    config.AddProfile(new AssemblyMappingProfile(Assembly.GetExecutingAssembly()));

    config.AddProfile(new AssemblyMappingProfile(typeof(IMappable<>).Assembly));
});

builder.Services.AddSwaggerGen(config =>
{
    config.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey
    });

    config.OperationFilter<SecurityRequirementsOperationFilter>();

    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    config.IncludeXmlComments(xmlPath);
});

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(config =>
{
    // show swagger page using root Uri
    config.RoutePrefix = string.Empty;

    config.SwaggerEndpoint("swagger/v1/swagger.json", "SpeechGPT.Backend");
});
app.UseCustomExceptionHandler();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
