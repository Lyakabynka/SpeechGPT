using Microsoft.EntityFrameworkCore;
using SpeechGPT.Domain;

namespace SpeechGPT.Application.Interfaces
{
    public interface IAppDbContext
    {
        DbSet<User> Users { get; set; }
        DbSet<Request> Requests { get; set; }
        DbSet<Response> Responses { get; set; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
