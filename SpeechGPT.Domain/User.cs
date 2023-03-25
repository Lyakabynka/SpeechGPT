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
        public int Id { get; set; }

        public string UserName { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }

        public Role Role { get; set; }

        public bool EmailConfirmed { get; set; }

        public ConfirmEmailCode? ConfirmEmailCode { get; set; }


        public List<Request> Requests { get; set; }
        public List<Response> Responses { get; set; }
    }
}