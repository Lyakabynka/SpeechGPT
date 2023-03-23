using System.Numerics;

namespace SpeechGPT.Domain
{
    public enum Role
    {
        User,
        Admin
    }

    public class User
    {
        public BigInteger Id { get; set; }

        public string Nickname { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }

        public Role Role { get; set; }
    }
}