
namespace SpeechGPT.Domain
{
    public class ConfirmEmailCode
    {
        public int Id { get; set; }
        public Guid Code { get; set; } = Guid.NewGuid();
        
        public User User { get; set; }
    }
}
