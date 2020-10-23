using Microsoft.EntityFrameworkCore;

namespace MassTransitUi.Models
{
    public class MassTransitUiContext : DbContext
    {
        public MassTransitUiContext(DbContextOptions<MassTransitUiContext> options) : base(options)
        {
        }

        public DbSet<FailedMessage> FailedMessages { get; set; }
        //public DbSet<FailedMessageHeader> FailedMessageHeaders { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<FailedMessage>()
                .HasMany(e => e.Headers)
                //.WithOne(h => h.FailedMessage)
                .WithOne()
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired(true);
        }
    }
}
