
using System.Numerics;

namespace SpeechGPT.Domain
{
    public class Response
    {
        public BigInteger Id { get; set; }
        public string Body { get; set; }
        public DateTime CreatedAt { get; set; }

        public User User { get; set; }
    }
}
