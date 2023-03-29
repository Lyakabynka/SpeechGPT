using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace SpeechGPT.Application.Options.OptionSetups
{
    /// <summary>
    /// Used to configure .AddJwtBearer in Program.cs
    /// Injects JwtOptions
    /// </summary>
    public class JwtBearerOptionsSetup : IConfigureNamedOptions<JwtBearerOptions>
    {
        private readonly JwtOptions _jwtOptions;
        public JwtBearerOptionsSetup(IOptions<JwtOptions> jwtOptions) =>
            _jwtOptions = jwtOptions.Value;

        public void Configure(JwtBearerOptions options)
        {
            Configure(string.Empty, options);
        }

        public void Configure(string? name, JwtBearerOptions options)
        {
            options.TokenValidationParameters = new TokenValidationParameters()
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = _jwtOptions.Issuer,
                ValidAudience = _jwtOptions.Audience,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.SecretKey)),
                ClockSkew = TimeSpan.Zero
            };
            
        }
    }
}
