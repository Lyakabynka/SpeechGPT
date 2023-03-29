using SpeechGPT.Domain.Enums;

namespace SpeechGPT.Domain
{
    public class Message
    {
        public int Id { get; set; }
        public string Body { get; set; }
        public DateTime CreatedAt { get; set; }

        public MessageType MessageType { get; set; }

        public int ChatId { get; set; }
        public Chat Chat { get; set; }
    }
}
