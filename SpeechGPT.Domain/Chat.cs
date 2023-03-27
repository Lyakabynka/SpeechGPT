
namespace SpeechGPT.Domain
{
    public class Chat
    {
        public int Id { get; set; }
        public List<Request> Requests { get; set; }
        public List<Response> Responses { get; set; }
    }
}
