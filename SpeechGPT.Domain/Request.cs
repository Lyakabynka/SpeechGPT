
using System.Numerics;

namespace SpeechGPT.Domain
{
    public class Request
    {
        public int Id { get; set; }
        public string Body { get; set; }
        public DateTime CreatedAt { get; set; }

        public User User { get; set; }
    }
}
