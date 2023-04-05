using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.OpenApi.Models;
using SpeechGPT.Application;
using SpeechGPT.Application.Common.Mappings;
using SpeechGPT.Persistence;
using SpeechGPT.WebApi;
using SpeechGPT.WebApi.Middleware;
using Swashbuckle.AspNetCore.Filters;
using System.Reflection;
using StackExchange.Redis;
using SpeechGPT.Application.Options;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer();

builder.Services.AddPersistence(builder.Configuration);
builder.Services.AddApplication();

builder.Services.AddCors(options =>
{
    // All clients (temporary)
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyHeader();
        policy.AllowAnyMethod();
        policy.AllowAnyOrigin();
    });
});

builder.Services.AddStackExchangeRedisCache(config =>
{
    var redisSection = builder.Configuration.GetRequiredSection("Redis");

    config.Configuration = redisSection["Configuration"];
    config.InstanceName = redisSection["InstanceName"];
});



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

//Configures options inside .AddJwtBearer(options)
builder.Services.AddCustomConfigurations(builder.Configuration);

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

// Activate "AllowAll" CORS policy
app.UseCors("AllowAll");

app.MapControllers();

app.Run();
