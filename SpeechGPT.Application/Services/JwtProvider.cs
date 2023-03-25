
using Microsoft.IdentityModel.Tokens;
using SpeechGPT.Application.Configurations;
using SpeechGPT.Domain;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SpeechGPT.Application.Services
{
    public class JwtProvider
    {
        private readonly JwtProviderConfiguration _configuration;
        public JwtProvider(JwtProviderConfiguration configuration) =>
            _configuration = configuration;
            
        public string CreateToken(User user)
        {
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Role, user.Role.ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.Key));

            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha384Signature);

            var token = new JwtSecurityToken(
                _configuration.Issuer,
                _configuration.Audience,
                claims: claims,
                expires: DateTime.Now.AddMinutes(_configuration.MinutesToExpiration),
                signingCredentials: credentials
                );

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;
        }
    }
}
