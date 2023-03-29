using SpeechGPT.Domain.Enums;
using System.Numerics;

namespace SpeechGPT.Domain
{
    public class User
    {
        public int Id { get; set; }

        public string UserName { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }

        public UserRole UserRole { get; set; }

        public bool EmailConfirmed { get; set; }

        public ConfirmEmailCode? ConfirmEmailCode { get; set; }


        public List<Chat>? Chats { get; set; }
    }
}