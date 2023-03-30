namespace SpeechGPT.Application.Interfaces
{
    public interface IChatGPT
    {
        Task<string> GetResponse(string requestBody);
    }
}
