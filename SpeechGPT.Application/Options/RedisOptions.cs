namespace SpeechGPT.Application.Options;

public class RedisOptions
{
    public static readonly string RedisSection = "Redis";
    
    public string Url { get; set; }
}