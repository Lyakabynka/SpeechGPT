
namespace SpeechGPT.Domain
{
    public class Chat
    {
        public int Id { get; set; }

        public string Name { get; set; } = "New Chat";

        public List<Message>? Messages { get; set; }
        
        public int UserId { get; set; }
        public User User { get; set; }
    }
}
