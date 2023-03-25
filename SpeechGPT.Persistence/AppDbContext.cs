using Microsoft.EntityFrameworkCore;
using SpeechGPT.Application.Interfaces;
using SpeechGPT.Domain;
using SpeechGPT.Persistence.EntityTypeConfigurations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpeechGPT.Persistence
{
    public class AppDbContext : DbContext, IAppDbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Request> Requests { get; set; }
        public DbSet<Response> Responses { get; set; }
        public DbSet<ConfirmEmailCode> ConfirmEmailCodes { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) 
            : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasOne(u => u.ConfirmEmailCode)
                .WithOne(c => c.User)
                .HasForeignKey<ConfirmEmailCode>("UserId");

            modelBuilder.ApplyConfiguration(new UserConfiguration());
            base.OnModelCreating(modelBuilder);
        }
    }
}
