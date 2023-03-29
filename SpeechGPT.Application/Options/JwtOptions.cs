
namespace SpeechGPT.Application.Options
{
    public class JwtOptions
    {
        public static string JwtSection = "Jwt";

        public string SecretKey { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public int MinutesToExpiration { get; set; }
    }
}
